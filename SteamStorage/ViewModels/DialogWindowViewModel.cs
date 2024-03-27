using SteamStorage.Models;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class DialogWindowViewModel : ViewModelBase
{
    #region Fields

    private readonly DialogWindowModel _dialogWindowModel;

    #endregion Fields

    #region Properties

    public ViewModelBase? Content
    {
        get => _dialogWindowModel.Content;
    }

    #endregion Properties

    #region Constructor

    public DialogWindowViewModel(
        DialogWindowModel dialogWindowModel)
    {
        _dialogWindowModel = dialogWindowModel;
        dialogWindowModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void SetContent(ViewModelBase content)
    {
        _dialogWindowModel.SetContent(content);
    }

    #endregion Methods
}
