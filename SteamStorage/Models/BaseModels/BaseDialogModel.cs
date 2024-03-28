using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Services.DialogService;

namespace SteamStorage.Models.BaseModels;

public abstract class BaseDialogModel : ModelBase
{
    #region Commands

    public RelayCommand SetDialogResultTrueCommand { get; }
    
    public RelayCommand SetDialogResultFalseCommand { get; }

    #endregion Commands

    #region Constructor

    public BaseDialogModel()
    {
        SetDialogResultTrueCommand = new(DoSetDialogResultTrueCommand, CanExecuteSetDialogResultTrueCommand);
        SetDialogResultFalseCommand = new(DoSetDialogResultFalseCommand, CanExecuteSetDialogResultFalseCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoSetDialogResultTrueCommand()
    {
        IDialogService.CurrentDialogWindow?.Close(default);
    }

    protected virtual bool CanExecuteSetDialogResultTrueCommand()
    {
        return true;
    }
    
    private void DoSetDialogResultFalseCommand()
    {
        IDialogService.CurrentDialogWindow?.Close(default);
    }

    protected virtual bool CanExecuteSetDialogResultFalseCommand()
    {
        return true;
    }

    #endregion Methods
}
