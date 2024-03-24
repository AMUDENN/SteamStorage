using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.BaseViewModels;

namespace SteamStorage.ViewModels;

public class ArchiveGroupEditViewModel : BaseEditViewModel
{
    #region Fields

    private readonly ArchiveGroupEditModel _archiveGroupEditModel;

    #endregion Fields

    #region Constructor

    public ArchiveGroupEditViewModel(
        ArchiveGroupEditModel archiveGroupEditModel) : base(archiveGroupEditModel)
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
