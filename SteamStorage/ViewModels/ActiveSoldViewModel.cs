using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class ActiveSoldViewModel : ViewModelBase
{
    #region Fields

    private readonly ActiveSoldModel _activeSoldModel;

    #endregion Fields

    #region Constructor

    public ActiveSoldViewModel(
        ActiveSoldModel activeSoldModel)
    {
        _activeSoldModel = activeSoldModel;
        activeSoldModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
    
    #region Methods

    public void SetSoldActive(ActiveModel? model)
    {
        _activeSoldModel.SetSoldActive(model);
    }
    
    #endregion Methods
}
