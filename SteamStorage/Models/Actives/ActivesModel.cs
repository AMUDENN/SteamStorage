﻿using System;
using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Home;
using SteamStorage.Models.Tools;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Utilities.Events.Actives;
using SteamStorage.Utilities.Events.ListItems;
using SteamStorage.ViewModels.Actives;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.Models.Actives;

public class ActivesModel : ModelBase
{
    #region Fields

    private ViewModelBase _currentViewModel;

    private SecondaryNavigationModel? _selectedSecondaryNavigationModel;

    private SecondaryNavigationModel? _lastSecondaryNavigationModel;

    private readonly ActivesReviewViewModel _activesReviewViewModel;
    private readonly ActiveGroupsModel _activeGroupsModel;
    private readonly ListActivesModel _listActivesModel;

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

    public ActivesModel(
        ActivesReviewViewModel activesReviewViewModel,
        ListActivesViewModel listActivesViewModel,
        ActiveGroupEditViewModel activeGroupEditViewModel,
        ActiveEditViewModel activeEditViewModel,
        ActiveSoldViewModel activeSoldViewModel,
        ActiveGroupsModel activeGroupsModel,
        ListActivesModel listActivesModel,
        ListItemsModel listItemsModel,
        ActiveEditModel activeEditModel,
        ActiveSoldModel activeSoldModel,
        ActiveGroupEditModel activeGroupEditModel)
    {
        SecondaryNavigationOptions =
        [
            new("Обзор", activesReviewViewModel, true),
            new("Список активов", listActivesViewModel, true),
            new("Управление группами", activeGroupEditViewModel, false),
            new("Управление активами", activeEditViewModel, false),
            new("Продажа актива", activeSoldViewModel, false)
        ];

        _selectedSecondaryNavigationModel = SecondaryNavigationOptions.First();
        _currentViewModel = _selectedSecondaryNavigationModel.Page;

        _activesReviewViewModel = activesReviewViewModel;
        _activeGroupsModel = activeGroupsModel;
        _listActivesModel = listActivesModel;

        activeGroupsModel.AddActive += AddActiveHandler;
        activeGroupsModel.OpenActives += OpenActivesHandler;
        activeGroupsModel.EditActiveGroup += EditActiveGroupHandler;
        activeGroupsModel.DeleteActiveGroup += DeleteActiveGroupHandler;

        listActivesModel.EditActive += EditActiveHandler;
        listActivesModel.SoldActive += SoldActiveHandler;

        listItemsModel.AddToActives += AddToActivesHandler;

        activeEditModel.GoingBack += GoingBackHandler;
        activeSoldModel.GoingBack += GoingBackHandler;
        activeGroupEditModel.GoingBack += GoingBackHandler;

        activeEditModel.ItemDeleted += ActiveItemDeletedHandler;
        activeSoldModel.ItemDeleted += ActiveItemDeletedHandler;
        activeGroupEditModel.ItemDeleted += ActiveGroupItemDeletedHandler;

        activeEditModel.ItemChanged += ActiveItemChangedHandler;
        activeSoldModel.ItemChanged += ActiveItemChangedHandler;
        activeGroupEditModel.ItemChanged += ActiveGroupItemChangedHandler;
    }

    #endregion Constructor

    #region Methods

    private void AddActiveHandler(object? sender, AddActiveEventArgs args)
    {
        SecondaryNavigationModel? navigationModel = FindViewModel(typeof(ActiveEditViewModel));

        _lastSecondaryNavigationModel = SelectedSecondaryNavigationModel;

        SelectedSecondaryNavigationModel = navigationModel;

        (navigationModel?.Page as ActiveEditViewModel)?.SetAddActive(args.Group);
    }

    private void OpenActivesHandler(object? sender, OpenActivesEventArgs args)
    {
        SecondaryNavigationModel? navigationModel = FindViewModel(typeof(ListActivesViewModel));

        SelectedSecondaryNavigationModel = navigationModel;

        (navigationModel?.Page as ListActivesViewModel)?.OpenActiveGroup(args.Group);
    }

    private void EditActiveGroupHandler(object? sender, EditActiveGroupEventArgs args)
    {
        SecondaryNavigationModel? navigationModel = FindViewModel(typeof(ActiveGroupEditViewModel));

        _lastSecondaryNavigationModel = SelectedSecondaryNavigationModel;

        SelectedSecondaryNavigationModel = navigationModel;

        (navigationModel?.Page as ActiveGroupEditViewModel)?.SetEditGroup(args.Group);
    }

    private void DeleteActiveGroupHandler(object? sender)
    {
        _activesReviewViewModel.UpdateGroups();
        _listActivesModel.UpdateSkins();
    }

    private void EditActiveHandler(object? sender, EditActiveEventArgs args)
    {
        SecondaryNavigationModel? navigationModel = FindViewModel(typeof(ActiveEditViewModel));

        _lastSecondaryNavigationModel = SelectedSecondaryNavigationModel;

        SelectedSecondaryNavigationModel = navigationModel;

        (navigationModel?.Page as ActiveEditViewModel)?.SetEditActive(args.ActiveModel);
    }

    private void SoldActiveHandler(object? sender, SoldActiveEventArgs args)
    {
        SecondaryNavigationModel? navigationModel = FindViewModel(typeof(ActiveSoldViewModel));

        _lastSecondaryNavigationModel = SelectedSecondaryNavigationModel;

        SelectedSecondaryNavigationModel = navigationModel;

        (navigationModel?.Page as ActiveSoldViewModel)?.SetSoldActive(args.ActiveModel);
    }

    private void AddToActivesHandler(object? sender, AddToActivesEventArgs args)
    {
        SecondaryNavigationModel? navigationModel = FindViewModel(typeof(ActiveEditViewModel));

        _lastSecondaryNavigationModel = SelectedSecondaryNavigationModel;

        SelectedSecondaryNavigationModel = navigationModel;

        (navigationModel?.Page as ActiveEditViewModel)?.SetAddActive(args.ListItem);
    }

    private void GoingBackHandler(object? sender)
    {
        if (SelectedSecondaryNavigationModel != _lastSecondaryNavigationModel)
            SelectedSecondaryNavigationModel = _lastSecondaryNavigationModel;
    }

    private void ActiveItemDeletedHandler(object? sender)
    {
        _listActivesModel.UpdateSkins();
    }
    
    private void ActiveGroupItemDeletedHandler(object? sender)
    {
        _activeGroupsModel.UpdateGroups();
    }
    
    private void ActiveItemChangedHandler(object? sender)
    {
        _listActivesModel.UpdateSkins();
    }
    
    private void ActiveGroupItemChangedHandler(object? sender)
    {
        _activeGroupsModel.UpdateGroups();
    }

    private SecondaryNavigationModel? FindViewModel(Type type)
    {
        return SecondaryNavigationOptions.FirstOrDefault(x => x.Page.GetType() == type);
    }

    #endregion Methods
}
