using SteamStorage.Models.Tools.BaseModels;

namespace SteamStorage.ViewModels.Tools.BaseViewModels;

public abstract class BaseListViewModel : ViewModelBase
{
    #region Fields

    private readonly BaseListModel _baseListModel;

    #endregion Fields
    
    #region Properties
    
    public string? NotFoundText
    {
        get => _baseListModel.NotFoundText;
    }
    
    public bool IsLoading
    {
        get => _baseListModel.IsLoading;
    }

    public int? PageNumber
    {
        get => _baseListModel.PageNumber;
        set => _baseListModel.PageNumber = value;
    }

    public int CurrentPageNumber
    {
        get => _baseListModel.CurrentPageNumber;
    }

    public int PagesCount
    {
        get => _baseListModel.PagesCount;
    }

    public int DisplayItemsCountStart
    {
        get => _baseListModel.DisplayItemsCountStart;
    }

    public int DisplayItemsCountEnd
    {
        get => _baseListModel.DisplayItemsCountEnd;
    }

    public int SavedItemsCount
    {
        get => _baseListModel.SavedItemsCount;
    }
    
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
