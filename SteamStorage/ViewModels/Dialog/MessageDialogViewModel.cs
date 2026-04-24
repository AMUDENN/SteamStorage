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

    public string Message => _messageDialogModel.Message;

    public DialogUtility.MessageType SelectedMessageType => _messageDialogModel.SelectedMessageType;

    public DialogUtility.MessageButtons SelectedMessageButtons => _messageDialogModel.SelectedMessageButtons;

    public bool IsInfoVisible => _messageDialogModel.IsInfoVisible;

    public bool IsErrorVisible => _messageDialogModel.IsErrorVisible;

    public bool IsQuestionVisible => _messageDialogModel.IsQuestionVisible;

    public bool IsOkVisible => _messageDialogModel.IsOkVisible;

    public bool IsSaveVisible => _messageDialogModel.IsSaveVisible;

    public bool IsCancelVisible => _messageDialogModel.IsCancelVisible;

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