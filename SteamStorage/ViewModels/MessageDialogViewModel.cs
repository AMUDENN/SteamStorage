using SteamStorage.Models;
using SteamStorage.Models.BaseModels;
using SteamStorage.ViewModels.BaseViewModels;

namespace SteamStorage.ViewModels;

public class MessageDialogViewModel : BaseDialogViewModel
{
    #region Fields

    private readonly MessageDialogModel _messageDialogModel;

    #endregion Fields

    #region Properties

    public string Message
    {
        get => _messageDialogModel.Message;
    }

    public BaseDialogModel.MessageType SelectedMessageType
    {
        get => _messageDialogModel.SelectedMessageType;
    }

    public BaseDialogModel.MessageButtons SelectedMessageButtons
    {
        get => _messageDialogModel.SelectedMessageButtons;
    }
    
    public bool IsCancelVisible
    {
        get => _messageDialogModel.IsCancelVisible;
    }

    #endregion Properties

    #region Constructor

    public MessageDialogViewModel(
        MessageDialogModel messageDialogModel) : base(messageDialogModel)
    {
        _messageDialogModel = messageDialogModel;
        messageDialogModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void SetMessageBox(string message,
        BaseDialogModel.MessageType messageType,
        BaseDialogModel.MessageButtons messageButtons)
    {
        _messageDialogModel.SetMessageBox(message, messageType, messageButtons);
    }

    #endregion Methods
}
