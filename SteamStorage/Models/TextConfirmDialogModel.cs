using SteamStorage.Models.BaseModels;

namespace SteamStorage.Models;

public class TextConfirmDialogModel : BaseDialogModel
{
    #region Fields

    private string _message;
    private string _confirmText;

    private string _inputText;

    #endregion Fields

    #region Properties

    public string Message
    {
        get => _message;
        private set => SetProperty(ref _message, value);
    }

    public string ConfirmText
    {
        get => _confirmText;
        private set => SetProperty(ref _confirmText, value);
    }

    public string InputText
    {
        get => _inputText;
        set
        {
            SetProperty(ref _inputText, value); 
            SetDialogResultTrueCommand.NotifyCanExecuteChanged();
        }
    }

    #endregion Properties

    #region Constructor

    public TextConfirmDialogModel()
    {
        _message = string.Empty;

        _confirmText = string.Empty;
        _inputText = string.Empty;
    }

    #endregion Constructor

    #region Methods

    protected override bool CanExecuteSetDialogResultTrueCommand()
    {
        return InputText == ConfirmText;
    }

    public void SetConfirmData(string message, string confirmText)
    {
        Message = message;
        ConfirmText = confirmText;
        InputText = string.Empty;
    }

    #endregion Methods
}
