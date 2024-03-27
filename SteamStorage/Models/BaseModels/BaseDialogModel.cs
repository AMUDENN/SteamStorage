using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Services.DialogService;

namespace SteamStorage.Models.BaseModels;

public class BaseDialogModel : ModelBase
{
    #region Enums

    public enum MessageType
    {
        Info,
        Error,
        Warning,
        Question
    }

    public enum MessageButtons
    {
        Ok,
        OkCancel
    }
    
    #endregion Enums
    
    #region Commands
    
    public RelayCommand ReturnTrueCommand { get; }
    
    public RelayCommand ReturnFalseCommand { get; }
    
    #endregion Commands
    
    #region Constructor

    public BaseDialogModel()
    {
        
        ReturnTrueCommand = new(DoReturnTrueCommand);
        ReturnFalseCommand = new(DoReturnFalseCommand);
    }
    
    #endregion Constructor
    
    #region Methods

    private void DoReturnTrueCommand()
    {
        IDialogService.CurrentDialogWindow?.Close(true);
    }
    
    private void DoReturnFalseCommand()
    {
        IDialogService.CurrentDialogWindow?.Close(false);
    }
    
    #endregion Methods
}
