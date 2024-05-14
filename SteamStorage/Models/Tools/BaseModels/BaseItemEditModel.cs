using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using SteamStorage.Services.DialogService;
using SteamStorage.Services.NotificationService;
using SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models.Tools.BaseModels;

public abstract class BaseItemEditModel : BaseEditModel
{
    #region Fields
    
    private static readonly char[] _separator = [' '];
    
    private bool _isNewItem;

    private BaseSkinViewModel? _defaultSkinModel;
    private BaseSkinViewModel? _selectedSkinModel;
    private string? _filter;
    private AutoCompleteFilterPredicate<object?>? _itemFilter;
    private Func<string?, CancellationToken, Task<IEnumerable<object>>>? _asyncPopulator;
    private List<BaseSkinViewModel> _skinModels;

    private CancellationTokenSource _cancellationTokenSource;

    #endregion Fields

    #region Properties
    
    public bool IsNewItem
    {
        get => _isNewItem;
        protected set => SetProperty(ref _isNewItem, value);
    }

    public BaseSkinViewModel? DefaultSkinModel
    {
        get => _defaultSkinModel;
        protected set => SetProperty(ref _defaultSkinModel, value);
    }

    public BaseSkinViewModel? SelectedSkinModel
    {
        get => _selectedSkinModel;
        set
        {
            SetProperty(ref _selectedSkinModel, value);
            SaveCommand.NotifyCanExecuteChanged();
            SetTitle(value);
        }
    }

    public string? Filter
    {
        get => _filter;
        set
        {
            SetProperty(ref _filter, value);
            if (!string.IsNullOrWhiteSpace(value)) GetSkinsAsync();
        }
    }

    public AutoCompleteFilterPredicate<object?>? ItemFilter
    {
        get => _itemFilter;
        private set => SetProperty(ref _itemFilter, value);
    }

    public Func<string?, CancellationToken, Task<IEnumerable<object>>>? AsyncPopulator
    {
        get => _asyncPopulator;
        private set => SetProperty(ref _asyncPopulator, value);
    }

    public List<BaseSkinViewModel> SkinModels
    {
        get => _skinModels;
        private set => SetProperty(ref _skinModels, value);
    }

    private CancellationTokenSource CancellationTokenSource
    {
        get => _cancellationTokenSource;
        set => SetProperty(ref _cancellationTokenSource, value);
    }

    #endregion Properties

    #region Constructor

    protected BaseItemEditModel(
        ApiClient apiClient,
        IDialogService dialogService,
        INotificationService notificationService) : base(apiClient, dialogService, notificationService)
    {
        _skinModels = [];
        _cancellationTokenSource = new();

        ItemFilter = ItemFilterPredicate;
        AsyncPopulator = PopulateAsync;

        GoingBack += GoingBackHandler;
    }

    #endregion Constructor

    #region Methods

    private void GoingBackHandler(object? sender)
    {
        Filter = null;
    }

    protected abstract void SetTitle(BaseSkinViewModel? model);

    private bool ItemFilterPredicate(string? search, object? item)
    {
        return item is not null && MatchFilter((BaseSkinViewModel)item, search);
    }

    private async Task<IEnumerable<object>> PopulateAsync(string? searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(0.4), cancellationToken);

        return
            SkinModels.Where(data => MatchFilter(data, searchText))
                .ToList();
    }

    private async void GetSkinsAsync()
    {
        await CancellationTokenSource.CancelAsync();

        CancellationTokenSource = new();
        CancellationToken token = CancellationTokenSource.Token;

        Skins.BaseSkinsResponse? skinsResponse =
            await ApiClient.GetAsync<Skins.BaseSkinsResponse, Skins.GetBaseSkinsRequest>(
                ApiConstants.ApiMethods.GetBaseSkins,
                new(Filter),
                token);

        if (skinsResponse?.Skins is null) return;

        SkinModels = skinsResponse.Skins.Select(x =>
                new BaseSkinViewModel(new(x.Id,
                    x.SkinIconUrl,
                    x.MarketUrl,
                    x.Title)))
            .ToList();
    }

    private static bool MatchFilter(BaseSkinViewModel model, string? filter)
    {
        if (filter is null)
            return true;
        string[] filterWords = filter.ToLower().Split(_separator, StringSplitOptions.RemoveEmptyEntries);

        return filterWords.All(word => model.Title.Contains(word, StringComparison.CurrentCultureIgnoreCase));
    }

    #endregion Methods
}
