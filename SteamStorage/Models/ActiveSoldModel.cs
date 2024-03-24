using System;
using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Utilities;

namespace SteamStorage.Models;

public class ActiveSoldModel : BaseEditModel
{
    #region Constants

    private const string TITLE = "Продажа актива";

    #endregion Constants

    #region Fields

    private BaseGroupModel? _defaultArchiveGroupModel;
    private BaseGroupModel? _selectedArchiveGroupModel;

    private string _defaultCount;
    private string _count;

    private string _defaultSoldPrice;
    private string _soldPrice;

    private DateTimeOffset _defaultSoldDate;
    private DateTimeOffset _soldDate;

    private string? _defaultDescription;
    private string? _description;

    private ActiveModel? _activeModel;

    #endregion Fields

    #region Properties

    public BaseGroupModel? DefaultArchiveGroupModel
    {
        get => _defaultArchiveGroupModel;
        private set => SetProperty(ref _defaultArchiveGroupModel, value);
    }

    public BaseGroupModel? SelectedArchiveGroupModel
    {
        get => _selectedArchiveGroupModel;
        set
        {
            SetProperty(ref _selectedArchiveGroupModel, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string DefaultSoldCount
    {
        get => _defaultCount;
        private set => SetProperty(ref _defaultCount, value);
    }

    public string SoldCount
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
        set
        {
            SetProperty(ref _description, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string? BuyPrice
    {
        get => _activeModel?.BuyPriceString;
    }

    public int? Count
    {
        get => _activeModel?.Count;
    }

    public string? CurrentPriceString
    {
        get => _activeModel?.CurrentPriceString;
    }

    public string? BuyDate
    {
        get => _activeModel?.BuyDateString;
    }

    public double? GoalPriceCompletion
    {
        get => _activeModel?.GoalPriceCompletion;
    }

    #endregion Properties

    #region Constructor

    public ActiveSoldModel()
    {
        _defaultCount = string.Empty;
        _count = string.Empty;

        _defaultSoldPrice = string.Empty;
        _soldPrice = string.Empty;
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
        return SelectedArchiveGroupModel is not null
               && int.TryParse(SoldCount.Replace(ProgramConstants.NUMBER_GROUP_SEPARATOR, string.Empty), out int _)
               && decimal.TryParse(SoldPrice, out decimal _)
               && Description?.Length <= 300;
    }

    private void SetTitle(BaseSkinModel? model)
    {
        if (model is null) Title = TITLE;
        Title = $"{TITLE}: «{model?.Title}»";
    }

    private void SetValuesFromDefault()
    {
        SelectedArchiveGroupModel = DefaultArchiveGroupModel;
        SoldCount = DefaultSoldCount;
        SoldPrice = DefaultSoldPrice;
        SoldDate = DefaultSoldDate;
        Description = DefaultDescription;
    }

    public void SetSoldActive(ActiveModel? model)
    {
        DefaultArchiveGroupModel = null;

        DefaultSoldCount = $"{model?.Count ?? 1:N0}";

        DefaultSoldPrice = $"{model?.CurrentPrice ?? 1:N2}";

        DefaultSoldDate = DateTimeOffset.Now;

        DefaultDescription = model?.Description ?? string.Empty;

        _activeModel = model;

        if (model is not null) model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);

        SetTitle(model);

        SetValuesFromDefault();
    }

    #endregion Methods
}
