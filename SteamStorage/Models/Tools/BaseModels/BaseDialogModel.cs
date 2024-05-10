using CommunityToolkit.Mvvm.Input;
using SteamStorage.Services.DialogService;

namespace SteamStorage.Models.Tools.BaseModels;

public abstract class BaseDialogModel : ModelBase
{
    #region Commands

    public RelayCommand<object> SetDialogResultCommand { get; }
    
    public RelayCommand SetDialogResultTrueCommand { get; }
    
    public RelayCommand SetDialogResultFalseCommand { get; }

    #endregion Commands

    #region Constructor

    protected BaseDialogModel()
    {
        SetDialogResultCommand = new(DoSetDialogResultCommand, CanExecuteSetDialogResultCommand);
        SetDialogResultTrueCommand = new(DoSetDialogResultTrueCommand, CanExecuteSetDialogResultTrueCommand);
        SetDialogResultFalseCommand = new(DoSetDialogResultFalseCommand, CanExecuteSetDialogResultFalseCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoSetDialogResultCommand(object? obj)
    {
        IDialogService.CurrentDialogWindow?.Close(obj);
    }

    protected virtual bool CanExecuteSetDialogResultCommand(object? obj)
    {
        return true;
    }
    
    private void DoSetDialogResultTrueCommand()
    {
        IDialogService.CurrentDialogWindow?.Close(true);
    }

    protected virtual bool CanExecuteSetDialogResultTrueCommand()
    {
        return true;
    }
    
    private void DoSetDialogResultFalseCommand()
    {
        IDialogService.CurrentDialogWindow?.Close(false);
    }

    protected virtual bool CanExecuteSetDialogResultFalseCommand()
    {
        return true;
    }

    #endregion Methods
}
