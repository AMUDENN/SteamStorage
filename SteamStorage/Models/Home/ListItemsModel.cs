﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools.BaseModels;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Events.ListItems;
using SteamStorage.ViewModels.Tools.UtilityViewModels;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models.Home;

public class ListItemsModel : BaseListModel
{
    #region Events

    public delegate void AddToActivesEventHandler(object? sender, AddToActivesEventArgs args);

    public event AddToActivesEventHandler? AddToActives;

    public delegate void AddToArchiveEventHandler(object? sender, AddToArchiveEventArgs args);

    public event AddToArchiveEventHandler? AddToArchive;

    #endregion Events

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly ChartTooltipModel _chartTooltipModel;
    private readonly UserModel _userModel;
    private readonly PeriodsModel _periodsModel;
    private readonly IThemeService _themeService;

    private GameModel? _selectedGameModel;
    private bool _isAllGamesChecked;

    private string? _filter;

    private bool? _isTitleOrdering;
    private bool? _isCurrentCostOrdering;
    private bool? _isChange7Ordering;
    private bool? _isChange30Ordering;

    private Skins.SkinOrderName? _skinOrderName;
    private bool? _isAscending;

    private bool _isMarked;

    private List<ListItemViewModel> _listItemModels;
    private ListItemViewModel? _selectedListItemModel;
    
    private CancellationTokenSource _cancellationTokenSource;

    #endregion Fields

    #region Properties

    public GameModel? SelectedGameModel
    {
        get => _selectedGameModel;
        set
        {
            SetProperty(ref _selectedGameModel, value);
            if (value is null) return;
            IsAllGamesChecked = false;
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
            GetSkinsAsync();
        }
    }

