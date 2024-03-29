﻿using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.ViewModels.Tools;
using SteamStorage.ViewModels.UtilityViewModels;

namespace SteamStorage.ViewModels;

public class ListArchivesViewModel : ViewModelBase
{
    #region Fields

    private readonly ListArchivesModel _listArchivesModel;
    private readonly ArchiveGroupsModel _archiveGroupsModel;
    private readonly GamesModel _gamesModel;

    #endregion Fields

    #region Properties

    public IEnumerable<BaseGroupModel> ArchiveGroupModels
    {
        get => _archiveGroupsModel.ArchiveGroupModels;
    }

    public BaseGroupModel? SelectedGroupModel
    {
        get => _listArchivesModel.SelectedGroupModel;
        set => _listArchivesModel.SelectedGroupModel = value;
    }

    public IEnumerable<GameModel> GameModels
    {
        get => _gamesModel.GameModels;
    }

    public GameModel? SelectedGameModel
    {
        get => _listArchivesModel.SelectedGameModel;
        set => _listArchivesModel.SelectedGameModel = value;
    }

    public bool IsAllGamesChecked
    {
        get => _listArchivesModel.IsAllGamesChecked;
        set => _listArchivesModel.IsAllGamesChecked = value;
    }

    public string? Filter
    {
        get => _listArchivesModel.Filter;
        set => _listArchivesModel.Filter = value;
    }

    public bool? IsTitleOrdering
    {
        get => _listArchivesModel.IsTitleOrdering;
        set => _listArchivesModel.IsTitleOrdering = value;
    }

    public bool? IsCountOrdering
    {
        get => _listArchivesModel.IsCountOrdering;
        set => _listArchivesModel.IsCountOrdering = value;
    }

    public bool? IsBuyPriceOrdering
    {
        get => _listArchivesModel.IsBuyPriceOrdering;
        set => _listArchivesModel.IsBuyPriceOrdering = value;
    }

    public bool? IsSoldPriceOrdering
    {
        get => _listArchivesModel.IsSoldPriceOrdering;
        set => _listArchivesModel.IsSoldPriceOrdering = value;
    }

    public bool? IsSoldSumOrdering
    {
        get => _listArchivesModel.IsSoldSumOrdering;
        set => _listArchivesModel.IsSoldSumOrdering = value;
    }

    public bool? IsChangeOrdering
    {
        get => _listArchivesModel.IsChangeOrdering;
        set => _listArchivesModel.IsChangeOrdering = value;
    }

    public IEnumerable<ArchiveViewModel> ArchiveModels
    {
        get => _listArchivesModel.ArchiveModels;
    }

    public ArchiveViewModel? SelectedArchiveModel
    {
        get => _listArchivesModel.SelectedArchiveModel;
        set => _listArchivesModel.SelectedArchiveModel = value;
    }

    public string? NotFoundText
    {
        get => _listArchivesModel.NotFoundText;
    }

    public bool IsLoading
    {
        get => _listArchivesModel.IsLoading;
    }

    public int? PageNumber
    {
        get => _listArchivesModel.PageNumber;
        set => _listArchivesModel.PageNumber = value;
    }

    public int CurrentPageNumber
    {
        get => _listArchivesModel.CurrentPageNumber;
    }

    public int PagesCount
    {
        get => _listArchivesModel.PagesCount;
    }

    public int DisplayItemsCountStart
    {
        get => _listArchivesModel.DisplayItemsCountStart;
    }

    public int DisplayItemsCountEnd
    {
        get => _listArchivesModel.DisplayItemsCountEnd;
    }

    public int SavedItemsCount
    {
        get => _listArchivesModel.SavedItemsCount;
    }

    #endregion Properties

    #region Commands

    public RelayCommand AddArchiveGroupCommand
    {
        get => _archiveGroupsModel.AddArchiveGroupCommand;
    }

    public RelayCommand<ArchiveGroupModel> AddArchiveCommand
    {
        get => _archiveGroupsModel.AddArchiveCommand;
    }

    public RelayCommand ClearFiltersCommand
    {
        get => _listArchivesModel.ClearFiltersCommand;
    }

    #endregion Commands

    #region Constructor

    public ListArchivesViewModel(
        ListArchivesModel listArchivesModel,
        ArchiveGroupsModel archiveGroupsModel,
        GamesModel gamesModel)
    {
        _listArchivesModel = listArchivesModel;
        _archiveGroupsModel = archiveGroupsModel;
        _gamesModel = gamesModel;

        listArchivesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        archiveGroupsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        gamesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void OpenArchiveGroup(ArchiveGroupModel? model)
    {
        _listArchivesModel.OpenArchiveGroup(ArchiveGroupModels, model);
    }

    #endregion Methods
}
