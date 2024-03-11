using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;
using SteamStorage.ViewModels.UtilityViewModels;

namespace SteamStorage.ViewModels;

public class ListActivesViewModel : ViewModelBase
{
    #region Fields

    private readonly ListActivesModel _listActivesModel;
    private readonly ActiveGroupsModel _activeGroupsModel;
    private readonly GamesModel _gamesModel;

    #endregion Fields

    #region Properties
    
    public IEnumerable<BaseGroupModel> ActiveGroupModels
    {
        get => _activeGroupsModel.ActiveGroupModels;
    }
    
    public BaseGroupModel? SelectedGroupModel
    {
        get => _listActivesModel.SelectedGroupModel;
        set => _listActivesModel.SelectedGroupModel = value;
    }

    public IEnumerable<GameModel> GameModels
    {
        get => _gamesModel.GameModels;
    }

    public GameModel? SelectedGameModel
    {
        get => _listActivesModel.SelectedGameModel;
        set => _listActivesModel.SelectedGameModel = value;
    }

    public bool IsAllGamesChecked
    {
        get => _listActivesModel.IsAllGamesChecked;
        set => _listActivesModel.IsAllGamesChecked = value;
    }

    public string? Filter
    {
        get => _listActivesModel.Filter;
        set => _listActivesModel.Filter = value;
    }

    public bool? IsTitleOrdering
    {
        get => _listActivesModel.IsTitleOrdering;
        set => _listActivesModel.IsTitleOrdering = value;
    }
    
    public bool? IsCountOrdering
    {
        get => _listActivesModel.IsCountOrdering;
        set => _listActivesModel.IsCountOrdering = value;
    }
    
    public bool? IsBuyPriceOrdering
    {
        get => _listActivesModel.IsBuyPriceOrdering;
        set => _listActivesModel.IsBuyPriceOrdering = value;
    }
    
    public bool? IsCurrentPriceOrdering
    {
        get => _listActivesModel.IsCurrentPriceOrdering;
        set => _listActivesModel.IsCurrentPriceOrdering = value;
    }
    
    public bool? IsCurrentSumOrdering
    {
        get => _listActivesModel.IsCurrentSumOrdering;
        set => _listActivesModel.IsCurrentSumOrdering = value;
    }
    
    public bool? IsChangeOrdering
    {
        get => _listActivesModel.IsChangeOrdering;
        set => _listActivesModel.IsChangeOrdering = value;
    }

    public IEnumerable<ActiveViewModel> ActiveModels
    {
        get => _listActivesModel.ActiveModels;
    }

    public ActiveViewModel? SelectedActiveModel
    {
        get => _listActivesModel.SelectedActiveModel;
        set => _listActivesModel.SelectedActiveModel = value;
    }

    public string? NotFoundText
    {
        get => _listActivesModel.NotFoundText;
    }

    public bool IsLoading
    {
        get => _listActivesModel.IsLoading;
    }

    public int? PageNumber
    {
        get => _listActivesModel.PageNumber;
        set => _listActivesModel.PageNumber = value;
    }

    public int CurrentPageNumber
    {
        get => _listActivesModel.CurrentPageNumber;
    }

    public int PagesCount
    {
        get => _listActivesModel.PagesCount;
    }

    public int DisplayItemsCountStart
    {
        get => _listActivesModel.DisplayItemsCountStart;
    }

    public int DisplayItemsCountEnd
    {
        get => _listActivesModel.DisplayItemsCountEnd;
    }

    public int SavedItemsCount
    {
        get => _listActivesModel.SavedItemsCount;
    }

    #endregion Properties

    #region Commands

    public RelayCommand AddActiveGroupCommand
    {
        get => _activeGroupsModel.AddActiveGroupCommand;
    }

    public RelayCommand<ActiveGroupModel> AddActiveCommand
    {
        get => _activeGroupsModel.AddActiveCommand;
    }

    public RelayCommand ClearFiltersCommand
    {
        get => _listActivesModel.ClearFiltersCommand;
    }

    #endregion Commands

    #region Constructor

    public ListActivesViewModel(
        ListActivesModel listActivesModel,
        ActiveGroupsModel activeGroupsModel,
        GamesModel gamesModel)
    {
        _listActivesModel = listActivesModel;
        _activeGroupsModel = activeGroupsModel;
        _gamesModel = gamesModel;

        listActivesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        activeGroupsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName); 
        gamesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
