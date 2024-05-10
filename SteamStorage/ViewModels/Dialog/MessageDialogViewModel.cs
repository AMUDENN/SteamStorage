using SteamStorage.Models.Dialog;
using SteamStorage.Utilities.Dialog;
using SteamStorage.ViewModels.Tools.BaseViewModels;

namespace SteamStorage.ViewModels.Dialog;

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

    public DialogUtility.MessageType SelectedMessageType
    {
        get => _messageDialogModel.SelectedMessageType;
    }

    public DialogUtility.MessageButtons SelectedMessageButtons
    {
        get => _messageDialogModel.SelectedMessageButtons;
    }
    
    public bool IsInfoVisible
    {
        get => _messageDialogModel.IsInfoVisible;
    }
    
    public bool IsErrorVisible
    {
        get => _messageDialogModel.IsErrorVisible;
    }
    
    public bool IsQuestionVisible
    {
        get => _messageDialogModel.IsQuestionVisible;
    }
    
    public bool IsOkVisible
    {
        get => _messageDialogModel.IsOkVisible;
    }
    
    public bool IsSaveVisible
    {
        get => _messageDialogModel.IsSaveVisible;
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
    }

    #endregion Constructor

    #region Methods

    public void SetMessageBox(string message,
        DialogUtility.MessageType messageType,
        DialogUtility.MessageButtons messageButtons)
    {
        _messageDialogModel.SetMessageBox(message, messageType, messageButtons);
    }

    #endregion Methods
}
