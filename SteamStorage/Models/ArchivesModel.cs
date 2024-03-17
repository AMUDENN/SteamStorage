using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Utilities.Events.Archives;
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
        ListArchivesViewModel listArchivesViewModel,
        ArchiveGroupEditViewModel archiveGroupEditViewModel,
        ArchiveGroupsModel archiveGroupsModel)
    {
        SecondaryNavigationOptions =
        [
            new("Обзор", archivesReviewViewModel, true),
            new("Список позиций", listArchivesViewModel, true),
            new("Управление группами", archiveGroupEditViewModel, false),
            new("Управление позициями", archivesReviewViewModel, false)
        ];

        _selectedSecondaryNavigationModel = SecondaryNavigationOptions.First();
        _currentViewModel = _selectedSecondaryNavigationModel.Page;
        
        archiveGroupsModel.AddArchive += AddArchiveHandler;
        archiveGroupsModel.OpenArchives += OpenArchivesHandler;
        archiveGroupsModel.EditArchiveGroup += EditArchiveGroupHandler;
    }

    #endregion Constructor
    
    #region Methods

    private void AddArchiveHandler(object? sender, AddArchiveEventArgs args)
    {
        
    }
    
    private void OpenArchivesHandler(object? sender, OpenArchivesEventArgs args)
    {
        
    }
    
    private void EditArchiveGroupHandler(object? sender, EditArchiveGroupEventArgs args)
    {
        SecondaryNavigationModel? navigationModel =
            SecondaryNavigationOptions.FirstOrDefault(x => x.Page.GetType() == typeof(ArchiveGroupEditViewModel));

        SelectedSecondaryNavigationModel = navigationModel;
    }

    #endregion Methods
}
