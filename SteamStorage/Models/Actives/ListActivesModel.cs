using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools.BaseModels;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;
using SteamStorage.Services.DialogService;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Dialog;
using SteamStorage.Utilities.Events.Actives;
using SteamStorage.ViewModels.Tools.UtilityViewModels;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models.Actives;

public class ListActivesModel : BaseListModel
{
    #region Events

    public delegate void EditActiveEventHandler(object? sender, EditActiveEventArgs args);

    public event EditActiveEventHandler? EditActive;

    public delegate void SoldActiveEventHandler(object? sender, SoldActiveEventArgs args);

    public event SoldActiveEventHandler? SoldActive;

    #endregion Events

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly ChartTooltipModel _chartTooltipModel;
    private readonly UserModel _userModel;
    private readonly PeriodsModel _periodsModel;
    private readonly IThemeService _themeService;
    private readonly IDialogService _dialogService;

    private int _count;
    private string _investedSumString;
    private string _currentSumString;
    
    private BaseGroupModel? _selectedGroupModel;

    private GameModel? _selectedGameModel;
    private bool _isAllGamesChecked;

    private string? _filter;

    private bool? _isTitleOrdering;
    private bool? _isCountOrdering;
    private bool? _isBuyPriceOrdering;
    private bool? _isCurrentPriceOrdering;
    private bool? _isCurrentSumOrdering;
    private bool? _isChangeOrdering;

    private SteamStorageAPI.SDK.ApiEntities.Actives.ActiveOrderName? _activeOrderName;
    private bool? _isAscending;

    private List<ActiveViewModel> _activeModels;
    private ActiveViewModel? _selectedActiveModel;

    private CancellationTokenSource _itemsCancellationTokenSource;
    private CancellationTokenSource _statisticsCancellationTokenSource;

    #endregion Fields

    #region Properties

    public int Count
    {
        get => _count;
        private set => SetProperty(ref _count, value);
    }

    public string InvestedSumString
    {
        get => _investedSumString;
        private set => SetProperty(ref _investedSumString, value);
    }

    public string CurrentSumString
    {
        get => _currentSumString;
        private set => SetProperty(ref _currentSumString, value);
    }
    
    public BaseGroupModel? SelectedGroupModel
    {
        get => _selectedGroupModel;
        set
        {
            SetProperty(ref _selectedGroupModel, value);
            GetStatisticsAsync();
            GetSkinsAsync();
        }
    }

    public GameModel? SelectedGameModel
    {
        get => _selectedGameModel;
        set
        {
            SetProperty(ref _selectedGameModel, value);
            if (value is null) return;
            IsAllGamesChecked = false;
            GetStatisticsAsync();
            GetSkinsAsync();
        }
    }

    public bool IsAllGamesChecked
    {
        get => _isAllGamesChecked;
        set
        {
            SetProperty(ref _isAllGamesChecked, value);
            if (!value) return;
            SelectedGameModel = null;
            GetStatisticsAsync();
            GetSkinsAsync();
        }
    }

    public string? Filter
    {
        get => _filter;
        set
        {
            SetProperty(ref _filter, value);
            GetStatisticsAsync();
            GetSkinsAsync();
        }
    }

    public bool? IsTitleOrdering
    {
        get => _isTitleOrdering;
        set
        {
            if (_isTitleOrdering is not null && value is null)
            {
                ActiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveOrderName = SteamStorageAPI.SDK.ApiEntities.Actives.ActiveOrderName.Title;
                IsAscending = value;
            }

            SetProperty(ref _isTitleOrdering, value);
        }
    }

    public bool? IsCountOrdering
    {
        get => _isCountOrdering;
        set
        {
            if (_isCountOrdering is not null && value is null)
            {
                ActiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveOrderName = SteamStorageAPI.SDK.ApiEntities.Actives.ActiveOrderName.Count;
                IsAscending = value;
            }

            SetProperty(ref _isCountOrdering, value);
        }
    }

    public bool? IsBuyPriceOrdering
    {
        get => _isBuyPriceOrdering;
        set
        {
            if (_isBuyPriceOrdering is not null && value is null)
            {
                ActiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveOrderName = SteamStorageAPI.SDK.ApiEntities.Actives.ActiveOrderName.BuyPrice;
                IsAscending = value;
            }

            SetProperty(ref _isBuyPriceOrdering, value);
        }
    }

