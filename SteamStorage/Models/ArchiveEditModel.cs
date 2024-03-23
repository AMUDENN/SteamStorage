using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.UtilityViewModels.BaseViewModels;
using SteamStorageAPI;

namespace SteamStorage.Models;

public class ArchiveEditModel : BaseItemEditModel
{
    #region Constants

    private const string TITLE = "Изменение элемента архива";

    #endregion Constants

    #region Fields

    private readonly ArchiveGroupsModel _archiveGroupsModel;

    #endregion Fields

    #region Properties



    #endregion Properties

    #region Constructor

    public ArchiveEditModel(
        ApiClient apiClient,
        ArchiveGroupsModel archiveGroupsModel) : base(apiClient)
    {
        _archiveGroupsModel = archiveGroupsModel;
    }

    #endregion Constructor

    #region Methods

    protected override void DoBackCommand()
    {

    }

    protected override void DoDeleteCommand()
    {

    }

    protected override void DoSaveCommand()
    {

    }

    protected override bool CanExecuteSaveCommand()
    {
        return true;
    }

    protected override void SetTitle(BaseSkinViewModel? model)
    {
        if (model is null) Title = TITLE;
        Title = $"{TITLE}: «{model?.Title}»";
    }

    private void SetValuesFromDefault()
    {

    }

    public void SetEditArchive(ArchiveModel? model)
    {

    }

    public void SetAddArchive(ArchiveGroupModel? model)
    {

    }

    public void SetAddArchive(ListItemModel? model)
    {

    }

    #endregion Methods
}
