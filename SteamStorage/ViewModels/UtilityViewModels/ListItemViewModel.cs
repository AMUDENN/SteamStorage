using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;

namespace SteamStorage.ViewModels.UtilityViewModels;

public class ListItemViewModel : BaseDynamicsSkinViewModel
{
    #region Fields

    private readonly ListItemModel _model;

    #endregion Fields

    #region Properties

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

    public RelayCommand AddToActivesCommand
    {
        get => _model.AddToActivesCommand;
    }

    public RelayCommand AddToArchiveCommand
    {
        get => _model.AddToArchiveCommand;
    }

    #endregion Commands

    #region Constructor

    public ListItemViewModel(
        ListItemModel model, 
        ChartTooltipModel chartTooltipModel) : base(model, chartTooltipModel)
    {
        _model = model;

        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        chartTooltipModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
