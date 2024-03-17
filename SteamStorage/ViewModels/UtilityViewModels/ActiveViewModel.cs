using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;

namespace SteamStorage.ViewModels.UtilityViewModels;

public class ActiveViewModel : BaseDynamicsSkinViewModel
{
    #region Fields

    private readonly ActiveModel _model;
    private readonly ListActivesModel _listActivesModel;

    #endregion Fields

    #region Properties

    public ActiveModel ActiveModel
    {
        get => _model;
    }
    
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

    public RelayCommand<ActiveModel> EditCommand
    {
        get => _listActivesModel.EditCommand;
    }
    
    public RelayCommand<ActiveModel> SoldCommand
    {
        get => _listActivesModel.SoldCommand;
    }

    public RelayCommand<ActiveModel> DeleteCommand
    {
        get => _listActivesModel.DeleteCommand;
    }

    #endregion Commands

    #region Constructor

    public ActiveViewModel(
        ActiveModel model,
        ListActivesModel listActivesModel,
        ChartTooltipModel chartTooltipModel) : base(model, chartTooltipModel)
    {
        _model = model;
        _listActivesModel = listActivesModel;

        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        listActivesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        chartTooltipModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
