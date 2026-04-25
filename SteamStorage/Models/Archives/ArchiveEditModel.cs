using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SteamStorage.Models.Tools.BaseModels;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;
using SteamStorage.Services.DialogService;
using SteamStorage.Services.NotificationService;
using SteamStorage.Utilities.Dialog;
using SteamStorage.Utilities.Extensions;
using SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;
using SteamStorageAPI.SDK.ApiClient;
using SteamStorageAPI.SDK.Utilities.ApiControllers;

namespace SteamStorage.Models.Archives;

public class ArchiveEditModel : BaseItemEditModel
{
    #region Constants

    private const string CHANGE_TITLE = "Edit archive item";

    private const string ADD_TITLE = "Add archive item";

    #endregion Constants

    #region Fields

    private readonly ArchiveGroupsModel _archiveGroupsModel;

    private ArchiveModel? _archiveModel;

    private BaseGroupModel? _defaultArchiveGroupModel;
    private BaseGroupModel? _selectedArchiveGroupModel;

    private string _defaultCount;
    private string _count;

    private string _defaultBuyPrice;
    private string _buyPrice;

    private string _defaultSoldPrice;
    private string _soldPrice;

    private string? _defaultDescription;
    private string? _description;

    private DateTimeOffset _defaultBuyDate;
    private DateTimeOffset _buyDate;

    private DateTimeOffset _defaultSoldDate;
    private DateTimeOffset _soldDate;

    #endregion Fields

    #region Properties

    public BaseGroupModel? DefaultArchiveGroupModel
    {
        get => _defaultArchiveGroupModel;
        private set => SetProperty(ref _defaultArchiveGroupModel, value);
    }

