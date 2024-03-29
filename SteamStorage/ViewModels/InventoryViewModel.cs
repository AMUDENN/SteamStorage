﻿using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;
using SteamStorage.ViewModels.UtilityViewModels;

namespace SteamStorage.ViewModels;

public class InventoryViewModel : ViewModelBase
{
    #region Fields

    private readonly InventoryModel _inventoryModel;
    private readonly GamesModel _gamesModel;

    #endregion Fields

    #region Properties

    public IEnumerable<GameModel> GameModels
    {
        get => _gamesModel.GameModels;
    }

    public GameModel? SelectedGameModel
    {
        get => _inventoryModel.SelectedGameModel;
        set => _inventoryModel.SelectedGameModel = value;
    }

    public bool IsAllGamesChecked
    {
        get => _inventoryModel.IsAllGamesChecked;
        set => _inventoryModel.IsAllGamesChecked = value;
    }

    public string? Filter
    {
        get => _inventoryModel.Filter;
        set => _inventoryModel.Filter = value;
    }

    public bool? IsTitleOrdering
    {
        get => _inventoryModel.IsTitleOrdering;
        set => _inventoryModel.IsTitleOrdering = value;
    }

    public bool? IsCountOrdering
    {
        get => _inventoryModel.IsCountOrdering;
        set => _inventoryModel.IsCountOrdering = value;
    }

    public bool? IsPriceOrdering
    {
        get => _inventoryModel.IsPriceOrdering;
        set => _inventoryModel.IsPriceOrdering = value;
    }

    public bool? IsSumOrdering
    {
        get => _inventoryModel.IsSumOrdering;
        set => _inventoryModel.IsSumOrdering = value;
    }
    
    public bool IsRefreshing
    {
        get => _inventoryModel.IsRefreshing;
    }

    public IEnumerable<InventoryItemViewModel> InventoryModels
    {
        get => _inventoryModel.InventoryModels;
    }

    public InventoryItemViewModel? SelectedInventoryModel
    {
        get => _inventoryModel.SelectedInventoryModel;
        set => _inventoryModel.SelectedInventoryModel = value;
    }

    public string? NotFoundText
    {
        get => _inventoryModel.NotFoundText;
    }

    public bool IsLoading
    {
        get => _inventoryModel.IsLoading;
    }

    public int? PageNumber
    {
        get => _inventoryModel.PageNumber;
        set => _inventoryModel.PageNumber = value;
    }

    public int CurrentPageNumber
    {
        get => _inventoryModel.CurrentPageNumber;
    }

    public int PagesCount
    {
        get => _inventoryModel.PagesCount;
    }

    public int DisplayItemsCountStart
    {
        get => _inventoryModel.DisplayItemsCountStart;
    }

    public int DisplayItemsCountEnd
    {
        get => _inventoryModel.DisplayItemsCountEnd;
    }

    public int SavedItemsCount
    {
        get => _inventoryModel.SavedItemsCount;
    }

    #endregion Properties

    #region Commands

    public RelayCommand ClearFiltersCommand
    {
        get => _inventoryModel.ClearFiltersCommand;
    }

    public AsyncRelayCommand RefreshInventoryCommand
    {
        get => _inventoryModel.RefreshInventoryCommand;
    }

    public RelayCommand AttachedToVisualTreeCommand
    {
        get => _inventoryModel.AttachedToVisualTreeCommand;
    }

    #endregion Commands

    #region Constructor

    public InventoryViewModel(
        InventoryModel inventoryModel,
        GamesModel gamesModel)
    {
        _inventoryModel = inventoryModel;
        _gamesModel = gamesModel;

        inventoryModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        gamesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
