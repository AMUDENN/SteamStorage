using System;
using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Utilities;

namespace SteamStorage.Models;

public class ActiveSoldModel : BaseItemEditModel
{
    #region Constants

    private const string TITLE = "Продажа актива";

    #endregion Constants

    #region Fields

    private string _defaultCount;
    private string _count;

    private string _defaultSoldPrice;
    private string _soldPrice;

    private DateTimeOffset _defaultSoldDate;
    private DateTimeOffset _soldDate;

    private string? _defaultDescription;
    private string? _description;

    private string _buyPrice;
    private int _totalCount;
    private string _currentPrice;
    private string _buyDate;
    private double? _goalPriceCompletion;

    #endregion Fields

    #region Properties

    public string DefaultCount
    {
        get => _defaultCount;
        private set => SetProperty(ref _defaultCount, value);
    }

    public string Count
    {
        get => _count;
        set
        {
            SetProperty(ref _count, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string DefaultSoldPrice
    {
        get => _defaultSoldPrice;
        private set => SetProperty(ref _defaultSoldPrice, value);
    }

    public string SoldPrice
    {
        get => _soldPrice;
        set
        {
            SetProperty(ref _soldPrice, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public DateTimeOffset DefaultSoldDate
    {
        get => _defaultSoldDate;
        private set => SetProperty(ref _defaultSoldDate, value);
    }

    public DateTimeOffset SoldDate
    {
        get => _soldDate;
        set => SetProperty(ref _soldDate, value);
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

    public string BuyPrice
    {
        get => _buyPrice;
        private set => SetProperty(ref _buyPrice, value);
    }

    public int TotalCount
    {
        get => _totalCount;
        private set => SetProperty(ref _totalCount, value);
    }

    public string CurrentPrice
    {
        get => _currentPrice;
        private set => SetProperty(ref _currentPrice, value);
    }

    public string BuyDate
    {
        get => _buyDate;
        private set => SetProperty(ref _buyDate, value);
    }

    public double? GoalPriceCompletion
    {
        get => _goalPriceCompletion;
        private set => SetProperty(ref _goalPriceCompletion, value);
    }

    #endregion Properties

    #region Constructor

    public ActiveSoldModel()
    {
        _defaultCount = string.Empty;
        _count = string.Empty;

        _defaultSoldPrice = string.Empty;
        _soldPrice = string.Empty;

        _buyPrice = string.Empty;
        _currentPrice = string.Empty;
        _buyDate = string.Empty;
    }

    #endregion Constructor

    #region Methods

    protected override void DoBackCommand()
    {

    }

    protected override void DoDeleteCommand()
    {

    }

    protected override void DoSaveCommand()
    {

    }

    protected override bool CanExecuteSaveCommand()
    {
        return SelectedGroupModel is not null
               && int.TryParse(Count.Replace(ProgramConstants.NUMBER_GROUP_SEPARATOR, string.Empty), out int _)
               && decimal.TryParse(SoldPrice, out decimal _);
    }

    private void SetTitle(BaseSkinModel? model)
    {
        if (model is null) Title = TITLE;
        Title = $"{TITLE}: «{model?.Title}»";
    }

    private void SetValuesFromDefault()
    {
        SelectedGroupModel = DefaultGroupModel;
        Count = DefaultCount;
        SoldPrice = DefaultSoldPrice;
        SoldDate = DefaultSoldDate;
        Description = DefaultDescription;
    }

    public void SetSoldActive(ActiveModel? model)
    {
        DefaultGroupModel = null;

        DefaultCount = $"{model?.Count ?? 1:N0}";

        DefaultSoldPrice = $"{model?.CurrentPrice ?? 1:N2}";

        DefaultSoldDate = DateTimeOffset.Now;

        DefaultDescription = model?.Description;

        BuyPrice = model?.BuyPriceString ?? string.Empty;

        TotalCount = model?.Count ?? 1;

        CurrentPrice = model?.CurrentPriceString ?? string.Empty;

        BuyDate = model?.BuyDateString ?? string.Empty;

        GoalPriceCompletion = model?.GoalPriceCompletion;

        SetTitle(model);

        SetValuesFromDefault();
    }

    #endregion Methods
}
