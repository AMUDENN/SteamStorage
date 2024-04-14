using System;
using System.Threading;
using System.Threading.Tasks;
using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Services.DialogService;
using SteamStorage.Utilities;
using SteamStorage.Utilities.Dialog;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models;

public class ActiveSoldModel : BaseEditModel
{
    #region Constants

    private const string TITLE = "Продажа актива";

    private const string NO_DATA = "(нет данных)";

    #endregion Constants

    #region Fields

    private ActiveModel? _activeModel;
    
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

    private string _buyPriceString;
    private string _countString;
    private string _currentPriceString;
    private string _buyDateString;
    private string _goalPriceCompletion;

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

    public string BuyPriceString
    {
        get => _buyPriceString;
        private set => SetProperty(ref _buyPriceString, value);
    }

    public string CountString
    {
        get => _countString;
        private set => SetProperty(ref _countString, value);
    }

    public string CurrentPriceString
    {
        get => _currentPriceString;
        private set => SetProperty(ref _currentPriceString, value);
    }

    public string BuyDateString
    {
        get => _buyDateString;
        private set => SetProperty(ref _buyDateString, value);
    }

    public string GoalPriceCompletion
    {
        get => _goalPriceCompletion;
        private set => SetProperty(ref _goalPriceCompletion, value);
    }

    #endregion Properties

    #region Constructor

    public ActiveSoldModel(
        ApiClient apiClient,
        IDialogService dialogService) : base(apiClient, dialogService)
    {
        _defaultCount = string.Empty;
        _count = string.Empty;

        _defaultSoldPrice = string.Empty;
        _soldPrice = string.Empty;

        _buyPriceString = string.Empty;
        _countString = string.Empty;
        _currentPriceString = string.Empty;
        _buyDateString = string.Empty;
        _goalPriceCompletion = string.Empty;
    }

    #endregion Constructor

    #region Methods

    protected override async Task DoDeleteCommand(CancellationToken cancellationToken)
    {
        if (_activeModel is null) return;
        
        bool result = await DialogService.ShowDialogAsync(
            $"Вы уверены, что хотите удалить актив: «{_activeModel.Title}»?",
            DialogUtility.MessageType.Question,
            DialogUtility.MessageButtons.OkCancel);
        
        if (!result) return;

        await ApiClient.DeleteAsync(
            ApiConstants.ApiMethods.DeleteActive,
            new Actives.DeleteActiveRequest(_activeModel.ActiveId),
            cancellationToken);
        
        OnItemDeleted();
        
        OnGoingBack();
    }

    protected override async Task DoSaveCommand(CancellationToken cancellationToken)
    {
        if (_activeModel is null || SelectedArchiveGroupModel is null) return;

        if (!((int.TryParse(SoldCount.Replace(ProgramConstants.NUMBER_GROUP_SEPARATOR, string.Empty), out int count) && count > 0)
              && _activeModel.Count >= count
              && (decimal.TryParse(SoldPrice, out decimal price) && price >= (decimal)0.01)
              && Description?.Length <= 300))
            return;

        bool result = await DialogService.ShowDialogAsync(
            $"Вы уверены, что хотите продать актив: «{_activeModel.Title}»?",
            DialogUtility.MessageType.Question,
            DialogUtility.MessageButtons.SaveCancel);

        if (!result) return;

        await ApiClient.PutAsync(
            ApiConstants.ApiMethods.SoldActive,
            new Actives.SoldActiveRequest(_activeModel.ActiveId,
                SelectedArchiveGroupModel.GroupId,
                count,
                price,
                SoldDate.DateTime,
                Description),
            cancellationToken);

        OnItemChanged();

        OnGoingBack();
    }

    protected override bool CanExecuteSaveCommand()
    {
        return SelectedArchiveGroupModel is not null
               && (int.TryParse(SoldCount.Replace(ProgramConstants.NUMBER_GROUP_SEPARATOR, string.Empty), out int count) && count > 0)
               && (_activeModel is null || _activeModel.Count >= count)
               && (decimal.TryParse(SoldPrice, out decimal price) && price >= (decimal)0.01)
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
        _activeModel = model;
        
        DefaultArchiveGroupModel = null;

        DefaultSoldCount = $"{model?.Count ?? 1:N0}";

        DefaultSoldPrice = $"{model?.CurrentPrice ?? 1:N2}";

        DefaultSoldDate = DateTimeOffset.Now;

        DefaultDescription = model?.Description ?? string.Empty;

        BuyPriceString = model?.BuyPriceString ?? NO_DATA;
        CountString = model?.Count is null ? NO_DATA : $"{model.Count:N0}";
        CurrentPriceString = model?.CurrentPriceString ?? NO_DATA;
        BuyDateString = model?.BuyDateString ?? NO_DATA;
        GoalPriceCompletion = model?.GoalPriceCompletion is null ? NO_DATA : $"{model.GoalPriceCompletion:N0}%";

        SetTitle(model);

        SetValuesFromDefault();
    }

    #endregion Methods
}
