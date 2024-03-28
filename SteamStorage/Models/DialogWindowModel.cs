using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Services.DialogService;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.Models;

public class DialogWindowModel : ModelBase
{
    #region Fields

    private ViewModelBase? _content;

    #endregion Fields

    #region Commands

    public RelayCommand CloseCommand { get; }

    #endregion Commands

    #region Properties

    public ViewModelBase? Content
    {
        get => _content;
        private set => SetProperty(ref _content, value);
    }

    #endregion Properties

    #region Constructor

    public DialogWindowModel()
    {
        CloseCommand = new(DoCloseCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoCloseCommand()
    {
        IDialogService.CurrentDialogWindow?.Close(default);
    }

    public void SetContent(ViewModelBase content)
    {
        Content = content;
    }

    #endregion Methods
}
