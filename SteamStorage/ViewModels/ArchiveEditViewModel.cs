using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class ArchiveEditViewModel : ViewModelBase
{
    #region Fields

    private readonly ArchiveEditModel _archiveEditModel;

    #endregion Fields

    #region Constructor

    public ArchiveEditViewModel(
        ArchiveEditModel archiveEditModel)
    {
        _archiveEditModel = archiveEditModel;
        archiveEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
    
    #region Methods

    public void SetEditArchive(ArchiveModel? model)
    {
        _archiveEditModel.SetEditArchive(model);
    }
    
    public void SetAddArchive(ArchiveGroupModel? model)
    {
        _archiveEditModel.SetAddArchive(model);
    }
    
    public void SetAddArchive(ListItemModel? model)
    {
        _archiveEditModel.SetAddArchive(model);
    }
    
    #endregion Methods
}
