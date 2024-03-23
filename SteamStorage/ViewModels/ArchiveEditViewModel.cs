using System;
using System.Collections.Generic;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.ViewModels.BaseViewModels;

namespace SteamStorage.ViewModels;

public class ArchiveEditViewModel : ExtendedBaseItemEditViewModel
{
    #region Fields

    private readonly ArchiveEditModel _archiveEditModel;
    private readonly ArchiveGroupsModel _archiveGroupsModel;

    #endregion Fields

    #region Properties

    public IEnumerable<BaseGroupModel> ArchiveGroupModels
    {
        get => _archiveGroupsModel.ArchiveGroupModels;
    }

    public string DefaultCount
    {
        get => _archiveEditModel.DefaultCount;
    }

    public string Count
    {
        get => _archiveEditModel.Count;
        set => _archiveEditModel.Count = value;
    }

    public string DefaultBuyPrice
    {
        get => _archiveEditModel.DefaultBuyPrice;
    }

    public string BuyPrice
    {
        get => _archiveEditModel.BuyPrice;
        set => _archiveEditModel.BuyPrice = value;
    }

    public string DefaultSoldPrice
    {
        get => _archiveEditModel.DefaultSoldPrice;
    }

    public string SoldPrice
    {
        get => _archiveEditModel.SoldPrice;
        set => _archiveEditModel.SoldPrice = value;
    }

    public string? DefaultDescription
    {
        get => _archiveEditModel.DefaultDescription;
    }

    public string? Description
    {
        get => _archiveEditModel.Description;
        set => _archiveEditModel.Description = value;
    }

    public DateTimeOffset DefaultBuyDate
    {
        get => _archiveEditModel.DefaultBuyDate;
    }

    public DateTimeOffset BuyDate
    {
        get => _archiveEditModel.BuyDate;
        set => _archiveEditModel.BuyDate = value;
    }

    public DateTimeOffset DefaultSoldDate
    {
        get => _archiveEditModel.DefaultSoldDate;
    }

    public DateTimeOffset SoldDate
    {
        get => _archiveEditModel.SoldDate;
        set => _archiveEditModel.SoldDate = value;
    }


    #endregion Properties

    #region Constructor

    public ArchiveEditViewModel(
        ArchiveEditModel archiveEditModel,
        ArchiveGroupsModel archiveGroupsModel) : base(archiveEditModel)
    {
        _archiveEditModel = archiveEditModel;
        _archiveGroupsModel = archiveGroupsModel;

        archiveEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        archiveGroupsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void SetEditArchive(ArchiveModel? model)
    {
        _archiveEditModel.SetEditArchive(model);
    }

    public void SetAddArchive(ArchiveGroupModel? model)
    {
        _archiveEditModel.SetAddArchive(model);
    }

    public void SetAddArchive(ListItemModel? model)
    {
        _archiveEditModel.SetAddArchive(model);
    }

    #endregion Methods
}
