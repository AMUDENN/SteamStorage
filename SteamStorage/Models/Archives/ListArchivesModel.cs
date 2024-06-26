﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools.BaseModels;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;
using SteamStorage.Services.DialogService;
using SteamStorage.Services.NotificationService;
using SteamStorage.Utilities.Dialog;
using SteamStorage.Utilities.Events.Archives;
using SteamStorage.ViewModels.Tools.UtilityViewModels;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models.Archives;

public class ListArchivesModel : BaseListModel
{
    #region Events

    public delegate void EditArchiveEventHandler(object? sender, EditArchiveEventArgs args);

    public event EditArchiveEventHandler? EditArchive;

    #endregion Events

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly UserModel _userModel;
    private readonly IDialogService _dialogService;
    private readonly INotificationService _notificationService;

    private int _count;
    private string _investedSumString;
    private string _soldSumString;
    
    private BaseGroupModel? _selectedGroupModel;

    private GameModel? _selectedGameModel;
    private bool _isAllGamesChecked;

    private string? _filter;

    private bool? _isTitleOrdering;
    private bool? _isCountOrdering;
    private bool? _isBuyPriceOrdering;
    private bool? _isSoldPriceOrdering;
    private bool? _isSoldSumOrdering;
    private bool? _isChangeOrdering;

    private SteamStorageAPI.SDK.ApiEntities.Archives.ArchiveOrderName? _archiveOrderName;
    private bool? _isAscending;

    private List<ArchiveViewModel> _archiveModels;
    private ArchiveViewModel? _selectedArchiveModel;

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

    public string SoldSumString
    {
        get => _soldSumString;
        private set => SetProperty(ref _soldSumString, value);
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
                ArchiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveOrderName = SteamStorageAPI.SDK.ApiEntities.Archives.ArchiveOrderName.Title;
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
                ArchiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveOrderName = SteamStorageAPI.SDK.ApiEntities.Archives.ArchiveOrderName.Count;
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
                ArchiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveOrderName = SteamStorageAPI.SDK.ApiEntities.Archives.ArchiveOrderName.BuyPrice;
                IsAscending = value;
            }

