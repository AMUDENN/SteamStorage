using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;

namespace SteamStorage.ViewModels.UtilityViewModels;

public class ActiveViewModel : BaseDynamicsSkinViewModel
{
    #region Fields

    private readonly ActiveModel _model;

    #endregion Fields

    #region Properties

    public int Count
    {
        get => _model.Count;
    }

    public string BuyPriceString
    {
        get => _model.BuyPriceString;
    }

    public string CurrentPriceString
    {
        get => _model.CurrentPriceString;
    }

    public string CurrentSumString
    {
        get => _model.CurrentSumString;
    }

    public string GoalPriceString
    {
        get => _model.GoalPriceString;
    }

    public double Change
    {
        get => _model.Change;
    }

    public string BuyDateString
    {
        get => _model.BuyDateString;
    }

    #endregion Properties

    #region Commands

    public RelayCommand EditCommand
    {
        get => _model.EditCommand;
    }
    
    public RelayCommand SoldCommand
    {
        get => _model.SoldCommand;
    }

    public RelayCommand DeleteCommand
    {
        get => _model.DeleteCommand;
    }

    #endregion Commands

    #region Constructor

    public ActiveViewModel(
        ActiveModel model,
        ChartTooltipModel chartTooltipModel) : base(model, chartTooltipModel)
    {
        _model = model;

        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        chartTooltipModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
