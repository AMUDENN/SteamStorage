using System.Collections.Generic;
using SteamStorage.Models.Actives;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels.Actives;

public class ActivesViewModel : ViewModelBase
{
    #region Fields

    private readonly ActivesModel _activesModel;

    #endregion Fields

    #region Properties

    public ViewModelBase CurrentViewModel
    {
        get => _activesModel.CurrentViewModel;
    }

    public IEnumerable<SecondaryNavigationModel> SecondaryNavigationOptions
    {
        get => _activesModel.SecondaryNavigationOptions;
    }

    public SecondaryNavigationModel? SelectedSecondaryNavigationModel
    {
        get => _activesModel.SelectedSecondaryNavigationModel;
        set => _activesModel.SelectedSecondaryNavigationModel = value;
    }

    #endregion Properties

    #region Constructor

    public ActivesViewModel(
        ActivesModel activesModel)
    {
        _activesModel = activesModel;
        activesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
