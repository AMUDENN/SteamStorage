using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.UtilityViewModels.BaseViewModels;

namespace SteamStorage.ViewModels.UtilityViewModels;

public class InventoryItemViewModel : BaseDynamicsSkinViewModel
{
    #region Fields

    private readonly InventoryItemModel _model;

    #endregion Fields

    #region Properties

    public int Count
    {
        get => _model.Count;
    }

    public string CurrentPriceString
    {
        get => _model.CurrentPriceString;
    }

    public string CurrentSumString
    {
        get => _model.CurrentSumString;
    }

    #endregion Properties

    #region Constructor

    public InventoryItemViewModel(
        InventoryItemModel model,
        ChartTooltipModel chartTooltipModel) : base(model, chartTooltipModel)
    {
        _model = model;
    }

    #endregion Constructor
}
