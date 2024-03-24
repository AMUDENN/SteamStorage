using System;
using System.Collections.Generic;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.ViewModels.BaseViewModels;

namespace SteamStorage.ViewModels;

public class ActiveSoldViewModel : BaseEditViewModel
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
    
    public BaseGroupModel? DefaultArchiveGroupModel
    {
        get => _activeSoldModel.DefaultArchiveGroupModel;
    }

    public BaseGroupModel? SelectedArchiveGroupModel
    {
        get => _activeSoldModel.SelectedArchiveGroupModel;
        set => _activeSoldModel.SelectedArchiveGroupModel = value;
    }

    public string DefaultCount
    {
        get => _activeSoldModel.DefaultCount;
    }

    public string Count
    {
        get => _activeSoldModel.Count;
        set => _activeSoldModel.Count = value;
    }

    public string DefaultSoldPrice
    {
        get => _activeSoldModel.DefaultSoldPrice;
    }

    public string SoldPrice
    {
        get => _activeSoldModel.SoldPrice;
        set => _activeSoldModel.SoldPrice = value;
    }

    public DateTimeOffset DefaultSoldDate
    {
        get => _activeSoldModel.DefaultSoldDate;
    }

    public DateTimeOffset SoldDate
    {
        get => _activeSoldModel.SoldDate;
        set => _activeSoldModel.SoldDate = value;
    }

    public string? DefaultDescription
    {
        get => _activeSoldModel.DefaultDescription;
    }

    public string? Description
    {
        get => _activeSoldModel.Description;
        set => _activeSoldModel.Description = value;
    }

    public string BuyPrice
    {
        get => _activeSoldModel.BuyPrice;
    }

    public int TotalCount
    {
        get => _activeSoldModel.TotalCount;
    }

    public string CurrentPrice
    {
        get => _activeSoldModel.CurrentPrice;
    }

    public string BuyDate
    {
        get => _activeSoldModel.BuyDate;
    }

    public double? GoalPriceCompletion
    {
        get => _activeSoldModel.GoalPriceCompletion;
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
