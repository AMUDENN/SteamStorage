using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Services.DialogService;
using SteamStorageAPI.SDK;

namespace SteamStorage.Models.BaseModels;

public abstract class BaseGroupEditModel : BaseEditModel
{
    #region Constants

    private const string CHANGE_TITLE = "Изменение группы";

    private const string ADD_TITLE = "Добавление группы";

    protected const string NO_DATA = "(нет данных)";

    #endregion Constants

    #region Constructor

    protected BaseGroupEditModel(
        ApiClient apiClient,
        IDialogService dialogService) : base(apiClient, dialogService)
    {
    }

    #endregion Constructor
    
    #region Methods

    protected void SetTitle(BaseGroupModel? model)
    {
        if (model is not null) Title = $"{CHANGE_TITLE}: «{model.Title}»";
        Title = ADD_TITLE;
    }

    #endregion Methods
}
