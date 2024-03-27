using System.Threading.Tasks;
using SteamStorage.Models.BaseModels;
using SteamStorage.ViewModels.Tools;
using SteamStorage.Views;

namespace SteamStorage.Services.DialogService;

public interface IDialogService
{
    public static DialogWindow? CurrentDialogWindow { get; protected set; }
    
    public Task<bool> ShowDialog(ViewModelBase viewModel);
    
    public Task<bool> ShowDialog(
        string message = "", 
        BaseDialogModel.MessageType messageType = BaseDialogModel.MessageType.Info,
        BaseDialogModel.MessageButtons messageButtons = BaseDialogModel.MessageButtons.Ok);

    public Task ShowMessage(ViewModelBase viewModel);
    
    public Task ShowMessage(
        string message = "", 
        BaseDialogModel.MessageType messageType = BaseDialogModel.MessageType.Info,
        BaseDialogModel.MessageButtons messageButtons = BaseDialogModel.MessageButtons.Ok);
}
