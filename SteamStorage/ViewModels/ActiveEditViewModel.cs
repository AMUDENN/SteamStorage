using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class ActiveEditViewModel : ViewModelBase
{
    #region Fields

    private readonly ActiveEditModel _activeEditModel;

    #endregion Fields

    #region Constructor

    public ActiveEditViewModel(
        ActiveEditModel activeEditModel)
    {
        _activeEditModel = activeEditModel;
        activeEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void SetEditActive(ActiveModel? model)
    {
        _activeEditModel.SetEditActive(model);
    }

    public void SetAddActive(ActiveGroupModel? model)
    {
        _activeEditModel.SetAddActive(model);
    }

    public void SetAddActive(ListItemModel? model)
    {
        _activeEditModel.SetAddActive(model);
    }

    #endregion Methods
}
