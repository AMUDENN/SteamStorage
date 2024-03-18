using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class ActiveGroupEditViewModel : ViewModelBase
{
    #region Fields

    private readonly ActiveGroupEditModel _activeGroupEditModel;

    #endregion Fields

    #region Constructor

    public ActiveGroupEditViewModel(
        ActiveGroupEditModel activeGroupEditModel)
    {
        _activeGroupEditModel = activeGroupEditModel;
        activeGroupEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void SetEditGroup(ActiveGroupModel? model)
    {
        _activeGroupEditModel.SetEditGroup(model);
    }

    #endregion Methods
}
