using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SteamStorage.Models.Tools.BaseModels;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;
using SteamStorage.Services.DialogService;
using SteamStorage.Services.NotificationService;
using SteamStorage.Utilities;
using SteamStorage.Utilities.Dialog;
using SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models.Actives;

public class ActiveEditModel : BaseItemEditModel
{
    #region Constants

    private const string CHANGE_TITLE = "Изменение актива";
    
    private const string ADD_TITLE = "Добавление актива";

    #endregion Constants

    #region Fields
    
    private readonly ActiveGroupsModel _activeGroupsModel;

    private ActiveModel? _activeModel;

    private BaseGroupModel? _defaultActiveGroupModel;
    private BaseGroupModel? _selectedActiveGroupModel;

    private string _defaultCount;
    private string _count;

    private string _defaultBuyPrice;
    private string _buyPrice;

    private string? _defaultGoalPrice;
    private string? _goalPrice;

    private string? _defaultDescription;
    private string? _description;

    private DateTimeOffset _defaultBuyDate;
    private DateTimeOffset _buyDate;

    #endregion Fields

    #region Properties

    public BaseGroupModel? DefaultActiveGroupModel
    {
        get => _defaultActiveGroupModel;
        private set => SetProperty(ref _defaultActiveGroupModel, value);
    }

