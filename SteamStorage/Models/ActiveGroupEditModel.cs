using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Services.DialogService;
using SteamStorage.Utilities.Dialog;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ActiveGroupEditModel : BaseEditModel
{
    #region Constants

    private const string TITLE = "Изменение группы";

    private const string NO_DATA = "(нет данных)";

    #endregion Constants

    #region Fields

    private readonly IDialogService _dialogService;

    private ActiveGroupModel? _activeGroupModel;

    private string _defaultGroupTitle;
    private string _groupTitle;

    private string? _defaultDescription;
    private string? _description;

    private string? _defaultGoalSum;
    private string? _goalSum;

    private Color _defaultColour;
    private Color _colour;

    private bool _isNewGroup;

    private string _dateCreationString;
    private string _buySumString;
    private string _countString;
    private string _currentSumString;
    private string _goalSumCompletion;

    #endregion Fields

    #region Propperties

    public string DefaultGroupTitle
    {
        get => _defaultGroupTitle;
        private set => SetProperty(ref _defaultGroupTitle, value);
    }

    public string GroupTitle
    {
        get => _groupTitle;
        set
        {
            SetProperty(ref _groupTitle, value);
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

    public string? DefaultGoalSum
    {
        get => _defaultGoalSum;
        private set => SetProperty(ref _defaultGoalSum, value);
    }

    public string? GoalSum
    {
        get => _goalSum;
        set
        {
            SetProperty(ref _goalSum, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public Color DefaultColour
    {
        get => _defaultColour;
        private set => SetProperty(ref _defaultColour, value);
    }

    public Color Colour
    {
        get => _colour;
        set
        {
            SetProperty(ref _colour, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public bool IsNewGroup
    {
        get => _isNewGroup;
        private set => SetProperty(ref _isNewGroup, value);
    }

    public string DateCreationString
    {
        get => _dateCreationString;
        private set => SetProperty(ref _dateCreationString, value);
    }

    public string BuySumString
    {
        get => _buySumString;
        private set => SetProperty(ref _buySumString, value);
    }

    public string CountString
    {
        get => _countString;
        private set => SetProperty(ref _countString, value);
    }

    public string CurrentSumString
    {
        get => _currentSumString;
        private set => SetProperty(ref _currentSumString, value);
    }

    public string GoalSumCompletion
    {
        get => _goalSumCompletion;
        private set => SetProperty(ref _goalSumCompletion, value);
    }

    public double ChangePeriod
    {
        get => _activeGroupModel?.ChangePeriod ?? 0;
    }

    public string DatePeriod
    {
        get => _activeGroupModel?.DatePeriod ?? NO_DATA;
    }

    public string? NotFoundText
    {
        get => _activeGroupModel?.NotFoundText;
    }

    public IEnumerable<ISeries> ChangeSeries
    {
        get => _activeGroupModel?.ChangeSeries ?? Enumerable.Empty<ISeries>();
    }

    public IEnumerable<Axis> XAxis
    {
        get => _activeGroupModel?.XAxis ?? Enumerable.Empty<Axis>();
    }

    public IEnumerable<Axis> YAxis
    {
        get => _activeGroupModel?.YAxis ?? Enumerable.Empty<Axis>();
    }

    public bool IsOneDayChecked
    {
        get => _activeGroupModel?.IsOneDayChecked ?? false;
        set
        {
            if (_activeGroupModel is not null)
                _activeGroupModel.IsOneDayChecked = value;
        }
    }

    public bool IsOneWeekChecked
    {
        get => _activeGroupModel?.IsOneWeekChecked ?? false;
        set
        {
            if (_activeGroupModel is not null)
                _activeGroupModel.IsOneWeekChecked = value;
        }
    }

    public bool IsOneMonthChecked
    {
        get => _activeGroupModel?.IsOneMonthChecked ?? false;
        set
        {
            if (_activeGroupModel is not null)
                _activeGroupModel.IsOneMonthChecked = value;
        }
    }

    public bool IsOneYearChecked
    {
        get => _activeGroupModel?.IsOneYearChecked ?? false;
        set
        {
            if (_activeGroupModel is not null)
                _activeGroupModel.IsOneYearChecked = value;
        }
    }

    public bool? IsLoading
    {
        get => _activeGroupModel?.IsLoading;
    }

    #endregion Properties

    #region Constructor

    public ActiveGroupEditModel(
        ApiClient apiClient,
        IDialogService dialogService) : base(apiClient)
    {
        _dialogService = dialogService;

        _defaultGroupTitle = string.Empty;
        _groupTitle = string.Empty;

        _dateCreationString = string.Empty;
        _buySumString = string.Empty;
        _countString = string.Empty;
        _currentSumString = string.Empty;
        _goalSumCompletion = string.Empty;
    }

    #endregion Constructor

    #region Methods

    protected override async Task DoDeleteCommand()
    {
        if (_activeGroupModel is null) return;

        bool result = await _dialogService.ShowDialogAsync(
            $"Вы уверены, что хотите удалить группу: «{_activeGroupModel.Title}»?",
            DialogUtility.MessageType.Question,
            DialogUtility.MessageButtons.OkCancel);

        if (!result) return;

        await ApiClient.DeleteAsync(
            ApiConstants.ApiMethods.DeleteActiveGroup,
            new ActiveGroups.DeleteActiveGroupRequest(_activeGroupModel.GroupId));

        OnItemDeleted();

        OnGoingBack();
    }

    protected override async Task DoSaveCommand()
    {
        if (!(GroupTitle.Length is >= 3 and <= 100
              && Description?.Length <= 300
              && ((decimal.TryParse(GoalSum, out decimal sum) && sum >= (decimal)0.01) || string.IsNullOrWhiteSpace(GoalSum))
              && Colour != Colors.Transparent))
            return;
        
        System.Diagnostics.Debug.WriteLine(Colour.ToString().Trim('#'));
        
        if (IsNewGroup)
        {
            bool result = await _dialogService.ShowDialogAsync(
                $"Вы уверены, что хотите добавить группу: «{GroupTitle}»?",
                DialogUtility.MessageType.Question,
                DialogUtility.MessageButtons.SaveCancel);

            if (!result) return;

            await ApiClient.PostAsync(
                ApiConstants.ApiMethods.PostActiveGroup,
                new ActiveGroups.PostActiveGroupRequest(GroupTitle,
                    Description,
                    Colour.ToString().Trim('#'),
                    string.IsNullOrWhiteSpace(GoalSum) ? null : sum));
        }
        else if (_activeGroupModel is not null)
        {
            bool result = await _dialogService.ShowDialogAsync(
                $"Вы уверены, что хотите изменить группу: «{_activeGroupModel.Title}»?",
                DialogUtility.MessageType.Question,
                DialogUtility.MessageButtons.SaveCancel);

            if (!result) return;

            await ApiClient.PutAsync(
                ApiConstants.ApiMethods.PutActiveGroup,
                new ActiveGroups.PutActiveGroupRequest(_activeGroupModel.GroupId,
                    GroupTitle,
                    Description,
                    Colour.ToString().Trim('#'),
                    string.IsNullOrWhiteSpace(GoalSum) ? null : sum));
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
        return GroupTitle.Length is >= 3 and <= 100
               && Description?.Length <= 300
               && (string.IsNullOrWhiteSpace(GoalSum) || (decimal.TryParse(GoalSum, out decimal sum) && sum >= (decimal)0.01))
               && Colour != Colors.Transparent;
    }

    private void SetTitle(BaseGroupModel? model)
    {
        if (model is null) Title = TITLE;
        Title = $"{TITLE}: «{model?.Title}»";
    }

    private void SetValuesFromDefault()
    {
        GroupTitle = DefaultGroupTitle;
        Description = DefaultDescription;
        GoalSum = DefaultGoalSum;
        Colour = DefaultColour;
    }

    public void SetEditGroup(ActiveGroupModel? model)
    {
        _activeGroupModel = model;

        DefaultGroupTitle = model?.Title ?? string.Empty;

        DefaultDescription = model?.Description ?? string.Empty;

        DefaultGoalSum = $"{model?.GoalSum:N2}";

        bool isColor = Color.TryParse(model?.Colour, out Color color);
        DefaultColour = isColor ? color : Colors.Black;

        DateCreationString = model?.DateCreationString ?? NO_DATA;
        BuySumString = model?.BuySumString ?? NO_DATA;
        CountString = model?.Count is null ? NO_DATA : $"{model.Count:N0}";
        CurrentSumString = model?.CurrentSumString ?? NO_DATA;
        GoalSumCompletion = model?.GoalSumCompletion is null ? NO_DATA : $"{model.GoalSumCompletion:N0}%";

        if (model is null)
        {
            IsNewGroup = true;
        }
        else
        {
            model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
            model.UpdateStats();
            IsNewGroup = false;
        }

        SetTitle(model);

        SetValuesFromDefault();
    }

    #endregion Methods
}
