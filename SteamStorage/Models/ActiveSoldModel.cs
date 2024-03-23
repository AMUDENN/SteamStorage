using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.UtilityViewModels.BaseViewModels;
using SteamStorageAPI;

namespace SteamStorage.Models;

public class ActiveSoldModel : BaseItemEditModel
{
    #region Constants

    private const string TITLE = "Продажа актива";

    #endregion Constants
    
    #region Fields

    

    #endregion Fields
    
    #region Properties
    
    
    
    #endregion Properties
    
    #region Constructor

    public ActiveSoldModel(
        ApiClient apiClient) : base(apiClient)
    {

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
        return true;
    }

    protected override void SetTitle(BaseSkinViewModel? model)
    {
        if (model is null) Title = TITLE;
        Title = $"{TITLE}: «{model?.Title}»";
    }

    private void SetValuesFromDefault()
    {
        
    }
    
    public void SetSoldActive(ActiveModel? model)
    {

    }

    #endregion Methods
}