    public bool? IsCurrentPriceOrdering
    {
        get => _isCurrentPriceOrdering;
        set
        {
            if (_isCurrentPriceOrdering is not null && value is null)
            {
                ActiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveOrderName = SteamStorageAPI.SDK.ApiEntities.Actives.ActiveOrderName.CurrentPrice;
                IsAscending = value;
            }

            SetProperty(ref _isCurrentPriceOrdering, value);
        }
    }

    public bool? IsCurrentSumOrdering
    {
        get => _isCurrentSumOrdering;
        set
        {
            if (_isCurrentSumOrdering is not null && value is null)
            {
                ActiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveOrderName = SteamStorageAPI.SDK.ApiEntities.Actives.ActiveOrderName.CurrentSum;
                IsAscending = value;
            }

            SetProperty(ref _isCurrentSumOrdering, value);
        }
    }

    public bool? IsChangeOrdering
    {
        get => _isChangeOrdering;
        set
        {
            if (_isChangeOrdering is not null && value is null)
            {
                ActiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveOrderName = SteamStorageAPI.SDK.ApiEntities.Actives.ActiveOrderName.Change;
                IsAscending = value;
            }

            SetProperty(ref _isChangeOrdering, value);
        }
    }

    public List<ActiveViewModel> ActiveModels
    {
        get => _activeModels;
        private set
        {
            SetProperty(ref _activeModels, value);
            OnPropertyChanged(nameof(NotFoundText));
        }
    }

    public ActiveViewModel? SelectedActiveModel
    {
        get => _selectedActiveModel;
        set
        {
            SetProperty(ref _selectedActiveModel, value);
            SelectedActiveModel?.UpdateStats();
        }
    }
    
    public override string? NotFoundText
    {
        get => ActiveModels.Count == 0 && !IsLoading ? EMPTY_LIST_TEXT : null;
    }

    private SteamStorageAPI.SDK.ApiEntities.Actives.ActiveOrderName? ActiveOrderName
    {
        get => _activeOrderName;
        set
        {
            SetProperty(ref _activeOrderName, value);
            GetSkinsAsync();
        }
    }

    private bool? IsAscending
    {
        get => _isAscending;
        set
        {
            SetProperty(ref _isAscending, value);
            GetSkinsAsync();
        }
    }

    private CancellationTokenSource ItemsCancellationTokenSource
    {
        get => _itemsCancellationTokenSource;
        set => SetProperty(ref _itemsCancellationTokenSource, value);
    }
    
    private CancellationTokenSource StatisticsCancellationTokenSource
    {
        get => _statisticsCancellationTokenSource;
        set => SetProperty(ref _statisticsCancellationTokenSource, value);
    }

    #endregion Properties

    #region Commands

    public RelayCommand ClearFiltersCommand { get; }

    public RelayCommand<ActiveModel> EditCommand { get; }

    public RelayCommand<ActiveModel> SoldCommand { get; }

    public AsyncRelayCommand<ActiveModel> DeleteCommand { get; }

    #endregion Commands

    #region Constructor

    public ListActivesModel(
        ApiClient apiClient,
        ChartTooltipModel chartTooltipModel,
        UserModel userModel,
        PeriodsModel periodsModel,
        IThemeService themeService,
        IDialogService dialogService)
    {
        _apiClient = apiClient;
        _chartTooltipModel = chartTooltipModel;
        _userModel = userModel;
        _periodsModel = periodsModel;
        _themeService = themeService;
        _dialogService = dialogService;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;

        _investedSumString = string.Empty;
        _currentSumString = string.Empty;
        
        _activeModels = [];
        _itemsCancellationTokenSource = new();
        _statisticsCancellationTokenSource = new();

        IsAllGamesChecked = true;

        SetOrderingsNull();

        IsLoading = false;

        PageNumber = 1;
        PageSize = 20;

        EditCommand = new(DoEditCommand);
        SoldCommand = new(DoSoldCommand);
        DeleteCommand = new(DoDeleteCommand);
        ClearFiltersCommand = new(DoClearFiltersCommand);
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        GetSkinsAsync();
        GetStatisticsAsync();
    }

    private void CurrencyChangedHandler(object? sender)
    {
        GetSkinsAsync();
        GetStatisticsAsync();
    }

    private void DoClearFiltersCommand()
    {
        Filter = null;
        SelectedGroupModel = null;
        IsAllGamesChecked = true;
        ActiveOrderName = null;
        IsAscending = null;
        PageNumber = 1;
        SetOrderingsNull();
    }

    private void DoEditCommand(ActiveModel? model)
    {
        OnEditActive(model);
    }

    private void DoSoldCommand(ActiveModel? model)
    {
        OnSoldActive(model);
    }

