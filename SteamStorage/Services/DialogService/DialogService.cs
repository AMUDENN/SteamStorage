using System.Threading.Tasks;
using SteamStorage.Models.BaseModels;
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

    public async Task<bool> ShowDialog(ViewModelBase viewModel)
    {
        _dialogWindowViewModel.SetContent(viewModel);
        DialogWindow dialogWindow = new()
        {
            DataContext = _dialogWindowViewModel
        };
        dialogWindow.InvalidateVisual();
        IDialogService.CurrentDialogWindow = dialogWindow;
        return await dialogWindow.ShowDialog<bool>(_mainWindow);
    }

    public async Task<bool> ShowDialog(string message = "",
        BaseDialogModel.MessageType messageType = BaseDialogModel.MessageType.Info,
        BaseDialogModel.MessageButtons messageButtons = BaseDialogModel.MessageButtons.Ok)
    {
        _messageDialogViewModel.SetMessageBox(message, messageType, messageButtons);
        return await ShowDialog(_messageDialogViewModel);
    }

    public async Task ShowMessage(ViewModelBase viewModel)
    {
        _dialogWindowViewModel.SetContent(viewModel);
        DialogWindow dialogWindow = new()
        {
            DataContext = _dialogWindowViewModel
        };

        IDialogService.CurrentDialogWindow = dialogWindow;
        await dialogWindow.ShowDialog(_mainWindow);
        dialogWindow.InvalidateVisual();
        dialogWindow.InvalidateArrange();
        dialogWindow.InvalidateMeasure();
        dialogWindow.Height += 1;
    }

    public async Task ShowMessage(string message = "",
        BaseDialogModel.MessageType messageType = BaseDialogModel.MessageType.Info,
        BaseDialogModel.MessageButtons messageButtons = BaseDialogModel.MessageButtons.Ok)
    {
        _messageDialogViewModel.SetMessageBox(message, messageType, messageButtons);
        await ShowMessage(_messageDialogViewModel);
    }

    #endregion Methods
}
