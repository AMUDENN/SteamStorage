using System.Threading.Tasks;
using SteamStorage.Utilities.Dialog;
using SteamStorage.ViewModels;
using SteamStorage.ViewModels.Tools;
using SteamStorage.Views;

namespace SteamStorage.Services.DialogService;

public class DialogService : IDialogService
{
    #region Fields

    private readonly MainWindow _mainWindow;
    private readonly DialogWindowViewModel _dialogWindowViewModel;
    private readonly MessageDialogViewModel _messageDialogViewModel;

    #endregion Fields

    #region Constructor

    public DialogService(
        MainWindow mainWindow,
        DialogWindowViewModel dialogWindowViewModel,
        MessageDialogViewModel messageDialogViewModel)
    {
        _mainWindow = mainWindow;
        _dialogWindowViewModel = dialogWindowViewModel;
        _messageDialogViewModel = messageDialogViewModel;
    }

    #endregion Constructor

    #region Methods

    public async Task<bool> ShowDialogAsync(ViewModelBase viewModel)
    {
        _dialogWindowViewModel.SetContent(viewModel);
        DialogWindow dialogWindow = new()
        {
            DataContext = _dialogWindowViewModel
        };
        IDialogService.CurrentDialogWindow = dialogWindow;
        return await dialogWindow.ShowDialog<bool>(_mainWindow);
    }

    public async Task<bool> ShowDialogAsync(string message = "",
        DialogUtility.MessageType messageType = DialogUtility.MessageType.Info,
        DialogUtility.MessageButtons messageButtons = DialogUtility.MessageButtons.Ok)
    {
        _messageDialogViewModel.SetMessageBox(message, messageType, messageButtons);
        return await ShowDialogAsync(_messageDialogViewModel);
    }

    public async Task ShowMessageAsync(ViewModelBase viewModel)
    {
        _dialogWindowViewModel.SetContent(viewModel);
        DialogWindow dialogWindow = new()
        {
            DataContext = _dialogWindowViewModel
        };

        IDialogService.CurrentDialogWindow = dialogWindow;
        await dialogWindow.ShowDialog(_mainWindow);
    }

    public async Task ShowMessageAsync(string message = "",
        DialogUtility.MessageType messageType = DialogUtility.MessageType.Info,
        DialogUtility.MessageButtons messageButtons = DialogUtility.MessageButtons.Ok)
    {
        _messageDialogViewModel.SetMessageBox(message, messageType, messageButtons);
        await ShowMessageAsync(_messageDialogViewModel);
    }

    #endregion Methods
}
