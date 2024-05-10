using System.Collections.Generic;
using SteamStorage.Models.Archives;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels.Archives;

public class ArchivesViewModel : ViewModelBase
{
    #region Fields

    private readonly ArchivesModel _archivesModel;

    #endregion Fields

    #region Properties

    public ViewModelBase CurrentViewModel
    {
        get => _archivesModel.CurrentViewModel;
    }

    public IEnumerable<SecondaryNavigationModel> SecondaryNavigationOptions
    {
        get => _archivesModel.SecondaryNavigationOptions;
    }

    public SecondaryNavigationModel? SelectedSecondaryNavigationModel
    {
        get => _archivesModel.SelectedSecondaryNavigationModel;
        set => _archivesModel.SelectedSecondaryNavigationModel = value;
    }

    #endregion Properties

    #region Constructor

    public ArchivesViewModel(
        ArchivesModel archivesModel)
    {
        _archivesModel = archivesModel;
        archivesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
