using System;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;

namespace SteamStorage.Models;

public class ActiveEditModel : ModelBase
{
    #region Fields

    private readonly ActiveGroupsModel _activeGroupsModel;

    private BaseGroupModel? _defaultGroupModel;
    private BaseGroupModel? _selectedGroupModel;

    private int _defaultCount;
    private int _count;

    private decimal _defaultBuyPrice;
    private decimal _buyPrice;

    private decimal? _defaultGoalPrice;
    private decimal? _goalPrice;

    //item

    private string? _defaultDescription;
    private string? _description;

    private DateTimeOffset _defaultBuyDate;
    private DateTimeOffset _buyDate;

    #endregion Fields

    #region Properties

    public BaseGroupModel? DefaultGroupModel
    {
        get => _defaultGroupModel;
        private set => SetProperty(ref _defaultGroupModel, value);
    }

    public BaseGroupModel? SelectedGroupModel
    {
        get => _selectedGroupModel;
        set => SetProperty(ref _selectedGroupModel, value);
    }

    public int DefaultCount
    {
        get => _defaultCount;
        private set => SetProperty(ref _defaultCount, value);
    }

    public int Count
    {
        get => _count;
        set => SetProperty(ref _count, value);
    }

    public decimal DefaultBuyPrice
    {
        get => _defaultBuyPrice;
        private set => SetProperty(ref _defaultBuyPrice, value);
    }

    public decimal BuyPrice
    {
        get => _buyPrice;
        set => SetProperty(ref _buyPrice, value);
    }

    public decimal? DefaultGoalPrice
    {
        get => _defaultGoalPrice;
        private set => SetProperty(ref _defaultGoalPrice, value);
    }

    public decimal? GoalPrice
    {
        get => _goalPrice;
        set => SetProperty(ref _goalPrice, value);
    }

    public string? DefaultDescription
    {
        get => _defaultDescription;
        private set => SetProperty(ref _defaultDescription, value);
    }

    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public DateTimeOffset DefaultBuyDate
    {
        get => _defaultBuyDate;
        private set => SetProperty(ref _defaultBuyDate, value);
    }

    public DateTimeOffset BuyDate
    {
        get => _buyDate;
        set => SetProperty(ref _buyDate, value);
    }

    #endregion Properties

    #region Constructor

    public ActiveEditModel(
        ActiveGroupsModel activeGroupsModel)
    {
        _activeGroupsModel = activeGroupsModel;

        _defaultDescription = string.Empty;
        _description = string.Empty;
    }

    #endregion Constructor

    #region Methods

    private void SetValuesFromDefault()
    {
        SelectedGroupModel = DefaultGroupModel;
        Count = DefaultCount;
        BuyPrice = DefaultBuyPrice;
        GoalPrice = DefaultGoalPrice;
        Description = DefaultDescription;
        BuyDate = DefaultBuyDate;
    }

    public void SetEditActive(ActiveModel? model)
    {
        DefaultGroupModel = _activeGroupsModel.ActiveGroupModels.FirstOrDefault(x => x.GroupId == model?.GroupId);

        DefaultCount = model?.Count ?? 1;

        DefaultBuyPrice = model?.BuyPrice ?? 1;

        DefaultGoalPrice = model?.GoalPrice;

        DefaultDescription = model?.Description;

        DefaultBuyDate = DateTime.SpecifyKind(model?.BuyDate ?? DateTime.Now, DateTimeKind.Local);

        SetValuesFromDefault();
    }

    public void SetAddActive(ActiveGroupModel? model)
    {
        DefaultGroupModel = _activeGroupsModel.ActiveGroupModels.FirstOrDefault(x => x.GroupId == model?.GroupId);

        DefaultCount = 1;

        DefaultBuyPrice = 1;

        DefaultGoalPrice = null;

        DefaultDescription = null;

        DefaultBuyDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        SetValuesFromDefault();
    }

    public void SetAddActive(ListItemModel? model)
    {
        DefaultGroupModel = null;

        DefaultCount = 1;

        DefaultBuyPrice = model?.CurrentPrice ?? 1;

        DefaultGoalPrice = null;

        DefaultDescription = null;

        DefaultBuyDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        SetValuesFromDefault();
    }

    #endregion Methods
}
