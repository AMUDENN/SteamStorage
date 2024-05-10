using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.ViewModels;
using SteamStorage.Views.Windows;
using SteamStorageAPI.SDK.Services.ReferenceInformationService;

namespace SteamStorage.Models.Windows;

public class MainWindowModel : ModelBase
{
    #region Fields

    private readonly IReferenceInformationService _referenceInformationService;
    private readonly MainWindow _mainWindow;

    #endregion Fields

    #region Properties

    public MainViewModel MainViewModel { get; }

    #endregion Properties

    #region Commands

    public RelayCommand<KeyEventArgs> KeyDownCommand { get; }

    public RelayCommand MinimizeCommand { get; }

    public RelayCommand MaximizeCommand { get; }

    public RelayCommand RestoreCommand { get; }

    public RelayCommand CloseCommand { get; }

    #endregion Commands

    #region Constructor

    public MainWindowModel(
        IReferenceInformationService referenceInformationService,
        MainWindow mainWindow,
        MainViewModel mainViewModel)
    {
        _referenceInformationService = referenceInformationService;
        _mainWindow = mainWindow;
        MainViewModel = mainViewModel;

        KeyDownCommand = new(DoKeyDownCommand);
        MinimizeCommand = new(() => ChangeWindowState(WindowState.Minimized));
        MaximizeCommand = new(() => ChangeWindowState(WindowState.Maximized));
        RestoreCommand = new(() => ChangeWindowState(WindowState.Normal));
        CloseCommand = new(DoCloseCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoKeyDownCommand(KeyEventArgs? eventArgs)
    {
        if (eventArgs?.Key == Key.F1)
            _referenceInformationService.OpenReferenceInformation();
    }

    private void ChangeWindowState(WindowState windowState)
    {
        _mainWindow.WindowState = windowState;
    }

    private void DoCloseCommand()
    {
        _mainWindow.Close();
    }

    #endregion Methods
}
