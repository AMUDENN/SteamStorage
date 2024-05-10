using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Archives;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;

namespace SteamStorage.ViewModels.Tools.UtilityViewModels;

public class ArchiveViewModel : BaseSkinViewModel
{
    #region Fields

    private readonly ArchiveModel _model;
    private readonly ListArchivesModel _listArchivesModel;

    #endregion Fields

    #region Properties

    public ArchiveModel ArchiveModel
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

    public RelayCommand<ArchiveModel> EditCommand
    {
        get => _listArchivesModel.EditCommand;
    }

    public AsyncRelayCommand<ArchiveModel> DeleteCommand
    {
        get => _listArchivesModel.DeleteCommand;
    }

    #endregion Commands

    #region Constructor

    public ArchiveViewModel(
        ArchiveModel model,
        ListArchivesModel listArchivesModel) : base(model)
    {
        _model = model;
        _listArchivesModel = listArchivesModel;

        listArchivesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
