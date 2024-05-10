using SteamStorage.Models.Archives;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools.BaseViewModels;

namespace SteamStorage.ViewModels.Archives;

public class ArchiveGroupEditViewModel : BaseGroupEditViewModel
{
    #region Fields

    private readonly ArchiveGroupEditModel _archiveGroupEditModel;

    #endregion Fields

    #region Properties

    public string DateCreationString
    {
        get => _archiveGroupEditModel.DateCreationString;
    }

    public string BuySumString
    {
        get => _archiveGroupEditModel.BuySumString;
    }

    public string SoldSumString
    {
        get => _archiveGroupEditModel.SoldSumString;
    }

    public string CountString
    {
        get => _archiveGroupEditModel.CountString;
    }

    #endregion Properties

    #region Constructor

    public ArchiveGroupEditViewModel(
        ArchiveGroupEditModel archiveGroupEditModel) : base(archiveGroupEditModel)
    {
        _archiveGroupEditModel = archiveGroupEditModel;
    }

    #endregion Constructor

    #region Methods

    public void SetEditGroup(ArchiveGroupModel? model)
    {
        _archiveGroupEditModel.SetEditGroup(model);
    }

    #endregion Methods
}
