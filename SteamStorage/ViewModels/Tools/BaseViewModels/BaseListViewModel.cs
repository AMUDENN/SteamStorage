using SteamStorage.Models.Tools.BaseModels;

namespace SteamStorage.ViewModels.Tools.BaseViewModels;

public abstract class BaseListViewModel : ViewModelBase
{
    #region Fields

    private readonly BaseListModel _baseListModel;

    #endregion Fields

    #region Properties

    public string? NotFoundText => _baseListModel.NotFoundText;

    public bool IsLoading => _baseListModel.IsLoading;

    public int? PageNumber
    {
        get => _baseListModel.PageNumber;
        set => _baseListModel.PageNumber = value;
    }

    public int CurrentPageNumber => _baseListModel.CurrentPageNumber;

    public int PagesCount => _baseListModel.PagesCount;

    public int DisplayItemsCountStart => _baseListModel.DisplayItemsCountStart;

    public int DisplayItemsCountEnd => _baseListModel.DisplayItemsCountEnd;

    public int SavedItemsCount => _baseListModel.SavedItemsCount;

    #endregion Properties

    #region Constructor

    protected BaseListViewModel(
        BaseListModel baseListModel)
    {
        _baseListModel = baseListModel;
        baseListModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}