    public BaseGroupModel? SelectedArchiveGroupModel
    {
        get => _selectedArchiveGroupModel;
        set
        {
            SetProperty(ref _selectedArchiveGroupModel, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string DefaultCount
    {
        get => _defaultCount;
        private set => SetProperty(ref _defaultCount, value);
    }

    public string Count
    {
        get => _count;
        set
        {
            SetProperty(ref _count, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string DefaultBuyPrice
    {
        get => _defaultBuyPrice;
        private set => SetProperty(ref _defaultBuyPrice, value);
    }

    public string BuyPrice
    {
        get => _buyPrice;
        set
        {
            SetProperty(ref _buyPrice, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string DefaultSoldPrice
    {
        get => _defaultSoldPrice;
        private set => SetProperty(ref _defaultSoldPrice, value);
    }

    public string SoldPrice
    {
        get => _soldPrice;
        set
        {
            SetProperty(ref _soldPrice, value);
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

    public DateTimeOffset DefaultBuyDate
    {
        get => _defaultBuyDate;
        private set => SetProperty(ref _defaultBuyDate, value);
    }

    public DateTimeOffset BuyDate
    {
        get => _buyDate;
        set => SetProperty(ref _buyDate, value);
    }

    public DateTimeOffset DefaultSoldDate
    {
        get => _defaultSoldDate;
        private set => SetProperty(ref _defaultSoldDate, value);
    }

    public DateTimeOffset SoldDate
    {
        get => _soldDate;
        set => SetProperty(ref _soldDate, value);
    }

    #endregion Properties

    #region Constructor

    public ArchiveEditModel(
        IApiClient apiClient,
        ArchiveGroupsModel archiveGroupsModel,
        IDialogService dialogService,
        INotificationService notificationService) : base(apiClient, dialogService, notificationService)
    {
        _archiveGroupsModel = archiveGroupsModel;

        _defaultCount = string.Empty;
        _count = string.Empty;

        _defaultBuyPrice = string.Empty;
        _buyPrice = string.Empty;

        _defaultSoldPrice = string.Empty;
        _soldPrice = string.Empty;
    }

    #endregion Constructor

    #region Methods

    protected override async Task DoDeleteCommand(CancellationToken cancellationToken)
    {
        if (_archiveModel is null) return;

        bool result = await DialogService.ShowDialogAsync(
            $"Are you sure you want to delete the archive item: «{_archiveModel.Title}»?",
            DialogUtility.MessageType.Question,
            DialogUtility.MessageButtons.OkCancel);

        if (!result) return;

        await ApiClient.DeleteAsync(
            ApiConstants.ApiMethods.DeleteArchive,
            new SteamStorageAPI.SDK.ApiEntities.Archives.DeleteArchiveRequest(_archiveModel.ArchiveId),
            cancellationToken);

        await NotificationService.ShowAsync("Delete archive item",
            $"You sent a request to delete the archive item: {_archiveModel.Title}",
            cancellationToken: cancellationToken);

        OnItemDeleted();

        OnGoingBack();
    }

    protected override async Task DoSaveCommand(CancellationToken cancellationToken)
    {
        if (SelectedSkinModel is null || SelectedArchiveGroupModel is null) return;

        if (!(Count.TryParse(out int count) && count > 0
                                            && BuyPrice.TryParse(out decimal buyPrice) && buyPrice.IsBetweenInclusive((decimal)0.01, 999999999999)
                                            && SoldPrice.TryParse(out decimal soldPrice) && soldPrice.IsBetweenInclusive((decimal)0.01, 999999999999)
                                            && Description?.Length <= 300))
            return;

        if (IsNewItem)
        {
            bool result = await DialogService.ShowDialogAsync(
                $"Are you sure you want to add the archive item: «{SelectedSkinModel.Title}»?",
                DialogUtility.MessageType.Question,
                DialogUtility.MessageButtons.SaveCancel);

            if (!result) return;

            await ApiClient.PostAsync(
                ApiConstants.ApiMethods.PostArchive,
                new SteamStorageAPI.SDK.ApiEntities.Archives.PostArchiveRequest(SelectedArchiveGroupModel.GroupId,
                    count,
                    buyPrice,
                    soldPrice,
                    SelectedSkinModel.SkinId,
                    Description,
                    BuyDate.DateTime,
                    SoldDate.DateTime),
                cancellationToken);

            await NotificationService.ShowAsync("Add archive item",
                $"You sent a request to add the archive item: {SelectedSkinModel.Title}",
                cancellationToken: cancellationToken);
        }
        else if (_archiveModel is not null)
        {
            bool result = await DialogService.ShowDialogAsync(
                $"Are you sure you want to edit the archive item: «{_archiveModel.Title}»?",
                DialogUtility.MessageType.Question,
                DialogUtility.MessageButtons.SaveCancel);

            if (!result) return;

            await ApiClient.PutAsync(
                ApiConstants.ApiMethods.PutArchive,
                new SteamStorageAPI.SDK.ApiEntities.Archives.PutArchiveRequest(_archiveModel.ArchiveId,
                    SelectedArchiveGroupModel.GroupId,
                    count,
                    buyPrice,
                    soldPrice,
                    SelectedSkinModel.SkinId,
                    Description,
                    BuyDate.DateTime,
                    SoldDate.DateTime),
                cancellationToken);

            await NotificationService.ShowAsync("Edit archive item",
                $"You sent a request to edit the archive item: {SelectedSkinModel.Title}",
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
        return SelectedArchiveGroupModel is not null
               && Count.TryParse(out int count) && count > 0
               && BuyPrice.TryParse(out decimal buyPrice) && buyPrice.IsBetweenInclusive((decimal)0.01, 999999999999)
               && SoldPrice.TryParse(out decimal soldPrice) && soldPrice.IsBetweenInclusive((decimal)0.01, 999999999999)
               && Description?.Length <= 300
               && SelectedSkinModel is not null;
    }

    protected override void SetTitle(BaseSkinViewModel? model)
    {
        Title = IsNewItem ? ADD_TITLE : $"{CHANGE_TITLE}: «{model?.Title}»";
    }

    private void SetValuesFromDefault()
    {
        SelectedArchiveGroupModel = DefaultArchiveGroupModel;
        Count = DefaultCount;
        BuyPrice = DefaultBuyPrice;
        SoldPrice = DefaultSoldPrice;
        SelectedSkinModel = DefaultSkinModel;
        Description = DefaultDescription;
        BuyDate = DefaultBuyDate;
        SoldDate = DefaultSoldDate;
    }

    public void SetEditArchive(ArchiveModel? model)
    {
        _archiveModel = model;

        DefaultArchiveGroupModel = _archiveGroupsModel.ArchiveGroupModels.FirstOrDefault(x => x.GroupId == model?.GroupId);

        DefaultCount = $"{model?.Count ?? 1:N0}";

        DefaultBuyPrice = $"{model?.BuyPrice ?? 1:N2}";

        DefaultSoldPrice = $"{model?.SoldPrice:N2}";

        DefaultSkinModel = model is not null ? new BaseSkinViewModel(model) : null;
        Filter = model?.Title;

        DefaultDescription = model?.Description ?? string.Empty;

        DefaultBuyDate = DateTime.SpecifyKind(model?.BuyDate ?? DateTime.Now, DateTimeKind.Local);

        DefaultSoldDate = DateTime.SpecifyKind(model?.SoldDate ?? DateTime.Now, DateTimeKind.Local);

        IsNewItem = model is null;

        SetValuesFromDefault();
    }

    public void SetAddArchive(ArchiveGroupModel? model)
    {
        _archiveModel = null;

        DefaultArchiveGroupModel = _archiveGroupsModel.ArchiveGroupModels.FirstOrDefault(x => x.GroupId == model?.GroupId);

        DefaultCount = "1";

        DefaultBuyPrice = "1";

        DefaultSoldPrice = "1";

        DefaultSkinModel = null;
        Filter = null;

        DefaultDescription = string.Empty;

        DefaultBuyDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        DefaultSoldDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        IsNewItem = true;

        SetValuesFromDefault();
    }

    public void SetAddArchive(ListItemModel? model)
    {
        _archiveModel = null;

        DefaultArchiveGroupModel = null;

        DefaultCount = "1";

        DefaultBuyPrice = $"{model?.CurrentPrice ?? 1:N2}";

        DefaultSoldPrice = $"{model?.CurrentPrice ?? 1:N2}";

        DefaultSkinModel = model is not null ? new BaseSkinViewModel(model) : null;
        Filter = model?.Title;

        DefaultDescription = string.Empty;

        DefaultBuyDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        DefaultSoldDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        IsNewItem = true;

        SetValuesFromDefault();
    }

    #endregion Methods
}