    public string? Filter
    {
        get => _filter;
        set
        {
            SetProperty(ref _filter, value);
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
                SkinOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                SkinOrderName = Skins.SkinOrderName.Title;
                IsAscending = value;
            }

            SetProperty(ref _isTitleOrdering, value);
        }
    }

    public bool? IsCurrentCostOrdering
    {
        get => _isCurrentCostOrdering;
        set
        {
            if (_isCurrentCostOrdering is not null && value is null)
            {
                SkinOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                SkinOrderName = Skins.SkinOrderName.Price;
                IsAscending = value;
            }

            SetProperty(ref _isCurrentCostOrdering, value);
        }
    }

    public bool? IsChange7Ordering
    {
        get => _isChange7Ordering;
        set
        {
            if (_isChange7Ordering is not null && value is null)
            {
                SkinOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                SkinOrderName = Skins.SkinOrderName.Change7D;
                IsAscending = value;
            }

            SetProperty(ref _isChange7Ordering, value);
        }
    }

    public bool? IsChange30Ordering
    {
        get => _isChange30Ordering;
        set
        {
            if (_isChange30Ordering is not null && value is null)
            {
                SkinOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                SkinOrderName = Skins.SkinOrderName.Change30D;
                IsAscending = value;
            }

            SetProperty(ref _isChange30Ordering, value);
        }
    }

    public bool IsMarked
    {
        get => _isMarked;
        set
        {
            SetProperty(ref _isMarked, value);
            GetSkinsAsync();
        }
    }

    public List<ListItemViewModel> ListItemModels
    {
        get => _listItemModels;
        private set
        {
            SetProperty(ref _listItemModels, value);
            OnPropertyChanged(nameof(NotFoundText));
        }
    }

    public ListItemViewModel? SelectedListItemModel
    {
        get => _selectedListItemModel;
        set
        {
            SetProperty(ref _selectedListItemModel, value);
            SelectedListItemModel?.UpdateStats();
        }
    }

    public override string? NotFoundText
    {
        get => ListItemModels.Count == 0 && !IsLoading ? EMPTY_LIST_TEXT : null;
    }
    
    private Skins.SkinOrderName? SkinOrderName
    {
        get => _skinOrderName;
        set
        {
            SetProperty(ref _skinOrderName, value);
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

    private CancellationTokenSource CancellationTokenSource
    {
        get => _cancellationTokenSource;
        set => SetProperty(ref _cancellationTokenSource, value);
    }

    #endregion Properties

    #region Commands

    public RelayCommand ClearFiltersCommand { get; }

    public RelayCommand<ListItemModel> AddToActivesCommand { get; }

    public RelayCommand<ListItemModel> AddToArchiveCommand { get; }

    #endregion Commands

    #region Constructor

    public ListItemsModel(
        ApiClient apiClient,
        ChartTooltipModel chartTooltipModel,
        UserModel userModel,
        PeriodsModel periodsModel,
        IThemeService themeService)
    {
        _apiClient = apiClient;
        _chartTooltipModel = chartTooltipModel;
        _userModel = userModel;
        _periodsModel = periodsModel;
        _themeService = themeService;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;

        _listItemModels = [];
        _cancellationTokenSource = new();

        IsAllGamesChecked = true;

        SetOrderingsNull();

        IsMarked = false;

        IsLoading = false;

        PageNumber = 1;
        PageSize = 20;

        ClearFiltersCommand = new(DoClearFiltersCommand);
        AddToActivesCommand = new(DoAddToActivesCommand);
        AddToArchiveCommand = new(DoAddToArchiveCommand);
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        GetSkinsAsync();
    }

    private void CurrencyChangedHandler(object? sender)
    {
        GetSkinsAsync();
    }

    private void DoClearFiltersCommand()
    {
        Filter = null;
        IsAllGamesChecked = true;
        SkinOrderName = null;
        IsAscending = null;
        IsMarked = false;
        PageNumber = 1;
        SetOrderingsNull();
    }

    private void DoAddToActivesCommand(ListItemModel? model)
    {
        OnAddToActives(model);
    }

    private void DoAddToArchiveCommand(ListItemModel? model)
    {
        OnAddToArchive(model);
    }

    private void SetOrderingsNull()
    {
        IsTitleOrdering = null;
        IsCurrentCostOrdering = null;
        IsChange7Ordering = null;
        IsChange30Ordering = null;
    }

    protected override async void GetSkinsAsync()
    {
        ListItemModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;

        await CancellationTokenSource.CancelAsync();

        CancellationTokenSource = new();
        CancellationToken token = CancellationTokenSource.Token;

        CurrentPageNumber = PageNumber ?? 1;

        DisplayItemsCountStart = (CurrentPageNumber - 1) * PageSize + 1;
        DisplayItemsCountEnd = CurrentPageNumber * PageSize;

        Skins.SkinsResponse? skinsResponse =
            await _apiClient.GetAsync<Skins.SkinsResponse, Skins.GetSkinsRequest>(
                ApiConstants.ApiMethods.GetSkins,
                new(SelectedGameModel?.Id, Filter, SkinOrderName, IsAscending, IsMarked ? IsMarked : null,
                    CurrentPageNumber, PageSize),
                token);

        if (skinsResponse is null) return;

        SavedItemsCount = skinsResponse.Count;
        PagesCount = skinsResponse.PagesCount;
        
        if (skinsResponse.Skins is null) return;

        ListItemModels = skinsResponse.Skins.Select(x =>
                new ListItemViewModel(
                    new(_apiClient,
                        _periodsModel,
                        _themeService,
                        x.Skin.Id,
                        x.Skin.SkinIconUrl,
                        x.Skin.MarketUrl,
                        x.Skin.Title,
                        x.CurrentPrice,
                        _userModel.CurrencyMark,
                        x.Change7D,
                        x.Change30D,
                        x.IsMarked),
                    this,
                    _chartTooltipModel))
            .ToList();

        IsLoading = false;
    }

    private void OnAddToActives(ListItemModel? model)
    {
        AddToActives?.Invoke(this, new(model));
    }

    private void OnAddToArchive(ListItemModel? model)
    {
        AddToArchive?.Invoke(this, new(model));
    }

    #endregion Methods
}