    private async Task DoDeleteCommand(ActiveModel? model, CancellationToken cancellationToken)
    {
        if (model is null) return;
        
        bool result = await _dialogService.ShowDialogAsync(
            $"Вы уверены, что хотите удалить актив: «{model.Title}»?",
            DialogUtility.MessageType.Question,
            DialogUtility.MessageButtons.OkCancel);
        
        if (!result) return;

        await _apiClient.DeleteAsync(
            ApiConstants.ApiMethods.DeleteActive,
            new SteamStorageAPI.SDK.ApiEntities.Actives.DeleteActiveRequest(model.ActiveId),
            cancellationToken);

        GetSkinsAsync();
        GetStatisticsAsync();
    }

    public void OpenActiveGroup(IEnumerable<BaseGroupModel> groupModels, ActiveGroupModel? model)
    {
        SelectedGroupModel = groupModels.SingleOrDefault(x => x.GroupId == model?.GroupId);
    }

    public void UpdateSkins()
    {
        GetSkinsAsync();
        GetStatisticsAsync();
    }

    private void SetOrderingsNull()
    {
        IsTitleOrdering = null;
        IsCountOrdering = null;
        IsBuyPriceOrdering = null;
        IsCurrentPriceOrdering = null;
        IsCurrentSumOrdering = null;
        IsChangeOrdering = null;
    }

    private async void GetStatisticsAsync()
    {
        await StatisticsCancellationTokenSource.CancelAsync();

        StatisticsCancellationTokenSource = new();
        CancellationToken token = StatisticsCancellationTokenSource.Token;

        SteamStorageAPI.SDK.ApiEntities.Actives.ActivesStatisticResponse? activesStatisticResponse =
            await _apiClient.GetAsync<SteamStorageAPI.SDK.ApiEntities.Actives.ActivesStatisticResponse, SteamStorageAPI.SDK.ApiEntities.Actives.GetActivesStatisticRequest>(
                ApiConstants.ApiMethods.GetActivesStatistic,
                new(SelectedGroupModel?.GroupId, SelectedGameModel?.Id, Filter),
                token);

        if (activesStatisticResponse is null) return;

        Count = activesStatisticResponse.ActivesCount;
        InvestedSumString = $"{activesStatisticResponse.InvestmentSum:N2} {_userModel.CurrencyMark}";
        CurrentSumString = $"{activesStatisticResponse.CurrentSum:N2} {_userModel.CurrencyMark}";
    }

    protected override async void GetSkinsAsync()
    {
        ActiveModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;

        await ItemsCancellationTokenSource.CancelAsync();

        ItemsCancellationTokenSource = new();
        CancellationToken token = ItemsCancellationTokenSource.Token;

        CurrentPageNumber = PageNumber ?? 1;

        DisplayItemsCountStart = (CurrentPageNumber - 1) * PageSize + 1;
        DisplayItemsCountEnd = CurrentPageNumber * PageSize;

        SteamStorageAPI.SDK.ApiEntities.Actives.ActivesResponse? activesResponse =
            await _apiClient.GetAsync<SteamStorageAPI.SDK.ApiEntities.Actives.ActivesResponse, SteamStorageAPI.SDK.ApiEntities.Actives.GetActivesRequest>(
                ApiConstants.ApiMethods.GetActives,
                new(SelectedGroupModel?.GroupId, SelectedGameModel?.Id, Filter, ActiveOrderName, IsAscending,
                    CurrentPageNumber, PageSize),
                token);

        if (activesResponse is null) return;

        SavedItemsCount = activesResponse.Count;
        PagesCount = activesResponse.PagesCount;

        if (activesResponse.Actives is null) return;

        ActiveModels = activesResponse.Actives.Select(x =>
                new ActiveViewModel(
                    new(_apiClient,
                        _periodsModel,
                        _themeService,
                        x.Skin.Id,
                        x.Skin.SkinIconUrl,
                        x.Skin.MarketUrl,
                        x.Skin.Title,
                        x.Id,
                        x.GroupId,
                        x.Count,
                        x.BuyPrice,
                        x.CurrentPrice,
                        x.CurrentSum,
                        x.GoalPrice,
                        x.GoalPriceCompletion,
                        _userModel.CurrencyMark,
                        x.Change,
                        x.BuyDate,
                        x.Description),
                    this,
                    _chartTooltipModel))
            .ToList();

        IsLoading = false;
    }

    private void OnEditActive(ActiveModel? model)
    {
        EditActive?.Invoke(this, new(model));
    }

    private void OnSoldActive(ActiveModel? model)
    {
        SoldActive?.Invoke(this, new(model));
    }

    #endregion Methods
}
