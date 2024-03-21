using System;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Utilities;

namespace SteamStorage.Models;

public class ActiveEditModel : ModelBase
{
    #region Constants

    private const string TITLE = "Изменение актива";

    #endregion Constants

    #region Fields

    private readonly ActiveGroupsModel _activeGroupsModel;

    private string _title;

    private BaseGroupModel? _defaultGroupModel;
    private BaseGroupModel? _selectedGroupModel;

    private string _defaultCount;
    private string _count;

    private string _defaultBuyPrice;
    private string _buyPrice;

    private string? _defaultGoalPrice;
    private string? _goalPrice;

    //item

    private string? _defaultDescription;
    private string? _description;

    private DateTimeOffset _defaultBuyDate;
    private DateTimeOffset _buyDate;

    #endregion Fields

    #region Properties

    public string Title
    {
        get => _title;
        private set => SetProperty(ref _title, value);
    }

    public BaseGroupModel? DefaultGroupModel
    {
        get => _defaultGroupModel;
        private set => SetProperty(ref _defaultGroupModel, value);
    }

    public BaseGroupModel? SelectedGroupModel
    {
        get => _selectedGroupModel;
        set
        {
            SetProperty(ref _selectedGroupModel, value);
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

    #region Commands

    public RelayCommand DeleteCommand { get; }

    public RelayCommand SaveCommand { get; }

    #endregion Commands

    #region Constructor

    public ActiveEditModel(
        ActiveGroupsModel activeGroupsModel)
    {
        _activeGroupsModel = activeGroupsModel;

        _title = string.Empty;

        _defaultCount = string.Empty;
        _count = string.Empty;

        _defaultBuyPrice = string.Empty;
        _buyPrice = string.Empty;

        DeleteCommand = new(DoDeleteCommand);
        SaveCommand = new(DoSaveCommand, CanExecuteSaveCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoDeleteCommand()
    {

    }

    private void DoSaveCommand()
    {

    }

    private bool CanExecuteSaveCommand()
    {
        return SelectedGroupModel is not null
               && int.TryParse(Count.Replace(ProgramConstants.NUMBER_GROUP_SEPARATOR, string.Empty), out int _)
               && decimal.TryParse(BuyPrice, out decimal _)
               && (string.IsNullOrEmpty(GoalPrice) || decimal.TryParse(GoalPrice, out decimal _));
    }

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
        SetTitle(model);

        DefaultGroupModel = _activeGroupsModel.ActiveGroupModels.FirstOrDefault(x => x.GroupId == model?.GroupId);

        DefaultCount = $"{model?.Count ?? 1:N0}";

        DefaultBuyPrice = $"{model?.BuyPrice ?? 1:N2}";

        DefaultGoalPrice = $"{model?.GoalPrice:N2}";

        DefaultDescription = model?.Description;

        DefaultBuyDate = DateTime.SpecifyKind(model?.BuyDate ?? DateTime.Now, DateTimeKind.Local);

        SetValuesFromDefault();
    }

    public void SetAddActive(ActiveGroupModel? model)
    {
        SetTitle(null);

        DefaultGroupModel = _activeGroupsModel.ActiveGroupModels.FirstOrDefault(x => x.GroupId == model?.GroupId);

        DefaultCount = "1";

        DefaultBuyPrice = "1";

        DefaultGoalPrice = null;

        DefaultDescription = null;

        DefaultBuyDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        SetValuesFromDefault();
    }

    public void SetAddActive(ListItemModel? model)
    {
        SetTitle(model);

        DefaultGroupModel = null;

        DefaultCount = "1";

        DefaultBuyPrice = $"{model?.CurrentPrice ?? 1:N2}";

        DefaultGoalPrice = null;

        DefaultDescription = null;

        DefaultBuyDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        SetValuesFromDefault();
    }

    private void SetTitle(BaseSkinModel? model)
    {
        if (model is null) Title = TITLE;
        Title = $"{TITLE}: «{model?.Title}»";
    }

    #endregion Methods
}
