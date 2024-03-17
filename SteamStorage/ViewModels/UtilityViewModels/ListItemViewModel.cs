using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;

namespace SteamStorage.ViewModels.UtilityViewModels;

public class ListItemViewModel : BaseDynamicsSkinViewModel
{
    #region Fields

    private readonly ListItemModel _model;
    private readonly ListItemsModel _listItemsModel;

    #endregion Fields

    #region Properties

    public ListItemModel ListItemModel
    {
        get => _model;
    }
    
    public string CurrentPriceString
    {
        get => _model.CurrentPriceString;
    }

    public double Change7D
    {
        get => _model.Change7D;
    }

    public double Change30D
    {
        get => _model.Change30D;
    }

    public bool IsMarked
    {
        get => _model.IsMarked;
        set => _model.IsMarked = value;
    }

    #endregion Properties

    #region Commands

    public RelayCommand<ListItemModel> AddToActivesCommand
    {
        get => _listItemsModel.AddToActivesCommand;
    }

    public RelayCommand<ListItemModel> AddToArchiveCommand
    {
        get => _listItemsModel.AddToArchiveCommand;
    }

    #endregion Commands

    #region Constructor

    public ListItemViewModel(
        ListItemModel model, 
        ListItemsModel listItemsModel,
        ChartTooltipModel chartTooltipModel) : base(model, chartTooltipModel)
    {
        _model = model;
        _listItemsModel = listItemsModel;
        
        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        listItemsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        chartTooltipModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