    public BaseGroupModel? SelectedActiveGroupModel
    {
        get => _selectedActiveGroupModel;
        set
        {
            SetProperty(ref _selectedActiveGroupModel, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

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

    public string DefaultBuyPrice
    {
        get => _defaultBuyPrice;
        private set => SetProperty(ref _defaultBuyPrice, value);
    }

    public string BuyPrice
    {
        get => _buyPrice;
        set
        {
            SetProperty(ref _buyPrice, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string? DefaultGoalPrice
    {
        get => _defaultGoalPrice;
        private set => SetProperty(ref _defaultGoalPrice, value);
    }

    public string? GoalPrice
    {
        get => _goalPrice;
        set
        {
            SetProperty(ref _goalPrice, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
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
        ApiClient apiClient,
        ActiveGroupsModel activeGroupsModel,
        IDialogService dialogService,
        INotificationService notificationService) : base(apiClient, dialogService, notificationService)
    {
        _activeGroupsModel = activeGroupsModel;

        _defaultCount = string.Empty;
        _count = string.Empty;

        _defaultBuyPrice = string.Empty;
        _buyPrice = string.Empty;
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
            new SteamStorageAPI.SDK.ApiEntities.Actives.DeleteActiveRequest(_activeModel.ActiveId),
            cancellationToken);

        await NotificationService.ShowAsync("Удаление актива",
            $"Вы отправили запрос на удаление актива: {_activeModel.Title}", 
            cancellationToken: cancellationToken);

        OnItemDeleted();

        OnGoingBack();
    }

    protected override async Task DoSaveCommand(CancellationToken cancellationToken)
    {
        if (SelectedSkinModel is null || SelectedActiveGroupModel is null) return;

        if (!((int.TryParse(Count.Replace(ProgramConstants.NUMBER_GROUP_SEPARATOR, string.Empty), out int count) && count > 0)
              && (decimal.TryParse(BuyPrice, out decimal price) && price >= (decimal)0.01)
              && ((decimal.TryParse(GoalPrice, out decimal goal) && goal >= (decimal)0.01) || string.IsNullOrWhiteSpace(GoalPrice))
              && Description?.Length <= 300))
            return;


        if (IsNewItem)
        {
            bool result = await DialogService.ShowDialogAsync(
                $"Вы уверены, что хотите добавить актив: «{SelectedSkinModel.Title}»?",
                DialogUtility.MessageType.Question,
                DialogUtility.MessageButtons.SaveCancel);

            if (!result) return;

            await ApiClient.PostAsync(
                ApiConstants.ApiMethods.PostActive,
                new SteamStorageAPI.SDK.ApiEntities.Actives.PostActiveRequest(SelectedActiveGroupModel.GroupId,
                    count,
                    price,
                    string.IsNullOrWhiteSpace(GoalPrice) ? null : goal,
                    SelectedSkinModel.SkinId,
                    Description,
                    BuyDate.DateTime),
                cancellationToken);
            
            await NotificationService.ShowAsync("Добавление актива",
                $"Вы отправили запрос на добавление актива: {SelectedSkinModel.Title}", 
                cancellationToken: cancellationToken);
        }
        else if (_activeModel is not null)
        {
            bool result = await DialogService.ShowDialogAsync(
                $"Вы уверены, что хотите изменить актив: «{_activeModel.Title}»?",
                DialogUtility.MessageType.Question,
                DialogUtility.MessageButtons.SaveCancel);

            if (!result) return;

            await ApiClient.PutAsync(
                ApiConstants.ApiMethods.PutActive,
                new SteamStorageAPI.SDK.ApiEntities.Actives.PutActiveRequest(_activeModel.ActiveId,
                    SelectedActiveGroupModel.GroupId,
                    count,
                    price,
                    string.IsNullOrWhiteSpace(GoalPrice) ? null : goal,
                    SelectedSkinModel.SkinId,
                    Description,
                    BuyDate.DateTime),
                cancellationToken);
            
            await NotificationService.ShowAsync("Изменение актива",
                $"Вы отправили запрос на изменение актива: {_activeModel.Title}", 
                cancellationToken: cancellationToken);
        }
        else
        {
            return;
        }

        OnItemChanged();

        OnGoingBack();
    }

    protected override bool CanExecuteSaveCommand()
    {
        return SelectedActiveGroupModel is not null
               && (int.TryParse(Count.Replace(ProgramConstants.NUMBER_GROUP_SEPARATOR, string.Empty), out int count) && count > 0)
               && (decimal.TryParse(BuyPrice, out decimal price) && price >= (decimal)0.01)
               && (string.IsNullOrWhiteSpace(GoalPrice) || (decimal.TryParse(GoalPrice, out decimal goal) && goal >= (decimal)0.01))
               && Description?.Length <= 300
               && SelectedSkinModel is not null;
    }

    protected override void SetTitle(BaseSkinViewModel? model, bool isNewItem)
    {
        if (isNewItem)
        {
            Title = ADD_TITLE;
            return;
        }
        if (model is null) Title = CHANGE_TITLE;
        Title = $"{CHANGE_TITLE}: «{model?.Title}»";
    }

    private void SetValuesFromDefault()
    {
        SelectedActiveGroupModel = DefaultActiveGroupModel;
        Count = DefaultCount;
        BuyPrice = DefaultBuyPrice;
        GoalPrice = DefaultGoalPrice;
        SelectedSkinModel = DefaultSkinModel;
        Description = DefaultDescription;
        BuyDate = DefaultBuyDate;
    }

    public void SetEditActive(ActiveModel? model)
    {
        _activeModel = model;

        DefaultActiveGroupModel = _activeGroupsModel.ActiveGroupModels.FirstOrDefault(x => x.GroupId == model?.GroupId);

        DefaultCount = $"{model?.Count ?? 1:N0}";

        DefaultBuyPrice = $"{model?.BuyPrice ?? 1:N2}";

        DefaultGoalPrice = $"{model?.GoalPrice:N2}";

        DefaultSkinModel = model is not null ? new(model) : null;
        Filter = model?.Title;

        DefaultDescription = model?.Description ?? string.Empty;

        DefaultBuyDate = DateTime.SpecifyKind(model?.BuyDate ?? DateTime.Now, DateTimeKind.Local);

        IsNewItem = model is null;

        SetValuesFromDefault();
    }

    public void SetAddActive(ActiveGroupModel? model)
    {
        _activeModel = null;

        DefaultActiveGroupModel = _activeGroupsModel.ActiveGroupModels.FirstOrDefault(x => x.GroupId == model?.GroupId);

        DefaultCount = "1";

        DefaultBuyPrice = "1";

        DefaultGoalPrice = null;

        DefaultSkinModel = null;
        Filter = null;

        DefaultDescription = string.Empty;

        DefaultBuyDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        IsNewItem = true;

        SetValuesFromDefault();
    }

    public void SetAddActive(ListItemModel? model)
    {
        _activeModel = null;

        DefaultActiveGroupModel = null;

        DefaultCount = "1";

        DefaultBuyPrice = $"{model?.CurrentPrice ?? 1:N2}";

        DefaultGoalPrice = null;

        DefaultSkinModel = model is not null ? new(model) : null;
        Filter = model?.Title;

        DefaultDescription = string.Empty;

        DefaultBuyDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        IsNewItem = true;

        SetValuesFromDefault();
    }

    #endregion Methods
}
