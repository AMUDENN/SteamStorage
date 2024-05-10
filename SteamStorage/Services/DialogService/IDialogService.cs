using System.Threading.Tasks;
using SteamStorage.Utilities.Dialog;
using SteamStorage.ViewModels.Tools;
using DialogWindow = SteamStorage.Views.Windows.DialogWindow;

namespace SteamStorage.Services.DialogService;

public interface IDialogService
{
    public static DialogWindow? CurrentDialogWindow { get; protected set; }
    
    public Task<bool> ShowDialogAsync(ViewModelBase viewModel);
    
    public Task<bool> ShowDialogAsync(
        string message = "", 
        DialogUtility.MessageType messageType = DialogUtility.MessageType.Info,
        DialogUtility.MessageButtons messageButtons = DialogUtility.MessageButtons.Ok);

    public Task ShowMessageAsync(ViewModelBase viewModel);
    
    public Task ShowMessageAsync(
        string message = "", 
        DialogUtility.MessageType messageType = DialogUtility.MessageType.Info,
        DialogUtility.MessageButtons messageButtons = DialogUtility.MessageButtons.Ok);
}
