﻿using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Archives;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;

namespace SteamStorage.ViewModels.Tools.UtilityViewModels;

public class ArchiveGroupViewModel : BaseExtendedGroupViewModel
{
    #region Fields

    private readonly ArchiveGroupModel _model;
    private readonly ArchiveGroupsModel _archiveGroupsModel;

    #endregion Fields

    #region Properties

    public ArchiveGroupModel ArchiveGroupModel
    {
        get => _model;
    }

    public string BuySumString
    {
        get => _model.BuySumString;
    }

    public string SoldSumString
    {
        get => _model.SoldSumString;
    }

    public double Change
    {
        get => _model.Change;
    }

    #endregion Properties

    #region Commands

    public RelayCommand<ArchiveGroupModel> OpenArchivesCommand
    {
        get => _archiveGroupsModel.OpenArchivesCommand;
    }

    public RelayCommand<ArchiveGroupModel> AddArchiveCommand
    {
        get => _archiveGroupsModel.AddArchiveCommand;
    }

    public RelayCommand AddArchiveGroupCommand
    {
        get => _archiveGroupsModel.AddArchiveGroupCommand;
    }

    public RelayCommand<ArchiveGroupModel> EditArchiveGroupCommand
    {
        get => _archiveGroupsModel.EditArchiveGroupCommand;
    }

    public AsyncRelayCommand<ArchiveGroupModel> DeleteArchiveGroupCommand
    {
        get => _archiveGroupsModel.DeleteArchiveGroupCommand;
    }

    #endregion Commands

    #region Constructor

    public ArchiveGroupViewModel(
        ArchiveGroupModel model,
        ArchiveGroupsModel archiveGroupsModel) : base(model)
    {
        _model = model;
        _archiveGroupsModel = archiveGroupsModel;

        archiveGroupsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
