using Avalonia.Media;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;
using SteamStorage.Services.DialogService;
using SteamStorageAPI.SDK;

namespace SteamStorage.Models.Tools.BaseModels;

public abstract class BaseGroupEditModel : BaseEditModel
{
    #region Constants

    private const string CHANGE_TITLE = "Изменение группы";

    private const string ADD_TITLE = "Добавление группы";

    protected const string NO_DATA = "(нет данных)";

    #endregion Constants

    #region Fields

    private string _defaultGroupTitle;
    private string _groupTitle;

    private string? _defaultDescription;
    private string? _description;

    private Color _defaultColour;
    private Color _colour;

    private bool _isNewGroup;

    #endregion Fields

    #region Properties

    public string DefaultGroupTitle
    {
        get => _defaultGroupTitle;
        protected set => SetProperty(ref _defaultGroupTitle, value);
    }

    public string GroupTitle
    {
        get => _groupTitle;
        set
        {
            SetProperty(ref _groupTitle, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string? DefaultDescription
    {
        get => _defaultDescription;
        protected set => SetProperty(ref _defaultDescription, value);
    }

    public string? Description
    {
        get => _description;
        set
        {
            SetProperty(ref _description, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public Color DefaultColour
    {
        get => _defaultColour;
        protected set => SetProperty(ref _defaultColour, value);
    }

    public Color Colour
    {
        get => _colour;
        set
        {
            SetProperty(ref _colour, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public bool IsNewGroup
    {
        get => _isNewGroup;
        protected set => SetProperty(ref _isNewGroup, value);
    }

    #endregion Properties

    #region Constructor

    protected BaseGroupEditModel(
        ApiClient apiClient,
        IDialogService dialogService) : base(apiClient, dialogService)
    {
        _defaultGroupTitle = string.Empty;
        _groupTitle = string.Empty;
    }

    #endregion Constructor

    #region Methods

    protected void SetTitle(BaseGroupModel? model)
    {
        Title = model is not null ? $"{CHANGE_TITLE}: «{model.Title}»" : ADD_TITLE;
    }

    #endregion Methods
}
