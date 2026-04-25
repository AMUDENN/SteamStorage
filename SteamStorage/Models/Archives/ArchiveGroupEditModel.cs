using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media;
using SteamStorage.Models.Tools.BaseModels;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Services.DialogService;
using SteamStorage.Services.NotificationService;
using SteamStorage.Utilities.Dialog;
using SteamStorage.Utilities.Extensions;
using SteamStorageAPI.SDK.ApiClient;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities.ApiControllers;

namespace SteamStorage.Models.Archives;

public class ArchiveGroupEditModel : BaseGroupEditModel
{
    #region Fields

    private ArchiveGroupModel? _archiveGroupModel;

    private string _dateCreationString;
    private string _buySumString;
    private string _soldSumString;
    private string _countString;

    #endregion Fields

    #region Properties

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
        IApiClient apiClient,
        IDialogService dialogService,
        INotificationService notificationService) : base(apiClient, dialogService, notificationService)
    {
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

        bool result = await DialogService.ShowDialogAsync(
            $"Are you sure you want to delete the group: «{_archiveGroupModel.Title}»?",
            DialogUtility.MessageType.Question,
            DialogUtility.MessageButtons.OkCancel);

        if (!result) return;

        await ApiClient.DeleteAsync(
            ApiConstants.ApiMethods.DeleteArchiveGroup,
            new ArchiveGroups.DeleteArchiveGroupRequest(_archiveGroupModel.GroupId),
            cancellationToken);

        await NotificationService.ShowAsync("Delete group",
            $"You sent a request to delete the group: {_archiveGroupModel.Title}",
            cancellationToken: cancellationToken);

        OnItemDeleted();

        OnGoingBack();
    }

    protected override async Task DoSaveCommand(CancellationToken cancellationToken)
    {
        if (!(GroupTitle.Length.IsBetweenInclusive(3, 100)
              && Description?.Length <= 300
              && Colour != Colors.Transparent))
            return;

        if (IsNewGroup)
        {
            bool result = await DialogService.ShowDialogAsync(
                $"Are you sure you want to add the group: «{GroupTitle}»?",
                DialogUtility.MessageType.Question,
                DialogUtility.MessageButtons.SaveCancel);

            if (!result) return;

            await ApiClient.PostAsync(
                ApiConstants.ApiMethods.PostArchiveGroup,
                new ArchiveGroups.PostArchiveGroupRequest(GroupTitle,
                    Description,
                    Colour.ToHexColor()),
                cancellationToken);

            await NotificationService.ShowAsync("Add group",
                $"You sent a request to add the group: {GroupTitle}",
                cancellationToken: cancellationToken);
        }
        else if (_archiveGroupModel is not null)
        {
            bool result = await DialogService.ShowDialogAsync(
                $"Are you sure you want to edit the group: «{_archiveGroupModel.Title}»?",
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

            await NotificationService.ShowAsync("Edit group",
                $"You sent a request to edit the group: {_archiveGroupModel.Title}",
                cancellationToken: cancellationToken);
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
        return GroupTitle.Length.IsBetweenInclusive(3, 100)
               && Description?.Length <= 300
               && Colour != Colors.Transparent;
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