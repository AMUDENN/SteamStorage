using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.BaseViewModels;
using SteamStorage.ViewModels.UtilityViewModels;

namespace SteamStorage.ViewModels;

public class ListItemsViewModel : BaseListViewModel
{
    #region Fields

    private readonly ListItemsModel _listItemsModel;
    private readonly GamesModel _gamesModel;

    #endregion Fields

    #region Properties

    public IEnumerable<GameModel> GameModels
    {
        get => _gamesModel.GameModels;
    }

    public GameModel? SelectedGameModel
    {
        get => _listItemsModel.SelectedGameModel;
        set => _listItemsModel.SelectedGameModel = value;
    }

    public bool IsAllGamesChecked
    {
        get => _listItemsModel.IsAllGamesChecked;
        set => _listItemsModel.IsAllGamesChecked = value;
    }

    public string? Filter
    {
        get => _listItemsModel.Filter;
        set => _listItemsModel.Filter = value;
    }

    public bool? IsTitleOrdering
    {
        get => _listItemsModel.IsTitleOrdering;
        set => _listItemsModel.IsTitleOrdering = value;
    }

    public bool? IsCurrentCostOrdering
    {
        get => _listItemsModel.IsCurrentCostOrdering;
        set => _listItemsModel.IsCurrentCostOrdering = value;
    }

    public bool? IsChange7Ordering
    {
        get => _listItemsModel.IsChange7Ordering;
        set => _listItemsModel.IsChange7Ordering = value;
    }

    public bool? IsChange30Ordering
    {
        get => _listItemsModel.IsChange30Ordering;
        set => _listItemsModel.IsChange30Ordering = value;
    }

    public bool IsMarked
    {
        get => _listItemsModel.IsMarked;
        set => _listItemsModel.IsMarked = value;
    }

    public IEnumerable<ListItemViewModel> ListItemModels
    {
        get => _listItemsModel.ListItemModels;
    }

    public ListItemViewModel? SelectedListItemModel
    {
        get => _listItemsModel.SelectedListItemModel;
        set => _listItemsModel.SelectedListItemModel = value;
    }

    #endregion Properties

    #region Commands

    public RelayCommand ClearFiltersCommand
    {
        get => _listItemsModel.ClearFiltersCommand;
    }

    #endregion Commands

    #region Constructor

    public ListItemsViewModel(
        ListItemsModel listItemsModel,
        GamesModel gamesModel) : base(listItemsModel)
    {
        _listItemsModel = listItemsModel;
        _gamesModel = gamesModel;

        listItemsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        gamesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
