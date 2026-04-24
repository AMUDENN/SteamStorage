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

    public string DateCreationString => _archiveGroupEditModel.DateCreationString;

    public string BuySumString => _archiveGroupEditModel.BuySumString;

    public string SoldSumString => _archiveGroupEditModel.SoldSumString;

    public string CountString => _archiveGroupEditModel.CountString;

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