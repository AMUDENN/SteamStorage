using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.BaseViewModels;

namespace SteamStorage.ViewModels;

public class ActiveGroupEditViewModel : BaseEditViewModel
{
    #region Fields

    private readonly ActiveGroupEditModel _activeGroupEditModel;

    #endregion Fields

    #region Constructor

    public ActiveGroupEditViewModel(
        ActiveGroupEditModel activeGroupEditModel) : base(activeGroupEditModel)
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
