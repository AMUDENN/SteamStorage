using System.Collections.Generic;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class ListItemsViewModel : ViewModelBase
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

    #endregion Properties
    
    #region Constructor

    public ListItemsViewModel(ListItemsModel listItemsModel, GamesModel gamesModel)
    {
        _listItemsModel = listItemsModel;
        _gamesModel = gamesModel;

        listItemsModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        gamesModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
