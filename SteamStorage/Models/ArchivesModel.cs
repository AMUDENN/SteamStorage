using System;
using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Utilities.Events.Archives;
using SteamStorage.Utilities.Events.ListItems;
using SteamStorage.ViewModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.Models;

public class ArchivesModel : ModelBase
{
    #region Fields

    private ViewModelBase _currentViewModel;

    private SecondaryNavigationModel? _selectedSecondaryNavigationModel;

    private SecondaryNavigationModel? _lastSecondaryNavigationModel;

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
        ArchiveEditViewModel archiveEditViewModel,
        ArchiveGroupsModel archiveGroupsModel,
        ListArchivesModel listArchivesModel,
        ListItemsModel listItemsModel,
        ArchiveEditModel archiveEditModel,
        ArchiveGroupEditModel archiveGroupEditModel)
    {
        SecondaryNavigationOptions =
        [
            new("Обзор", archivesReviewViewModel, true),
            new("Список позиций", listArchivesViewModel, true),
            new("Управление группами", archiveGroupEditViewModel, false),
            new("Управление позициями", archiveEditViewModel, false)
        ];

        _selectedSecondaryNavigationModel = SecondaryNavigationOptions.First();
        _currentViewModel = _selectedSecondaryNavigationModel.Page;

        archiveGroupsModel.AddArchive += AddArchiveHandler;
        archiveGroupsModel.OpenArchives += OpenArchivesHandler;
        archiveGroupsModel.EditArchiveGroup += EditArchiveGroupHandler;

        listArchivesModel.EditArchive += EditArchiveHandler;

        listItemsModel.AddToArchive += AddToArchiveHandler;

        archiveEditModel.GoingBack += GoingBackHandler;
        archiveGroupEditModel.GoingBack += GoingBackHandler;
    }

    #endregion Constructor

    #region Methods

    private void AddArchiveHandler(object? sender, AddArchiveEventArgs args)
    {
        SecondaryNavigationModel? navigationModel = FindViewModel(typeof(ArchiveEditViewModel));

        _lastSecondaryNavigationModel = SelectedSecondaryNavigationModel;

        SelectedSecondaryNavigationModel = navigationModel;

        (navigationModel?.Page as ArchiveEditViewModel)?.SetAddArchive(args.Group);
    }

    private void OpenArchivesHandler(object? sender, OpenArchivesEventArgs args)
    {
        SecondaryNavigationModel? navigationModel = FindViewModel(typeof(ListArchivesViewModel));

        _lastSecondaryNavigationModel = SelectedSecondaryNavigationModel;

        SelectedSecondaryNavigationModel = navigationModel;

        (navigationModel?.Page as ListArchivesViewModel)?.OpenArchiveGroup(args.Group);
    }

    private void EditArchiveGroupHandler(object? sender, EditArchiveGroupEventArgs args)
    {
        SecondaryNavigationModel? navigationModel = FindViewModel(typeof(ArchiveGroupEditViewModel));

        _lastSecondaryNavigationModel = SelectedSecondaryNavigationModel;

        SelectedSecondaryNavigationModel = navigationModel;

        (navigationModel?.Page as ArchiveGroupEditViewModel)?.SetEditGroup(args.Group);
    }

    private void EditArchiveHandler(object? sender, EditArchiveEventArgs args)
    {
        SecondaryNavigationModel? navigationModel = FindViewModel(typeof(ArchiveEditViewModel));

        _lastSecondaryNavigationModel = SelectedSecondaryNavigationModel;

        SelectedSecondaryNavigationModel = navigationModel;

        (navigationModel?.Page as ArchiveEditViewModel)?.SetEditArchive(args.ArchiveModel);
    }

    private void AddToArchiveHandler(object? sender, AddToArchiveEventArgs args)
    {
        SecondaryNavigationModel? navigationModel = FindViewModel(typeof(ArchiveEditViewModel));

        _lastSecondaryNavigationModel = SelectedSecondaryNavigationModel;

        SelectedSecondaryNavigationModel = navigationModel;

        (navigationModel?.Page as ArchiveEditViewModel)?.SetAddArchive(args.ListItem);
    }

    private void GoingBackHandler(object? sender)
    {
        if (SelectedSecondaryNavigationModel != _lastSecondaryNavigationModel)
            SelectedSecondaryNavigationModel = _lastSecondaryNavigationModel;
    }

    private SecondaryNavigationModel? FindViewModel(Type type)
    {
        return SecondaryNavigationOptions.FirstOrDefault(x => x.Page.GetType() == type);
    }

    #endregion Methods
}
