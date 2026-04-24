using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.Home;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;

namespace SteamStorage.ViewModels.Tools.UtilityViewModels;

public class ListItemViewModel : BaseDynamicsSkinViewModel
{
    #region Fields

    private readonly ListItemModel _model;
    private readonly ListItemsModel _listItemsModel;

    #endregion Fields

    #region Properties

    public ListItemModel ListItemModel => _model;

    public string CurrentPriceString => _model.CurrentPriceString;

    public decimal Change7D => _model.Change7D;

    public decimal Change30D => _model.Change30D;

    public bool IsMarked
    {
        get => _model.IsMarked;
        set => _model.IsMarked = value;
    }

    #endregion Properties

    #region Commands

    public RelayCommand<ListItemModel> AddToActivesCommand => _listItemsModel.AddToActivesCommand;

    public RelayCommand<ListItemModel> AddToArchiveCommand => _listItemsModel.AddToArchiveCommand;

    #endregion Commands

    #region Constructor

    public ListItemViewModel(
        ListItemModel model,
        ListItemsModel listItemsModel,
        ChartTooltipModel chartTooltipModel) : base(model, chartTooltipModel)
    {
        _model = model;
        _listItemsModel = listItemsModel;

        listItemsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}