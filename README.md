# SteamStorage

Desktop application for tracking Steam inventory investments — actives, archives, and market dynamics.

Built with [Avalonia UI](https://avaloniaui.net/) and [SteamStorageAPI.SDK](https://github.com/AMUDENN/SteamStorageAPI).

---

## Features

- **Home** — aggregated statistics: total invested sum, financial goal progress, item counts, server ping indicator, quick item list with "add to Actives / Archive" shortcuts
- **Actives** — open positions grouped by category: buy price, current market price, profit/loss (absolute and percent), price dynamics chart per period
- **Archives** — history of sold items with final profit calculation, grouped by category
- **Inventory** — full Steam inventory with per-game statistics and one-click refresh
- **Profile** — user info pulled from Steam, preferred currency, start page selection, account management
- **Settings** — light/dark theme switching, Excel export, reference information from API

---

## Tech Stack

| | |
|---|---|
| UI Framework | Avalonia 11 (cross-platform, compiled XAML bindings) |
| Architecture | MVVM — CommunityToolkit.Mvvm |
| Charts | LiveChartsCore.SkiaSharpView.Avalonia |
| DI | Microsoft.Extensions.DependencyInjection |
| API Client | SteamStorageAPI.SDK 2.0.0 |
| Image Loading | AsyncImageLoader.Avalonia |
| Runtime | .NET 10, C# 14 |

---

## Architecture

### Overview

```
┌──────────────────────────────────────────────────┐
│  Views  (.axaml / .axaml.cs)                     │
│  Pure UI — compiled bindings, zero logic         │
└───────────────┬──────────────────────────────────┘
                │ DataContext (ViewLocator convention)
┌───────────────▼──────────────────────────────────┐
│  ViewModels  (.cs)                               │
│  Thin wrappers — expose Model properties/cmds    │
└───────────────┬──────────────────────────────────┘
                │ constructor injection
┌───────────────▼──────────────────────────────────┐
│  Models  (.cs)                                   │
│  Business logic, API calls, observable state     │
└───────────────┬──────────────────────────────────┘
                │ IApiClient / SDK services
┌───────────────▼──────────────────────────────────┐
│  SteamStorageAPI.SDK                             │
│  HTTP client, auth, ping, reference info         │
└──────────────────────────────────────────────────┘
```

All bindings use Avalonia's **compiled binding** mode (`AvaloniaUseCompiledBindingsByDefault=true`), which resolves binding paths at compile time and eliminates reflection overhead at runtime.

---

### MVVM

**Views** contain no logic. Interaction is limited to Behaviors (attached via `Avalonia.Xaml.Behaviors`) and minimal code-behind for focus or clipboard operations that cannot be expressed in XAML.

**ViewModels** are thin: they hold a reference to the corresponding Model, expose its properties through forwarding properties, and forward commands. The split exists so that the DI container can manage lifetimes independently and so that ViewModels remain testable without UI.

**Models** extend `ModelBase` (`ObservableObject` from CommunityToolkit.Mvvm). They own the business logic, issue API calls, and raise `PropertyChanged` notifications. Heavy async operations use `async/await` directly on model methods; cancellation tokens are managed via `CancellationTokenSource` fields reset on each new request.

**ViewLocator** resolves Views from ViewModels by naming convention:

```
SteamStorage.ViewModels.Actives.ActivesViewModel
         → SteamStorage.Views.Actives.ActivesView
```

The suffix `ViewModel` is replaced with `View` on the fully-qualified type name, and the resulting type is instantiated via `Activator.CreateInstance`. No manual registration is needed.

---

### Model hierarchy

```
ModelBase  (ObservableObject)
│
├── BaseListModel
│   │  Pagination state: PageNumber, PagesCount, PageSize,
│   │  DisplayItemsCountStart/End, SavedItemsCount, IsLoading
│   │  Abstract GetSkinsAsync() triggers reload on page change
│   │
│   ├── ListActivesModel       — paginated active positions list
│   ├── ListArchivesModel      — paginated archive list
│   └── InventoryModel         — Steam inventory with per-game grouping
│
├── BaseEditModel
│   │  CRUD commands: BackCommand, DeleteCommand, SaveCommand
│   │  Typed events: GoingBack, ItemDeleted, ItemChanged
│   │  Holds IApiClient, IDialogService, INotificationService
│   │
│   ├── BaseGroupEditModel
│   │   │  Group title, description, color picker, IsNewGroup flag
│   │   ├── ActiveGroupEditModel
│   │   └── ArchiveGroupEditModel
│   │
│   └── BaseItemEditModel
│       │  Skin autocomplete: Filter, SkinModels list,
│       │  AsyncPopulator (debounced 400 ms), ItemFilter predicate
│       │  Cancels in-flight search on each new keystroke
│       ├── ActiveEditModel
│       └── ArchiveEditModel
│
└── domain models (singletons, shared across the app)
    ├── UserModel              — current Steam user, fires UserChanged
    ├── CurrenciesModel        — available currencies list
    ├── GamesModel             — games catalog
    ├── PagesModel             — available start pages
    ├── PeriodsModel           — chart time periods (7d / 30d / 90d / …)
    ├── ChartTooltipModel      — shared chart tooltip state
    ├── StatisticsModel        — home page aggregate numbers
    ├── HomeModel              — home page orchestration
    ├── ActiveGroupsModel      — actives group list + group CRUD
    ├── ActivesModel           — actives page orchestration
    ├── ActivesReviewModel     — single active detail/review view
    ├── ActiveSoldModel        — "mark as sold" flow
    ├── ArchiveGroupsModel
    ├── ArchivesModel
    ├── ArchivesReviewModel
    ├── ProfileModel
    ├── SettingsModel
    └── MainModel              — navigation, auth state, sidebar
```

#### Utility models

Item-level models have their own hierarchy:

```
ModelBase
└── BaseSkinModel
    │  SkinId, ImageUrl, MarketUrl, Title
    │  OpenInSteamCommand → opens MarketUrl in browser
    │
    └── BaseDynamicsSkinModel
        │  Price dynamics chart: ChangeSeries, XAxis, YAxis
        │  Period selector: SelectedPeriodModel → fetches SkinDynamics
        │  Theme-aware colors via IThemeService.ChartThemeChanged
        │
        ├── ActiveModel      — BuyPrice, CurrentPrice, Change, Count, …
        └── ArchiveModel     — BuyPrice, SoldPrice, FinalProfit, …
```

Group-level models:

```
ModelBase
└── BaseGroupModel           — Id, Title, Color, IsAllGroup flag
    └── ExtendedBaseGroupModel
        │  Count, BuySum, CurrentSum, Change aggregate stats
        ├── ActiveGroupModel
        └── ArchiveGroupModel
```

---

### ViewModel hierarchy

```
ViewModelBase
│
├── BaseListViewModel         — wraps BaseListModel, forwards pagination
├── BaseEditViewModel
│   ├── BaseGroupEditViewModel
│   └── BaseItemEditViewModel
│
├── page ViewModels           — HomeViewModel, ActivesViewModel, …
└── utility ViewModels
    ├── BaseSkinViewModel     — wraps BaseSkinModel
    │   └── BaseDynamicsSkinViewModel
    │       ├── ActiveViewModel
    │       └── ArchiveViewModel
    ├── BaseGroupViewModel    — wraps BaseGroupModel
    │   └── BaseExtendedGroupViewModel
    │       ├── ActiveGroupViewModel
    │       └── ArchiveGroupViewModel
    ├── InventoryItemViewModel
    └── ListItemViewModel
```

---

### Dependency Injection

All registration happens in `App.axaml.cs` (`GetServiceCollection()`). The container is built **once** in the static constructor and stored in a private `Container` property. A public `App.GetService<T>()` helper provides access where constructor injection is not available (e.g., ViewLocator).

Registration scopes:

| Scope | Used for |
|---|---|
| `Singleton` | All Models, ViewModels, windows, `INotificationService`, `ISettingsService` |
| `Scoped` | `IThemeService`, `IClipboardService`, `IDialogService` |

SDK services are registered via extension methods from the SDK:

```csharp
services.AddSteamStorageApi(options => { });
services.AddSteamStorageAuthorizationService(options => { });
services.AddSteamStoragePingService(options => { });
services.AddSteamStorageReferenceInformationService();
```

`SettingsService` is registered manually so its constructor can receive `programName` as a string argument alongside injected services:

```csharp
services.AddSingleton<ISettingsService, SettingsService>(x =>
    new(ProgramConstants.PROGRAM_NAME,
        x.GetRequiredService<IApiClient>(),
        x.GetRequiredService<IThemeService>()));
```

Unhandled UI-thread exceptions are caught globally in `Dispatcher.UIThread.UnhandledException` and routed to `INotificationService` as toast notifications.

---

### Services

| Interface | Scope | Responsibility |
|---|---|---|
| `IApiClient` | SDK singleton | HTTP client for all API calls, owns auth token |
| `IAuthorizationService` | SDK | Steam OAuth flow, fires `AuthorizationCompleted` / `LogOutCompleted` |
| `IPingService` | SDK | Periodic server availability check, updates ping indicator |
| `IReferenceInformationService` | SDK | Fetches API reference docs displayed in Settings |
| `ISettingsService` | Singleton | Reads/writes `UserSettings`; listens to `IApiClient.TokenChanged` and `IThemeService.ThemeChanged` to persist changes automatically |
| `IThemeService` | Scoped | Switches Avalonia `ThemeVariant`; fires `ThemeChanged` and `ChartThemeChanged` |
| `IDialogService` | Scoped | Shows `DialogWindow` with arbitrary content ViewModel |
| `INotificationService` | Singleton | Toast-style overlay notifications via `ShowAsync(title, message)` |
| `IClipboardService` | Scoped | Clipboard read/write using the Avalonia `TopLevel` clipboard API |

---

### Settings persistence

`UserSettings` carries the auth token and the selected theme variant. It implements `INotifyPropertyChanged` so that `SettingsService` can react to any property change and immediately persist to disk.

Storage path:

```
%APPDATA%\SteamStorage\appSettings.json   # Windows
~/.config/SteamStorage/appSettings.json  # Linux / macOS
```

`SettingsFile<T>` wraps plain `JsonSerializer` read/write. `SettingsService` wires up three event subscriptions in its constructor:

```
IApiClient.TokenChanged   → UserSettings.Token  = token
IThemeService.ThemeChanged → UserSettings.Theme  = theme
UserSettings.PropertyChanged → SettingsFile.Write(UserSettings)
```

On dispose (application shutdown) it performs a final save.

---

### Navigation

`MainModel` owns a fixed `NavigationOptions` list built in the constructor:

```csharp
NavigationOptions =
[
    new("HomeVectorImage",      "Главная",   homeViewModel),
    new("ActivesVectorImage",   "Активы",    activesViewModel),
    new("ArchiveVectorImage",   "Архив",     archivesViewModel),
    new("InventoryVectorImage", "Инвентарь", inventoryViewModel),
    new("ProfileVectorImage",   "Профиль",   profileViewModel)
];
```

`SelectedNavigationModel` setter updates `CurrentViewModel`, which the main view binds to via `ContentControl`. Settings opens via a separate boolean toggle (`IsSettingsChecked`) that deselects the navigation list.

After login (`UserModel.UserChanged`) the model navigates to the page stored as the user's `StartPage` preference.

#### Cross-page drill-down via typed events

Drill-down navigation (e.g., Home → add item to Actives) avoids a router. Models raise typed events with `EventArgs` carrying the required context:

```
Actives domain
  AddActiveEventArgs, EditActiveEventArgs, EditActiveGroupEventArgs,
  OpenActivesEventArgs, SoldActiveEventArgs

Archives domain
  AddArchiveEventArgs, EditArchiveEventArgs, EditArchiveGroupEventArgs,
  OpenArchivesEventArgs

Home / cross-domain
  AddToActivesEventArgs, AddToArchiveEventArgs

Settings domain
  ThemeChangedEventArgs, ChartThemeChangedEventArgs,
  SettingsPropertyChangedEventArgs
```

`MainModel` subscribes to `ListItemsModel.AddToActives` and `AddToArchive` to switch the active tab, and saves `_lastNavigationModel` for the `GoingBack` event to return the user to the originating page.

---

### Charts

LiveChartsCore renders `LineSeries<SkinDynamicResponse>` for price dynamics. Each data point maps `(index, price)` via a custom `Mapping` lambda; tooltip labels format date and price.

Colors are theme-aware. `IThemeService` fires both `ThemeChanged` (Avalonia `ThemeVariant`) and `ChartThemeChanged` (SkiaSharp color palette). `BaseDynamicsSkinModel` subscribes to `ChartThemeChanged` and rebuilds `ChangeSeries` with the appropriate `SolidColorPaint`:

- Positive period change → `ChartThemeVariants.ChartColors.Positive`
- Negative period change → `ChartThemeVariants.ChartColors.Negative`

Axis labels and separators are hidden for a minimal look (`LabelsPaint = null`, `SeparatorsPaint = null`). Y-axis limits are padded to ±10% of the data range so the line never touches the chart edges.

---

### Custom controls and resources

#### Controls

| Control | Purpose |
|---|---|
| `VectorImage` | Renders SVG paths embedded as AXAML `DrawingImage` resources; used for all UI icons |
| `AdvancedNumericUpDown` | `NumericUpDown` with extra validation and formatting options |
| `AdvancedTextBox` | `TextBox` with configurable empty-state placeholder styling |
| `DefaultContentControl` | `ContentControl` that shows a placeholder when `Content` is null |

#### Behaviors (Avalonia.Xaml.Behaviors)

| Behavior | Purpose |
|---|---|
| `MouseScrollIgnoreBehavior` | Stops scroll events from propagating out of a control (used on charts and numeric inputs) |
| `ControlRightButtonPressedIgnoreBehavior` | Suppresses right-click context menus on specific controls |
| `TextBlockBehavior` | Enables automatic `ToolTip` when text is trimmed/clipped |

#### Value converters

| Converter | Input → Output |
|---|---|
| `PercentConverter` | `decimal` → `"12.34%"` string |
| `MarkedPercentConverter` | `decimal` → `"+12.34%"` / `"-12.34%"` with sign |
| `BrushConverter` | `Color` / `string` → `ISolidColorBrush` |
| `DoubleGreaterConverter` | `(double, double)` → `bool` (a > b) |
| `DoubleLessConverter` | `(double, double)` → `bool` (a < b) |
| `DoubleMultiplicationConverter` | `(double, double)` → `double` (a × b) |

---

## Project structure

```
SteamStorage/
├── Models/
│   ├── Tools/
│   │   ├── ModelBase.cs
│   │   ├── BaseModels/
│   │   │   ├── BaseListModel.cs
│   │   │   ├── BaseEditModel.cs
│   │   │   ├── BaseGroupEditModel.cs
│   │   │   ├── BaseItemEditModel.cs
│   │   │   └── BaseDialogModel.cs
│   │   └── UtilityModels/
│   │       ├── BaseModels/
│   │       │   ├── BaseSkinModel.cs
│   │       │   ├── BaseDynamicsSkinModel.cs
│   │       │   ├── BaseGroupModel.cs
│   │       │   └── ExtendedBaseGroupModel.cs
│   │       ├── ActiveModel.cs / ActiveGroupModel.cs
│   │       ├── ArchiveModel.cs / ArchiveGroupModel.cs
│   │       ├── InventoryItemModel.cs
│   │       ├── ListItemModel.cs
│   │       ├── NavigationModel.cs / SecondaryNavigationModel.cs
│   │       ├── CurrencyModel.cs / GameModel.cs
│   │       ├── PageModel.cs / PeriodModel.cs / ThemeModel.cs
│   ├── Actives/                # ActiveEditModel, ActiveGroupEditModel,
│   │                           # ActiveGroupsModel, ActivesModel,
│   │                           # ActiveSoldModel, ActivesReviewModel,
│   │                           # ListActivesModel
│   ├── Archives/               # mirror of Actives/
│   ├── Dialog/                 # MessageDialogModel, TextConfirmDialogModel
│   ├── Home/                   # HomeModel, ListItemsModel, StatisticsModel
│   ├── Inventory/              # InventoryModel
│   ├── Profile/                # ProfileModel
│   ├── Settings/               # SettingsModel
│   ├── Windows/                # MainWindowModel, DialogWindowModel
│   ├── MainModel.cs
│   ├── UserModel.cs
│   ├── CurrenciesModel.cs / GamesModel.cs / PagesModel.cs
│   ├── PeriodsModel.cs / ChartTooltipModel.cs
│
├── ViewModels/                 # mirrors Models/ structure
│   ├── Tools/
│   │   ├── ViewModelBase.cs
│   │   ├── BaseViewModels/     # BaseListViewModel, BaseEditViewModel, …
│   │   └── UtilityViewModels/  # BaseSkinViewModel, BaseGroupViewModel, …
│   └── …
│
├── Views/                      # mirrors Models/ structure (.axaml + .axaml.cs)
│
├── Services/
│   ├── Settings/
│   │   ├── SettingsFile/       # SettingsFile<T> — JSON read/write
│   │   ├── SettingsService/    # ISettingsService, SettingsService
│   │   └── UserSettings/       # UserSettings (Token, Theme)
│   ├── ThemeService/           # IThemeService, ThemeService
│   ├── DialogService/          # IDialogService, DialogService
│   ├── NotificationService/    # INotificationService, NotificationService
│   └── ClipboardService/       # IClipboardService, ClipboardService
│
├── Resources/
│   ├── Controls/               # VectorImage, AdvancedNumericUpDown, …
│   ├── Behaviors/              # scroll/click suppression, TextBlock tooltip
│   ├── Converters/             # value converters
│   └── Images/
│       ├── Bitmaps/            # .ico icons (24, 32, 256 px)
│       └── Vectors/            # .axaml SVG DrawingImage resources
│
├── Utilities/
│   ├── Events/
│   │   ├── Actives/            # AddActiveEventArgs, EditActiveEventArgs, …
│   │   ├── Archives/           # AddArchiveEventArgs, …
│   │   ├── ListItems/          # AddToActivesEventArgs, AddToArchiveEventArgs
│   │   └── Settings/           # ThemeChangedEventArgs, …
│   ├── ThemeVariants/          # ChartThemeVariant, ChartColor, ThemeVariants
│   ├── Extensions/             # ColorExtension, MathExtension, StringExtension
│   ├── ProgramConstants.cs
│   ├── MathUtility.cs
│   ├── UrlUtility.cs
│   ├── DialogUtility.cs
│   └── ViewLocator.cs
│
├── App.axaml / App.axaml.cs   # DI wiring, global exception handler
└── SteamStorage.csproj
```

---

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- A running [SteamStorageAPI](https://github.com/AMUDENN/SteamStorageAPI) server

---

## Build

```bash
dotnet build SteamStorage/SteamStorage.csproj
```

## Publish

```bash
# Windows x64
dotnet publish SteamStorage/SteamStorage.csproj -c Release -r win-x64 --self-contained

# Linux x64
dotnet publish SteamStorage/SteamStorage.csproj -c Release -r linux-x64 --self-contained

# macOS arm64
dotnet publish SteamStorage/SteamStorage.csproj -c Release -r osx-arm64 --self-contained
```

---

## CI

GitHub Actions workflow builds and publishes self-contained binaries for all three platforms on every push to `master`. Artifacts are attached to the workflow run.

---

## License

MIT
