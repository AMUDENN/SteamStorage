using SteamStorage.Models.BaseModels;

namespace SteamStorage.Models;

public class MessageDialogModel : BaseDialogModel
{
    #region Fields
    
    private string _message;
    private MessageType _selectedMessageType;
    private MessageButtons _selectedMessageButtons;

    #endregion Fields
    
    #region Properties

    public string Message
    {
        get => _message;
        private set => SetProperty(ref _message, value);
    }
    
    public MessageType SelectedMessageType
    {
        get => _selectedMessageType;
        private set => SetProperty(ref _selectedMessageType, value);
    }
    
    public MessageButtons SelectedMessageButtons
    {
        get => _selectedMessageButtons;
        private set
        {
            SetProperty(ref _selectedMessageButtons, value); 
            OnPropertyChanged(nameof(IsCancelVisible));
        }
    }

    public bool IsCancelVisible
    {
        get => _selectedMessageButtons == MessageButtons.OkCancel;
    }
    
    #endregion Properties
    
    #region Constructor

    public MessageDialogModel()
    {
        _message = string.Empty;
    }
    
    #endregion Constructor
    
    #region Methods
    
    public void SetMessageBox(string message, MessageType messageType, MessageButtons messageButtons)
    {
        Message = message;
        SelectedMessageType = messageType;
        SelectedMessageButtons = messageButtons;
    }
    
    #endregion Methods
}
