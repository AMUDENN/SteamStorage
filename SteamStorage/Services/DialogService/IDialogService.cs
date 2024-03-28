using System.Threading.Tasks;
using SteamStorage.Models.BaseModels;
using SteamStorage.ViewModels.Tools;
using SteamStorage.Views;

namespace SteamStorage.Services.DialogService;

public interface IDialogService
{
    public static DialogWindow? CurrentDialogWindow { get; protected set; }
    
    public Task<bool> ShowDialogAsync(ViewModelBase viewModel);
    
    public Task<bool> ShowDialogAsync(
        string message = "", 
        BaseDialogModel.MessageType messageType = BaseDialogModel.MessageType.Info,
        BaseDialogModel.MessageButtons messageButtons = BaseDialogModel.MessageButtons.Ok);

    public Task ShowMessageAsync(ViewModelBase viewModel);
    
    public Task ShowMessageAsync(
        string message = "", 
        BaseDialogModel.MessageType messageType = BaseDialogModel.MessageType.Info,
        BaseDialogModel.MessageButtons messageButtons = BaseDialogModel.MessageButtons.Ok);
}
