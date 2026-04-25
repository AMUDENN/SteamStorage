using System;
using System.Collections.Generic;
using SteamStorage.Models.Actives;
using SteamStorage.Models.Archives;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;
using SteamStorage.ViewModels.Tools.BaseViewModels;

namespace SteamStorage.ViewModels.Actives;

public class ActiveSoldViewModel : BaseEditViewModel
{
    #region Fields

    private readonly ActiveSoldModel _activeSoldModel;
    private readonly ArchiveGroupsModel _archiveGroupsModel;

    #endregion Fields

    #region Properties

    public IEnumerable<BaseGroupModel> ArchiveGroupModels => _archiveGroupsModel.ArchiveGroupModels;

    public BaseGroupModel? DefaultArchiveGroupModel => _activeSoldModel.DefaultArchiveGroupModel;

    public BaseGroupModel? SelectedArchiveGroupModel
    {
        get => _activeSoldModel.SelectedArchiveGroupModel;
        set => _activeSoldModel.SelectedArchiveGroupModel = value;
    }

    public string DefaultSoldCount => _activeSoldModel.DefaultSoldCount;

    public string SoldCount
    {
        get => _activeSoldModel.SoldCount;
        set => _activeSoldModel.SoldCount = value;
    }

    public string DefaultSoldPrice => _activeSoldModel.DefaultSoldPrice;

    public string SoldPrice
    {
        get => _activeSoldModel.SoldPrice;
        set => _activeSoldModel.SoldPrice = value;
    }

    public DateTimeOffset DefaultSoldDate => _activeSoldModel.DefaultSoldDate;

    public DateTimeOffset SoldDate
    {
        get => _activeSoldModel.SoldDate;
        set => _activeSoldModel.SoldDate = value;
    }

    public string? DefaultDescription => _activeSoldModel.DefaultDescription;

    public string? Description
    {
        get => _activeSoldModel.Description;
        set => _activeSoldModel.Description = value;
    }

    public string BuyPriceString => _activeSoldModel.BuyPriceString;

    public string CountString => _activeSoldModel.CountString;

    public string CurrentPriceString => _activeSoldModel.CurrentPriceString;

    public string BuyDateString => _activeSoldModel.BuyDateString;

    public string GoalPriceCompletion => _activeSoldModel.GoalPriceCompletion;

    #endregion Properties

    #region Constructor

    public ActiveSoldViewModel(
        ActiveSoldModel activeSoldModel,
        ArchiveGroupsModel archiveGroupsModel) : base(activeSoldModel)
    {
        _activeSoldModel = activeSoldModel;
        _archiveGroupsModel = archiveGroupsModel;

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