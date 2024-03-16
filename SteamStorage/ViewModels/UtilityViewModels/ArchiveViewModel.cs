using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;

namespace SteamStorage.ViewModels.UtilityViewModels;

public class ArchiveViewModel : BaseSkinViewModel
{
    #region Fields

    private readonly ArchiveModel _model;

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

    public string SoldPriceString
    {
        get => _model.SoldPriceString;
    }

    public string SoldSumString
    {
        get => _model.SoldSumString;
    }

    public double Change
    {
        get => _model.Change;
    }
    
    public string BuyDateString
    {
        get => _model.BuyDateString;
    }
    
    public string SoldDateString
    {
        get => _model.SoldDateString;
    }

    #endregion Properties

    #region Commands

    public RelayCommand EditCommand
    {
        get => _model.EditCommand;
    }

    public RelayCommand DeleteCommand
    {
        get => _model.DeleteCommand;
    }

    #endregion Commands

    #region Constructor

    public ArchiveViewModel(
        ArchiveModel model) : base(model)
    {
        _model = model;

        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
