using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.ViewModels.BaseViewModels;
using SteamStorage.ViewModels.UtilityViewModels;

namespace SteamStorage.ViewModels;

public class ListActivesViewModel : BaseListViewModel
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
    
    public int Count
    {
        get => _listActivesModel.Count;
    }

    public string InvestedSumString
    {
        get => _listActivesModel.InvestedSumString;
    }

    public string CurrentSumString
    {
        get => _listActivesModel.CurrentSumString;
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
        GamesModel gamesModel) : base(listActivesModel)
    {
        _listActivesModel = listActivesModel;
        _activeGroupsModel = activeGroupsModel;
        _gamesModel = gamesModel;

        activeGroupsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        gamesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void OpenActiveGroup(ActiveGroupModel? model)
    {
        _listActivesModel.OpenActiveGroup(ActiveGroupModels, model);
    }

    #endregion Methods
}
