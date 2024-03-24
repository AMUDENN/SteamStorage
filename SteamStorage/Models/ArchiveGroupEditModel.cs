using System.Text.RegularExpressions;
using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Utilities;

namespace SteamStorage.Models;

public class ArchiveGroupEditModel : BaseEditModel
{
    #region Constants

    private const string TITLE = "Изменение группы";

    #endregion Constants

    #region Fields

    private string _defaultGroupTitle;
    private string _groupTitle;

    private string? _defaultDescription;
    private string? _description;

    private string? _defaultColour;
    private string? _colour;

    private bool _isNewGroup;

    private ArchiveGroupModel? _archiveGroupModel;

    #endregion Fields

    #region Properties

    public string DefaultGroupTitle
    {
        get => _defaultGroupTitle;
        private set => SetProperty(ref _defaultGroupTitle, value);
    }

    public string GroupTitle
    {
        get => _groupTitle;
        set
        {
            SetProperty(ref _groupTitle, value);
            SaveCommand.NotifyCanExecuteChanged();
            SetTitle(_archiveGroupModel);
        }
    }

    public string? DefaultDescription
    {
        get => _defaultDescription;
        private set => SetProperty(ref _defaultDescription, value);
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

    public string? DefaultColour
    {
        get => _defaultColour;
        private set => SetProperty(ref _defaultColour, value);
    }

    public string? Colour
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
        private set => SetProperty(ref _isNewGroup, value);
    }

    public string? DateCreationString
    {
        get => _archiveGroupModel?.DateCreationString;
    }

    public string? BuySumString
    {
        get => _archiveGroupModel?.BuySumString;
    }

    public string? SoldSumString
    {
        get => _archiveGroupModel?.SoldSumString;
    }

    public int? Count
    {
        get => _archiveGroupModel?.Count;
    }

    #endregion Properties

    #region Constructor

    public ArchiveGroupEditModel()
    {
        _defaultGroupTitle = string.Empty;
        _groupTitle = string.Empty;
    }

    #endregion Constructor

    #region Methods

    protected override void DoBackCommand()
    {

    }

    protected override void DoDeleteCommand()
    {

    }

    protected override void DoSaveCommand()
    {

    }

    protected override bool CanExecuteSaveCommand()
    {
        return Title.Length is >= 3 and <= 100
               && Description?.Length <= 300
               && (string.IsNullOrEmpty(Colour) || Regex.IsMatch(Colour, ProgramConstants.COLOUR_PATTERN));
    }

    private void SetTitle(BaseGroupModel? model)
    {
        if (model is null) Title = TITLE;
        Title = $"{TITLE}: «{model?.Title}»";
    }

    private void SetValuesFromDefault()
    {
        GroupTitle = DefaultGroupTitle;
        Description = DefaultDescription;
        Colour = DefaultColour;
    }

    public void SetEditGroup(ArchiveGroupModel? model)
    {
        DefaultGroupTitle = model?.Title ?? string.Empty;

        DefaultDescription = model?.Description;

        DefaultColour = model?.Colour;

        _archiveGroupModel = model;

        if (model is null)
            IsNewGroup = true;
        else
        {
            model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
            IsNewGroup = false;
        }

        SetTitle(model);

        SetValuesFromDefault();
    }

    #endregion Methods
}
