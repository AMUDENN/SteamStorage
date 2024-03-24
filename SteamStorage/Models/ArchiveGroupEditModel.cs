using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;

namespace SteamStorage.Models;

public class ArchiveGroupEditModel : BaseEditModel
{
    #region Constants

    private const string TITLE = "Изменение группы";

    #endregion Constants
    
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

    private void SetTitle(BaseGroupModel? model)
    {
        if (model is null) Title = TITLE;
        Title = $"{TITLE}: «{model?.Title}»";
    }

    private void SetValuesFromDefault()
    {
        
    }
    
    public void SetEditGroup(ArchiveGroupModel? model)
    {

    }

    #endregion Methods
}
