using System.Collections.Generic;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.ViewModels.BaseViewModels;

namespace SteamStorage.ViewModels;

public class ActiveSoldViewModel : BaseItemEditViewModel
{
    #region Fields

    private readonly ActiveSoldModel _activeSoldModel;
    private readonly ArchiveGroupsModel _archiveGroupsModel;

    #endregion Fields

    #region Properties

    public IEnumerable<BaseGroupModel> ArchiveGroupModels
    {
        get => _archiveGroupsModel.ArchiveGroupModels;
    }

    #endregion Properties

    #region Constructor

    public ActiveSoldViewModel(
        ActiveSoldModel activeSoldModel,
        ArchiveGroupsModel archiveGroupsModel) : base(activeSoldModel)
    {
        _activeSoldModel = activeSoldModel;
        _archiveGroupsModel = archiveGroupsModel;

        activeSoldModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        archiveGroupsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void SetSoldActive(ActiveModel? model)
    {
        _activeSoldModel.SetSoldActive(model);
    }

    #endregion Methods
}
