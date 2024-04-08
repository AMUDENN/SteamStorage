using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media;
using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Services.DialogService;
using SteamStorage.Utilities.Dialog;
using SteamStorage.Utilities.Extensions;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ArchiveGroupEditModel : BaseEditModel
{
    #region Constants

    private const string TITLE = "Изменение группы";

    private const string NO_DATA = "(нет данных)";

    #endregion Constants

    #region Fields

    private readonly IDialogService _dialogService;
    
    private ArchiveGroupModel? _archiveGroupModel;
    
    private string _defaultGroupTitle;
    private string _groupTitle;

    private string? _defaultDescription;
    private string? _description;

    private Color _defaultColour;
    private Color _colour;

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

    public Color DefaultColour
    {
        get => _defaultColour;
        private set => SetProperty(ref _defaultColour, value);
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

    public ArchiveGroupEditModel(
        ApiClient apiClient,
        IDialogService dialogService) : base(apiClient)
    {
        _dialogService = dialogService;
        
        _defaultGroupTitle = string.Empty;
        _groupTitle = string.Empty;

        _dateCreationString = string.Empty;
        _buySumString = string.Empty;
        _soldSumString = string.Empty;
        _countString = string.Empty;
    }

    #endregion Constructor

    #region Methods

    protected override async Task DoDeleteCommand(CancellationToken cancellationToken)
    {
        if (_archiveGroupModel is null) return;
        
        bool result = await _dialogService.ShowDialogAsync(
            $"Вы уверены, что хотите удалить группу: «{_archiveGroupModel.Title}»?",
            DialogUtility.MessageType.Question,
            DialogUtility.MessageButtons.OkCancel);
        
        if (!result) return;

        await ApiClient.DeleteAsync(
            ApiConstants.ApiMethods.DeleteArchiveGroup,
            new ArchiveGroups.DeleteArchiveGroupRequest(_archiveGroupModel.GroupId),
            cancellationToken);
        
        OnItemDeleted();
        
        OnGoingBack();
    }

    protected override async Task DoSaveCommand(CancellationToken cancellationToken)
    {
        if (!(GroupTitle.Length is >= 3 and <= 100
              && Description?.Length <= 300
              && Colour != Colors.Transparent))
            return;

        if (IsNewGroup)
        {
            bool result = await _dialogService.ShowDialogAsync(
                $"Вы уверены, что хотите добавить группу: «{GroupTitle}»?",
                DialogUtility.MessageType.Question,
                DialogUtility.MessageButtons.SaveCancel);

            if (!result) return;

            await ApiClient.PostAsync(
                ApiConstants.ApiMethods.PostArchiveGroup,
                new ArchiveGroups.PostArchiveGroupRequest(GroupTitle,
                    Description,
                    Colour.ToHexColor()),
                cancellationToken);
        }
        else if (_archiveGroupModel is not null)
        {
            bool result = await _dialogService.ShowDialogAsync(
                $"Вы уверены, что хотите изменить группу: «{_archiveGroupModel.Title}»?",
                DialogUtility.MessageType.Question,
                DialogUtility.MessageButtons.SaveCancel);

            if (!result) return;

            await ApiClient.PutAsync(
                ApiConstants.ApiMethods.PutArchiveGroup,
                new ArchiveGroups.PutArchiveGroupRequest(_archiveGroupModel.GroupId,
                    GroupTitle,
                    Description,
                    Colour.ToHexColor()),
                cancellationToken);
        }
        else
        {
            return;
        }

        OnItemChanged();

        OnGoingBack();
    }

    protected override bool CanExecuteSaveCommand()
    {
        return GroupTitle.Length is >= 3 and <= 100
               && Description?.Length <= 300
               && Colour != Colors.Transparent;
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
        _archiveGroupModel = model;
        
        DefaultGroupTitle = model?.Title ?? string.Empty;

        DefaultDescription = model?.Description ?? string.Empty;

        bool isColor = Color.TryParse(model?.Colour, out Color color);
        DefaultColour = isColor ? color : Colors.White;

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
