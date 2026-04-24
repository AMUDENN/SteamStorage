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

    public ArchiveModel ArchiveModel => _model;

    public int Count => _model.Count;

    public string BuyPriceString => _model.BuyPriceString;

    public string SoldPriceString => _model.SoldPriceString;

    public string SoldSumString => _model.SoldSumString;

    public decimal Change => _model.Change;

    public string BuyDateString => _model.BuyDateString;

    public string SoldDateString => _model.SoldDateString;

    #endregion Properties

    #region Commands

    public RelayCommand<ArchiveModel> EditCommand => _listArchivesModel.EditCommand;

    public AsyncRelayCommand<ArchiveModel> DeleteCommand => _listArchivesModel.DeleteCommand;

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