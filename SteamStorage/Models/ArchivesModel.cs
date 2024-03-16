using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.Models;

public class ArchivesModel : ModelBase
{
    #region Fields

    private ViewModelBase _currentViewModel;

    private SecondaryNavigationModel? _selectedSecondaryNavigationModel;

    #endregion Fields

    #region Properties

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public IEnumerable<SecondaryNavigationModel> SecondaryNavigationOptions { get; }

    public SecondaryNavigationModel? SelectedSecondaryNavigationModel
    {
        get => _selectedSecondaryNavigationModel;
        set
        {
            SetProperty(ref _selectedSecondaryNavigationModel, value);
            if (value is null) return;
            CurrentViewModel = value.Page;
        }
    }

    #endregion Properties

    #region Constructor

    public ArchivesModel(
        ArchivesReviewViewModel archivesReviewViewModel,
        ListArchivesViewModel listArchivesViewModel)
    {
        SecondaryNavigationOptions =
        [
            new("Обзор", archivesReviewViewModel, true),
            new("Список позиций", listArchivesViewModel, true),
            new("Управление группами", archivesReviewViewModel, false),
            new("Управление позициями", archivesReviewViewModel, false)
        ];

        _selectedSecondaryNavigationModel = SecondaryNavigationOptions.First();
        _currentViewModel = _selectedSecondaryNavigationModel.Page;
    }

    #endregion Constructor
}
