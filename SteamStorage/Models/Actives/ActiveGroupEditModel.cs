using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SteamStorage.Models.Tools.BaseModels;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Services.DialogService;
using SteamStorage.Services.NotificationService;
using SteamStorage.Utilities.Dialog;
using SteamStorage.Utilities.Extensions;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models.Actives;

public class ActiveGroupEditModel : BaseGroupEditModel
{
    #region Fields
    
    private readonly PeriodsModel _periodsModel;
    private ActiveGroupModel? _activeGroupModel;

    private string? _defaultGoalSum;
    private string? _goalSum;

    private string _dateCreationString;
    private string _buySumString;
    private string _countString;
    private string _currentSumString;
    private string _goalSumCompletion;

    #endregion Fields

    #region Propperties

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
    
    public IEnumerable<PeriodModel> PeriodModels
    {
        get => _periodsModel.PeriodModels;
    }
    
    public PeriodModel? SelectedPeriodModel
    {
        get => _activeGroupModel?.SelectedPeriodModel;
        set
        {
            if (_activeGroupModel is not null)
                _activeGroupModel.SelectedPeriodModel = value;
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
        PeriodsModel periodsModel,
        IDialogService dialogService,
        INotificationService notificationService) : base(apiClient, dialogService, notificationService)
    {
        _periodsModel = periodsModel;

        _dateCreationString = string.Empty;
        _buySumString = string.Empty;
        _countString = string.Empty;
        _currentSumString = string.Empty;
        _goalSumCompletion = string.Empty;
    }

    #endregion Constructor

    #region Methods

    protected override async Task DoDeleteCommand(CancellationToken cancellationToken)
    {
        if (_activeGroupModel is null) return;

        bool result = await DialogService.ShowDialogAsync(
            $"Вы уверены, что хотите удалить группу: «{_activeGroupModel.Title}»?",
            DialogUtility.MessageType.Question,
            DialogUtility.MessageButtons.OkCancel);

        if (!result) return;

        await ApiClient.DeleteAsync(
            ApiConstants.ApiMethods.DeleteActiveGroup,
            new ActiveGroups.DeleteActiveGroupRequest(_activeGroupModel.GroupId),
            cancellationToken);
        
        await NotificationService.ShowAsync("Удаление группы",
            $"Вы отправили запрос на удаление группы: {_activeGroupModel.Title}", 
            cancellationToken: cancellationToken);

        OnItemDeleted();

        OnGoingBack();
    }

    protected override async Task DoSaveCommand(CancellationToken cancellationToken)
    {
        if (!(GroupTitle.Length is >= 3 and <= 100
              && Description?.Length <= 300
              && ((decimal.TryParse(GoalSum, out decimal sum) && sum >= (decimal)0.01) || string.IsNullOrWhiteSpace(GoalSum))
              && Colour != Colors.Transparent))
            return;
        
        if (IsNewGroup)
        {
            bool result = await DialogService.ShowDialogAsync(
                $"Вы уверены, что хотите добавить группу: «{GroupTitle}»?",
                DialogUtility.MessageType.Question,
                DialogUtility.MessageButtons.SaveCancel);

            if (!result) return;
            
            await ApiClient.PostAsync(
                ApiConstants.ApiMethods.PostActiveGroup,
                new ActiveGroups.PostActiveGroupRequest(GroupTitle,
                    Description,
                    Colour.ToHexColor(),
                    string.IsNullOrWhiteSpace(GoalSum) ? null : sum),
                cancellationToken);
            
            await NotificationService.ShowAsync("Добавление группы",
                $"Вы отправили запрос на добавление группы: {GroupTitle}", 
                cancellationToken: cancellationToken);
        }
        else if (_activeGroupModel is not null)
        {
            bool result = await DialogService.ShowDialogAsync(
                $"Вы уверены, что хотите изменить группу: «{_activeGroupModel.Title}»?",
                DialogUtility.MessageType.Question,
                DialogUtility.MessageButtons.SaveCancel);

            if (!result) return;

            await ApiClient.PutAsync(
                ApiConstants.ApiMethods.PutActiveGroup,
                new ActiveGroups.PutActiveGroupRequest(_activeGroupModel.GroupId,
                    GroupTitle,
                    Description,
                    Colour.ToHexColor(),
                    string.IsNullOrWhiteSpace(GoalSum) ? null : sum),
                cancellationToken);
            
            await NotificationService.ShowAsync("Изменение группы",
                $"Вы отправили запрос на изменение группы: {_activeGroupModel.Title}", 
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
        return GroupTitle.Length is >= 3 and <= 100
               && Description?.Length <= 300
               && (string.IsNullOrWhiteSpace(GoalSum) || (decimal.TryParse(GoalSum, out decimal sum) && sum >= (decimal)0.01))
               && Colour != Colors.Transparent;
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