            SetProperty(ref _isBuyPriceOrdering, value);
        }
    }

    public bool? IsSoldPriceOrdering
    {
        get => _isSoldPriceOrdering;
        set
        {
            if (_isSoldPriceOrdering is not null && value is null)
            {
                ArchiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveOrderName = SteamStorageAPI.SDK.ApiEntities.Archives.ArchiveOrderName.SoldPrice;
                IsAscending = value;
            }

            SetProperty(ref _isSoldPriceOrdering, value);
        }
    }

    public bool? IsSoldSumOrdering
    {
        get => _isSoldSumOrdering;
        set
        {
            if (_isSoldSumOrdering is not null && value is null)
            {
                ArchiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveOrderName = SteamStorageAPI.SDK.ApiEntities.Archives.ArchiveOrderName.SoldSum;
                IsAscending = value;
            }

            SetProperty(ref _isSoldSumOrdering, value);
        }
    }

    public bool? IsChangeOrdering
    {
        get => _isChangeOrdering;
        set
        {
            if (_isChangeOrdering is not null && value is null)
            {
                ArchiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveOrderName = SteamStorageAPI.SDK.ApiEntities.Archives.ArchiveOrderName.Change;
                IsAscending = value;
            }

            SetProperty(ref _isChangeOrdering, value);
        }
    }

    public List<ArchiveViewModel> ArchiveModels
    {
        get => _archiveModels;
        private set
        {
            SetProperty(ref _archiveModels, value);
            OnPropertyChanged(nameof(NotFoundText));
        }
    }

    public ArchiveViewModel? SelectedArchiveModel
    {
        get => _selectedArchiveModel;
        set => SetProperty(ref _selectedArchiveModel, value);
    }

    public override string? NotFoundText
    {
        get => ArchiveModels.Count == 0 && !IsLoading ? EMPTY_LIST_TEXT : null;
    }

    private SteamStorageAPI.SDK.ApiEntities.Archives.ArchiveOrderName? ArchiveOrderName
    {
        get => _archiveOrderName;
        set
        {
            SetProperty(ref _archiveOrderName, value);
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

    public RelayCommand<ArchiveModel> EditCommand { get; }

    public AsyncRelayCommand<ArchiveModel> DeleteCommand { get; }

    #endregion Commands

    #region Constructor

    public ListArchivesModel(
        ApiClient apiClient,
        UserModel userModel,
        IDialogService dialogService,
        INotificationService notificationService)
    {
        _apiClient = apiClient;
        _userModel = userModel;
        _dialogService = dialogService;
        _notificationService = notificationService;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;

        _investedSumString = string.Empty;
        _soldSumString = string.Empty;
        
        _archiveModels = [];
        _itemsCancellationTokenSource = new();
        _statisticsCancellationTokenSource = new();

        IsAllGamesChecked = true;

        SetOrderingsNull();

        IsLoading = false;

        PageNumber = 1;
        PageSize = 20;

        ClearFiltersCommand = new(DoClearFiltersCommand);
        EditCommand = new(DoEditCommand);
        DeleteCommand = new(DoDeleteCommand);
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
        ArchiveOrderName = null;
        IsAscending = null;
        PageNumber = 1;
        SetOrderingsNull();
    }

    private void DoEditCommand(ArchiveModel? model)
    {
        OnEditArchive(model);
    }

    private async Task DoDeleteCommand(ArchiveModel? model, CancellationToken cancellationToken)
    {
        if (model is null) return;
        
        bool result = await _dialogService.ShowDialogAsync(
            $"Вы уверены, что хотите удалить элемент архива: «{model.Title}»?",
            DialogUtility.MessageType.Question,
            DialogUtility.MessageButtons.OkCancel);
        
        if (!result) return;

        await _apiClient.DeleteAsync(
            ApiConstants.ApiMethods.DeleteArchive,
            new SteamStorageAPI.SDK.ApiEntities.Archives.DeleteArchiveRequest(model.ArchiveId),
            cancellationToken);
        
        await _notificationService.ShowAsync("Удаление элемента архива",
            $"Вы отправили запрос на удаление элемента архива: {model.Title}", 
            cancellationToken: cancellationToken);

        GetSkinsAsync();
        GetStatisticsAsync();
    }

    public void OpenArchiveGroup(IEnumerable<BaseGroupModel> groupModels, ArchiveGroupModel? model)
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
        IsSoldPriceOrdering = null;
        IsSoldSumOrdering = null;
        IsChangeOrdering = null;
    }
    
    private async void GetStatisticsAsync()
    {
        await StatisticsCancellationTokenSource.CancelAsync();

        StatisticsCancellationTokenSource = new();
        CancellationToken token = StatisticsCancellationTokenSource.Token;

        SteamStorageAPI.SDK.ApiEntities.Archives.ArchivesStatisticResponse? archivesStatisticResponse =
            await _apiClient.GetAsync<SteamStorageAPI.SDK.ApiEntities.Archives.ArchivesStatisticResponse, SteamStorageAPI.SDK.ApiEntities.Archives.GetArchivesStatisticRequest>(
                ApiConstants.ApiMethods.GetArchivesStatistic,
                new(SelectedGroupModel?.GroupId, SelectedGameModel?.Id, Filter),
                token);

        if (archivesStatisticResponse is null) return;
        
        Count = archivesStatisticResponse.ArchivesCount;
        InvestedSumString = $"{archivesStatisticResponse.InvestmentSum:N2} {_userModel.CurrencyMark}";
        SoldSumString = $"{archivesStatisticResponse.SoldSum:N2} {_userModel.CurrencyMark}";
    }

    protected override async void GetSkinsAsync()
    {
        ArchiveModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;

        await ItemsCancellationTokenSource.CancelAsync();

        ItemsCancellationTokenSource = new();
        CancellationToken token = ItemsCancellationTokenSource.Token;

        CurrentPageNumber = PageNumber ?? 1;

        DisplayItemsCountStart = (CurrentPageNumber - 1) * PageSize + 1;
        DisplayItemsCountEnd = CurrentPageNumber * PageSize;

        SteamStorageAPI.SDK.ApiEntities.Archives.ArchivesResponse? archivesResponse =
            await _apiClient.GetAsync<SteamStorageAPI.SDK.ApiEntities.Archives.ArchivesResponse, SteamStorageAPI.SDK.ApiEntities.Archives.GetArchivesRequest>(
                ApiConstants.ApiMethods.GetArchives,
                new(SelectedGroupModel?.GroupId, SelectedGameModel?.Id, Filter, ArchiveOrderName, IsAscending,
                    CurrentPageNumber, PageSize),
                token);

        if (archivesResponse is null) return;

        SavedItemsCount = archivesResponse.Count;
        PagesCount = archivesResponse.PagesCount;
        
        if (archivesResponse.Archives is null) return;

        ArchiveModels = archivesResponse.Archives.Select(x =>
                new ArchiveViewModel(
                    new(x.Skin.Id,
                        x.Skin.SkinIconUrl,
                        x.Skin.MarketUrl,
                        x.Skin.Title,
                        x.Id,
                        x.GroupId,
                        x.Count,
                        x.BuyPrice,
                        x.SoldPrice,
                        x.SoldSum,
                        _userModel.CurrencyMark,
                        x.Change,
                        x.BuyDate,
                        x.SoldDate,
                        x.Description),
                    this))
            .ToList();

        IsLoading = false;
    }

    private void OnEditArchive(ArchiveModel? model)
    {
        EditArchive?.Invoke(this, new(model));
    }

    #endregion Methods
}
