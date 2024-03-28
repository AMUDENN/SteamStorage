using SteamStorage.Models;
using SteamStorage.ViewModels.BaseViewModels;

namespace SteamStorage.ViewModels;

public class TextConfirmDialogViewModel : BaseDialogViewModel
{
    #region Fields

    private readonly TextConfirmDialogModel _textConfirmDialogModel;

    #endregion Fields

    #region Properties

    public string Message
    {
        get => _textConfirmDialogModel.Message;
    }

    public string ConfirmText
    {
        get => _textConfirmDialogModel.ConfirmText;
    }

    public string InputText
    {
        get => _textConfirmDialogModel.InputText;
        set => _textConfirmDialogModel.InputText = value;
    }
    
    #endregion Properties

    #region Constructor

    public TextConfirmDialogViewModel(
        TextConfirmDialogModel textConfirmDialogModel) : base(textConfirmDialogModel)
    {
        _textConfirmDialogModel = textConfirmDialogModel;
    }

    #endregion Constructor

    #region Methods

    public void SetConfirmData(string message, string confirmText)
    {
        _textConfirmDialogModel.SetConfirmData(message, confirmText);
    }
    
    #endregion Methods
}
