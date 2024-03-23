using System.Collections.Generic;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.ViewModels.BaseViewModels;

namespace SteamStorage.ViewModels;

public class ArchiveEditViewModel : BaseItemEditViewModel
{
    #region Fields

    private readonly ArchiveEditModel _archiveEditModel;
    private readonly ArchiveGroupsModel _archiveGroupsModel;

    #endregion Fields

    #region Properties

    public IEnumerable<BaseGroupModel> ArchiveGroupModels
    {
        get => _archiveGroupsModel.ArchiveGroupModels;
    }

    #endregion Properties

    #region Constructor

    public ArchiveEditViewModel(
        ArchiveEditModel archiveEditModel,
        ArchiveGroupsModel archiveGroupsModel) : base(archiveEditModel)
    {
        _archiveEditModel = archiveEditModel;
        _archiveGroupsModel = archiveGroupsModel;

        archiveEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        archiveGroupsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
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
