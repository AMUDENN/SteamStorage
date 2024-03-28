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
            OnPropertyChanged(nameof(IsOkVisible));
            OnPropertyChanged(nameof(IsSaveVisible));
            OnPropertyChanged(nameof(IsCancelVisible));
        }
    }

    public bool IsOkVisible
    {
        get => _selectedMessageButtons is MessageButtons.OkCancel or MessageButtons.Ok;
    }
    
    public bool IsSaveVisible
    {
        get => _selectedMessageButtons is MessageButtons.SaveCancel or MessageButtons.Save;
    }
    
    public bool IsCancelVisible
    {
        get => _selectedMessageButtons is MessageButtons.OkCancel or MessageButtons.SaveCancel;
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
