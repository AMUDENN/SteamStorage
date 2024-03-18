using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class ArchiveGroupEditViewModel : ViewModelBase
{
    #region Fields

    private readonly ArchiveGroupEditModel _archiveGroupEditModel;

    #endregion Fields

    #region Constructor

    public ArchiveGroupEditViewModel(
        ArchiveGroupEditModel archiveGroupEditModel)
    {
        _archiveGroupEditModel = archiveGroupEditModel;
        archiveGroupEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void SetEditGroup(ArchiveGroupModel? model)
    {
        _archiveGroupEditModel.SetEditGroup(model);
    }

    #endregion Methods
}
