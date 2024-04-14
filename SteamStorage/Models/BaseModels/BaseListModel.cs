using SteamStorage.Models.Tools;

namespace SteamStorage.Models.BaseModels;

public abstract class BaseListModel : ModelBase
{
    #region Constants

    protected const string EMPTY_LIST_TEXT = "Элементы не найдены";

    #endregion Constants

    #region Fields

    private bool _isLoading;

    private readonly int _pageSize;
    private int? _pageNumber;
    private int _currentPageNumber;
    private int _pagesCount;

    private int _displayItemsCountStart;
    private int _displayItemsCountEnd;
    private int _savedItemsCount;

    #endregion Fields

    #region Properties

    public abstract string? NotFoundText { get; }

    public bool IsLoading
    {
        get => _isLoading;
        protected set
        {
            SetProperty(ref _isLoading, value);
            OnPropertyChanged(nameof(NotFoundText));
        }
    }

    protected int PageSize
    {
        get => _pageSize;
        init
        {
            SetProperty(ref _pageSize, value);
            GetSkinsAsync();
        }
    }

    public int? PageNumber
    {
        get => _pageNumber;
        set
        {
            SetProperty(ref _pageNumber, value);
            if (PageNumber is not null) GetSkinsAsync();
        }
    }

    public int CurrentPageNumber
    {
        get => _currentPageNumber;
        protected set => SetProperty(ref _currentPageNumber, value);
    }

    public int PagesCount
    {
        get => _pagesCount;
        protected set
        {
            SetProperty(ref _pagesCount, value);
            if (value == 0)
            {
                PageNumber = 1;
            }
            else if (value < PageNumber)
            {
                PageNumber = value;
            }
        }
    }

    public int DisplayItemsCountStart
    {
        get => _displayItemsCountStart;
        protected set => SetProperty(ref _displayItemsCountStart, value < 1 ? 1 : value);
    }

    public int DisplayItemsCountEnd
    {
        get => _displayItemsCountEnd;
        protected set => SetProperty(ref _displayItemsCountEnd, value < PageSize ? PageSize : value);
    }

    public int SavedItemsCount
    {
        get => _savedItemsCount;
        protected set => SetProperty(ref _savedItemsCount, value);
    }

    #endregion Properties

    #region Methods

    protected abstract void GetSkinsAsync();

    #endregion Methods
}
