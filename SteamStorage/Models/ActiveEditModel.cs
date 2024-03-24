using System;
using System.Linq;
using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Utilities;
using SteamStorage.ViewModels.UtilityViewModels.BaseViewModels;
using SteamStorageAPI;

namespace SteamStorage.Models;

public class ActiveEditModel : BaseItemEditModel
{
    #region Constants

    private const string TITLE = "Изменение актива";

    #endregion Constants

    #region Fields

    private readonly ActiveGroupsModel _activeGroupsModel;
    
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
        ActiveGroupsModel activeGroupsModel) : base(apiClient)
    {
        _activeGroupsModel = activeGroupsModel;

        _defaultCount = string.Empty;
        _count = string.Empty;

        _defaultBuyPrice = string.Empty;
        _buyPrice = string.Empty;
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
        return SelectedActiveGroupModel is not null
               && int.TryParse(Count.Replace(ProgramConstants.NUMBER_GROUP_SEPARATOR, string.Empty), out int _)
               && decimal.TryParse(BuyPrice, out decimal _)
               && (string.IsNullOrEmpty(GoalPrice) || decimal.TryParse(GoalPrice, out decimal _))
               && Description?.Length <= 300
               && SelectedSkinModel is not null;
    }

    protected override void SetTitle(BaseSkinViewModel? model)
    {
        if (model is null) Title = TITLE;
        Title = $"{TITLE}: «{model?.Title}»";
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
        DefaultActiveGroupModel = _activeGroupsModel.ActiveGroupModels.FirstOrDefault(x => x.GroupId == model?.GroupId);

        DefaultCount = $"{model?.Count ?? 1:N0}";

        DefaultBuyPrice = $"{model?.BuyPrice ?? 1:N2}";

        DefaultGoalPrice = $"{model?.GoalPrice:N2}";

        DefaultSkinModel = model is not null ? new(model) : null;

        DefaultDescription = model?.Description ?? string.Empty;

        DefaultBuyDate = DateTime.SpecifyKind(model?.BuyDate ?? DateTime.Now, DateTimeKind.Local);

        SetValuesFromDefault();
    }

    public void SetAddActive(ActiveGroupModel? model)
    {
        DefaultActiveGroupModel = _activeGroupsModel.ActiveGroupModels.FirstOrDefault(x => x.GroupId == model?.GroupId);

        DefaultCount = "1";

        DefaultBuyPrice = "1";

        DefaultGoalPrice = null;

        DefaultSkinModel = null;

        DefaultDescription = string.Empty;

        DefaultBuyDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        SetValuesFromDefault();
    }

    public void SetAddActive(ListItemModel? model)
    {
        DefaultActiveGroupModel = null;

        DefaultCount = "1";

        DefaultBuyPrice = $"{model?.CurrentPrice ?? 1:N2}";

        DefaultGoalPrice = null;

        DefaultSkinModel = model is not null ? new(model) : null;

        DefaultDescription = string.Empty;

        DefaultBuyDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        SetValuesFromDefault();
    }

    #endregion Methods
}
