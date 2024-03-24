using System.Collections.Generic;
using System.Text.RegularExpressions;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Utilities;

namespace SteamStorage.Models;

public class ActiveGroupEditModel : BaseEditModel
{
    #region Constants

    private const string TITLE = "Изменение группы";

    #endregion Constants

    #region Fields

    private string _defaultGroupTitle;
    private string _groupTitle;

    private string? _defaultDescription;
    private string? _description;

    private string? _defaultGoalSum;
    private string? _goalSum;

    private string? _defaultColour;
    private string? _colour;

    private bool _isNewGroup;

    private ActiveGroupModel? _activeGroupModel;

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
            SetTitle(_activeGroupModel);
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

    public string? DefaultColour
    {
        get => _defaultColour;
        private set => SetProperty(ref _defaultColour, value);
    }

    public string? Colour
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

    public string? DateCreationString
    {
        get => _activeGroupModel?.DateCreationString;
    }

    public string? BuySumString
    {
        get => _activeGroupModel?.BuySumString;
    }

    public int? Count
    {
        get => _activeGroupModel?.Count;
    }

    public string? CurrentSumString
    {
        get => _activeGroupModel?.CurrentSumString;
    }

    public double? GoalSumCompletion
    {
        get => _activeGroupModel?.GoalSumCompletion;
    }

    public double? ChangePeriod
    {
        get => _activeGroupModel?.ChangePeriod;
    }

    public string? DatePeriod
    {
        get => _activeGroupModel?.DatePeriod;
    }

    public string? NotFoundText
    {
        get => _activeGroupModel?.NotFoundText;
    }

    public IEnumerable<ISeries>? ChangeSeries
    {
        get => _activeGroupModel?.ChangeSeries;
    }

    public IEnumerable<Axis>? XAxis
    {
        get => _activeGroupModel?.XAxis;
    }

    public IEnumerable<Axis>? YAxis
    {
        get => _activeGroupModel?.YAxis;
    }

    public bool? IsOneDayChecked
    {
        get => _activeGroupModel?.IsOneDayChecked;
        set
        {
            if (_activeGroupModel is not null && value is not null)
                _activeGroupModel.IsOneDayChecked = (bool)value;
        }
    }

    public bool? IsOneWeekChecked
    {
        get => _activeGroupModel?.IsOneWeekChecked;
        set
        {
            if (_activeGroupModel is not null && value is not null)
                _activeGroupModel.IsOneWeekChecked = (bool)value;
        }
    }

    public bool? IsOneMonthChecked
    {
        get => _activeGroupModel?.IsOneMonthChecked;
        set
        {
            if (_activeGroupModel is not null && value is not null)
                _activeGroupModel.IsOneMonthChecked = (bool)value;
        }
    }

    public bool? IsOneYearChecked
    {
        get => _activeGroupModel?.IsOneYearChecked;
        set
        {
            if (_activeGroupModel is not null && value is not null)
                _activeGroupModel.IsOneYearChecked = (bool)value;
        }
    }

    public bool? IsLoading
    {
        get => _activeGroupModel?.IsLoading;
    }

    #endregion Properties

    #region Constructor

    public ActiveGroupEditModel()
    {
        _defaultGroupTitle = string.Empty;
        _groupTitle = string.Empty;
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
        return Title.Length is >= 3 and <= 100
               && Description?.Length <= 300
               && (string.IsNullOrEmpty(GoalSum) || decimal.TryParse(GoalSum, out decimal _))
               && (string.IsNullOrEmpty(Colour) || Regex.IsMatch(Colour, ProgramConstants.COLOUR_PATTERN));
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
        DefaultGroupTitle = model?.Title ?? string.Empty;

        DefaultDescription = model?.Description;

        DefaultGoalSum = model?.GoalSumString;

        DefaultColour = model?.Colour;

        _activeGroupModel = model;

        if (model is null)
            IsNewGroup = true;
        else
        {
            model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
            IsNewGroup = false;
        }

        SetTitle(model);

        SetValuesFromDefault();
    }

    #endregion Methods
}
