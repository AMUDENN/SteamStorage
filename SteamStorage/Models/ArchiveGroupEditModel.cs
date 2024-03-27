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

    private const string NO_DATA = "(нет данных)";

    #endregion Constants

    #region Fields

    private string _defaultGroupTitle;
    private string _groupTitle;

    private string? _defaultDescription;
    private string? _description;

    private string? _defaultColour;
    private string? _colour;

    private bool _isNewGroup;

    private string _dateCreationString;
    private string _buySumString;
    private string _soldSumString;
    private string _countString;

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

    public string DateCreationString
    {
        get => _dateCreationString;
        private set => SetProperty(ref _dateCreationString, value);
    }

    public string BuySumString
    {
        get => _buySumString;
        private set => SetProperty(ref _buySumString, value);
    }

    public string SoldSumString
    {
        get => _soldSumString;
        private set => SetProperty(ref _soldSumString, value);
    }

    public string CountString
    {
        get => _countString;
        private set => SetProperty(ref _countString, value);
    }

    #endregion Properties

    #region Constructor

    public ArchiveGroupEditModel()
    {
        _defaultGroupTitle = string.Empty;
        _groupTitle = string.Empty;

        _dateCreationString = string.Empty;
        _buySumString = string.Empty;
        _soldSumString = string.Empty;
        _countString = string.Empty;
    }

    #endregion Constructor

    #region Methods

    protected override void DoDeleteCommand()
    {
        //TODO:
    }

    protected override void DoSaveCommand()
    {
        //TODO:
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

        DefaultDescription = model?.Description ?? string.Empty;

        DefaultColour = model?.Colour ?? string.Empty;

        DateCreationString = model?.DateCreationString ?? NO_DATA;
        BuySumString = model?.BuySumString ?? NO_DATA;
        SoldSumString = model?.SoldSumString ?? NO_DATA;
        CountString = model?.Count is null ? NO_DATA : $"{model.Count:N0}";

        IsNewGroup = model is null;

        SetTitle(model);

        SetValuesFromDefault();
    }

    #endregion Methods
}
