using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Utilities;
using SteamStorage.ViewModels.UtilityViewModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ActiveEditModel : ModelBase
{
    #region Constants

    private const string TITLE = "Изменение актива";

    #endregion Constants

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly ActiveGroupsModel _activeGroupsModel;

    private string _title;

    private BaseGroupModel? _defaultGroupModel;
    private BaseGroupModel? _selectedGroupModel;

    private string _defaultCount;
    private string _count;

    private string _defaultBuyPrice;
    private string _buyPrice;

    private string? _defaultGoalPrice;
    private string? _goalPrice;

    private BaseSkinViewModel? _defaultSkinModel;
    private BaseSkinViewModel? _selectedSkinModel;
    private string? _filter;
    private AutoCompleteFilterPredicate<object?>? _itemFilter;
    private List<BaseSkinViewModel> _skinModels;

    private string? _defaultDescription;
    private string? _description;

    private DateTimeOffset _defaultBuyDate;
    private DateTimeOffset _buyDate;

    private CancellationTokenSource _cancellationTokenSource;

    #endregion Fields

    #region Properties

    public string Title
    {
        get => _title;
        private set => SetProperty(ref _title, value);
    }

    public BaseGroupModel? DefaultGroupModel
    {
        get => _defaultGroupModel;
        private set => SetProperty(ref _defaultGroupModel, value);
    }

    public BaseGroupModel? SelectedGroupModel
    {
        get => _selectedGroupModel;
        set
        {
            SetProperty(ref _selectedGroupModel, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string DefaultCount
    {
        get => _defaultCount;
        private set => SetProperty(ref _defaultCount, value);
    }

    public string Count
    {
        get => _count;
        set
        {
            SetProperty(ref _count, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string DefaultBuyPrice
    {
        get => _defaultBuyPrice;
        private set => SetProperty(ref _defaultBuyPrice, value);
    }

    public string BuyPrice
    {
        get => _buyPrice;
        set
        {
            SetProperty(ref _buyPrice, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string? DefaultGoalPrice
    {
        get => _defaultGoalPrice;
        private set => SetProperty(ref _defaultGoalPrice, value);
    }

    public string? GoalPrice
    {
        get => _goalPrice;
        set
        {
            SetProperty(ref _goalPrice, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public BaseSkinViewModel? DefaultSkinModel
    {
        get => _defaultSkinModel;
        private set => SetProperty(ref _defaultSkinModel, value);
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
            GetSkins();
        }
    }

    public AutoCompleteFilterPredicate<object?>? ItemFilter
    {
        get => _itemFilter;
        private set => SetProperty(ref _itemFilter, value);
    }

    public List<BaseSkinViewModel> SkinModels
    {
        get => _skinModels;
        private set => SetProperty(ref _skinModels, value);
    }

    public string? DefaultDescription
    {
        get => _defaultDescription;
        private set => SetProperty(ref _defaultDescription, value);
    }

    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public DateTimeOffset DefaultBuyDate
    {
        get => _defaultBuyDate;
        private set => SetProperty(ref _defaultBuyDate, value);
    }

    public DateTimeOffset BuyDate
    {
        get => _buyDate;
        set => SetProperty(ref _buyDate, value);
    }

    private CancellationTokenSource CancellationTokenSource
    {
        get => _cancellationTokenSource;
        set => SetProperty(ref _cancellationTokenSource, value);
    }

    #endregion Properties

    #region Commands

    public RelayCommand DeleteCommand { get; }

    public RelayCommand SaveCommand { get; }

    #endregion Commands

    #region Constructor

    public ActiveEditModel(
        ApiClient apiClient,
        ActiveGroupsModel activeGroupsModel)
    {
        _apiClient = apiClient;
        _activeGroupsModel = activeGroupsModel;

        _title = string.Empty;

        _defaultCount = string.Empty;
        _count = string.Empty;

        _defaultBuyPrice = string.Empty;
        _buyPrice = string.Empty;

        _skinModels = [];
        _cancellationTokenSource = new();

        ItemFilter = ItemFilterPredicate;

        DeleteCommand = new(DoDeleteCommand);
        SaveCommand = new(DoSaveCommand, CanExecuteSaveCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoDeleteCommand()
    {

    }

    private void DoSaveCommand()
    {

    }

    private bool CanExecuteSaveCommand()
    {
        return SelectedGroupModel is not null
               && int.TryParse(Count.Replace(ProgramConstants.NUMBER_GROUP_SEPARATOR, string.Empty), out int _)
               && decimal.TryParse(BuyPrice, out decimal _)
               && (string.IsNullOrEmpty(GoalPrice) || decimal.TryParse(GoalPrice, out decimal _))
               && SelectedSkinModel is not null;
    }

    private bool ItemFilterPredicate(string? search, object? item)
    {
        return item is not null &&
               (string.IsNullOrEmpty(search) ||
                ((BaseSkinViewModel)item).Title.Contains(search, StringComparison.CurrentCultureIgnoreCase));
    }

    private void SetTitle(BaseSkinViewModel? model)
    {
        if (model is null) Title = TITLE;
        Title = $"{TITLE}: «{model?.Title}»";
    }

    private void SetValuesFromDefault()
    {
        SelectedGroupModel = DefaultGroupModel;
        Count = DefaultCount;
        BuyPrice = DefaultBuyPrice;
        GoalPrice = DefaultGoalPrice;
        SelectedSkinModel = DefaultSkinModel;
        Description = DefaultDescription;
        BuyDate = DefaultBuyDate;
    }

    public void SetEditActive(ActiveModel? model)
    {
        DefaultGroupModel = _activeGroupsModel.ActiveGroupModels.FirstOrDefault(x => x.GroupId == model?.GroupId);

        DefaultCount = $"{model?.Count ?? 1:N0}";

        DefaultBuyPrice = $"{model?.BuyPrice ?? 1:N2}";

        DefaultGoalPrice = $"{model?.GoalPrice:N2}";

        DefaultSkinModel = model is not null ? new(model) : null;

        DefaultDescription = model?.Description;

        DefaultBuyDate = DateTime.SpecifyKind(model?.BuyDate ?? DateTime.Now, DateTimeKind.Local);

        SetValuesFromDefault();
    }

    public void SetAddActive(ActiveGroupModel? model)
    {
        DefaultGroupModel = _activeGroupsModel.ActiveGroupModels.FirstOrDefault(x => x.GroupId == model?.GroupId);

        DefaultCount = "1";

        DefaultBuyPrice = "1";

        DefaultGoalPrice = null;

        DefaultSkinModel = null;

        DefaultDescription = null;

        DefaultBuyDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        SetValuesFromDefault();
    }

    public void SetAddActive(ListItemModel? model)
    {
        DefaultGroupModel = null;

        DefaultCount = "1";

        DefaultBuyPrice = $"{model?.CurrentPrice ?? 1:N2}";

        DefaultGoalPrice = null;

        DefaultSkinModel = model is not null ? new(model) : null;

        DefaultDescription = null;

        DefaultBuyDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        SetValuesFromDefault();
    }

    private async void GetSkins()
    {
        SkinModels = [];

        await CancellationTokenSource.CancelAsync();

        CancellationTokenSource = new();
        CancellationToken token = CancellationTokenSource.Token;

        IEnumerable<Skins.BaseSkinResponse>? skinsResponse =
            await _apiClient.GetAsync<IEnumerable<Skins.BaseSkinResponse>, Skins.GetBaseSkinsRequest>(
                ApiConstants.ApiControllers.Skins,
                ApiConstants.ApiMethods.GetBaseSkins,
                new(Filter),
                token);

        if (skinsResponse is null) return;

        SkinModels = skinsResponse.Select(x =>
                new BaseSkinViewModel(new(x.Id,
                    x.SkinIconUrl,
                    x.MarketUrl,
                    x.Title)))
            .ToList();
    }

    #endregion Methods
}
