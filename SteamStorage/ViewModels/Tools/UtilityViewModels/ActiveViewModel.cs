using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.Actives;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;

namespace SteamStorage.ViewModels.Tools.UtilityViewModels;

public class ActiveViewModel : BaseDynamicsSkinViewModel
{
    #region Fields

    private readonly ActiveModel _model;
    private readonly ListActivesModel _listActivesModel;

    #endregion Fields

    #region Properties

    public ActiveModel ActiveModel => _model;

    public int Count => _model.Count;

    public string BuyPriceString => _model.BuyPriceString;

    public string CurrentPriceString => _model.CurrentPriceString;

    public string CurrentSumString => _model.CurrentSumString;

    public string GoalPriceString => _model.GoalPriceString;

    public decimal Change => _model.Change;

    public string BuyDateString => _model.BuyDateString;

    #endregion Properties

    #region Commands

    public RelayCommand<ActiveModel> EditCommand => _listActivesModel.EditCommand;

    public RelayCommand<ActiveModel> SoldCommand => _listActivesModel.SoldCommand;

    public AsyncRelayCommand<ActiveModel> DeleteCommand => _listActivesModel.DeleteCommand;

    #endregion Commands

    #region Constructor

    public ActiveViewModel(
        ActiveModel model,
        ListActivesModel listActivesModel,
        ChartTooltipModel chartTooltipModel) : base(model, chartTooltipModel)
    {
        _model = model;
        _listActivesModel = listActivesModel;

        listActivesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}