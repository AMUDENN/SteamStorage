using SteamStorage.Models.BaseModels;
using SteamStorage.Utilities.Dialog;

namespace SteamStorage.Models;

public class MessageDialogModel : BaseDialogModel
{
    #region Fields

    private string _message;
    private DialogUtility.MessageType _selectedMessageType;
    private DialogUtility.MessageButtons _selectedMessageButtons;

    #endregion Fields

    #region Properties

    public string Message
    {
        get => _message;
        private set => SetProperty(ref _message, value);
    }

    public DialogUtility.MessageType SelectedMessageType
    {
        get => _selectedMessageType;
        private set
        {
            SetProperty(ref _selectedMessageType, value); 
            OnPropertyChanged(nameof(IsInfoVisible));
            OnPropertyChanged(nameof(IsErrorVisible));
            OnPropertyChanged(nameof(IsQuestionVisible));
        }
    }

    public DialogUtility.MessageButtons SelectedMessageButtons
    {
        get => _selectedMessageButtons;
        private set
        {
            SetProperty(ref _selectedMessageButtons, value);
            OnPropertyChanged(nameof(IsOkVisible));
            OnPropertyChanged(nameof(IsSaveVisible));
            OnPropertyChanged(nameof(IsCancelVisible));
        }
    }

    public bool IsInfoVisible
    {
        get => _selectedMessageType == DialogUtility.MessageType.Info;
    }
    
    public bool IsErrorVisible
    {
        get => _selectedMessageType == DialogUtility.MessageType.Error;
    }
    
    public bool IsQuestionVisible
    {
        get => _selectedMessageType == DialogUtility.MessageType.Question;
    }

    public bool IsOkVisible
    {
        get => _selectedMessageButtons is DialogUtility.MessageButtons.OkCancel or DialogUtility.MessageButtons.Ok;
    }

    public bool IsSaveVisible
    {
        get => _selectedMessageButtons is DialogUtility.MessageButtons.SaveCancel or DialogUtility.MessageButtons.Save;
    }

    public bool IsCancelVisible
    {
        get => _selectedMessageButtons is DialogUtility.MessageButtons.OkCancel
            or DialogUtility.MessageButtons.SaveCancel;
    }

    #endregion Properties

    #region Constructor

    public MessageDialogModel()
    {
        _message = string.Empty;
    }

    #endregion Constructor

    #region Methods

    public void SetMessageBox(string message, 
        DialogUtility.MessageType messageType,
        DialogUtility.MessageButtons messageButtons)
    {
        Message = message;
        SelectedMessageType = messageType;
        SelectedMessageButtons = messageButtons;
    }

    #endregion Methods
}
