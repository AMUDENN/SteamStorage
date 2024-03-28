using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Services.DialogService;

namespace SteamStorage.Models.BaseModels;

public class BaseDialogModel : ModelBase
{
    #region Commands

    public RelayCommand<object> SetDialogResultCommand { get; }

    #endregion Commands

    #region Constructor

    public BaseDialogModel()
    {
        SetDialogResultCommand = new(DoSetDialogResultCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoSetDialogResultCommand(object? dialogResult)
    {
        IDialogService.CurrentDialogWindow?.Close(dialogResult);
    }

    #endregion Methods
}
