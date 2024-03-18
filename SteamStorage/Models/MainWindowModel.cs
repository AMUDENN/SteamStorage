using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.ViewModels;
using SteamStorage.Views;

namespace SteamStorage.Models;

public class MainWindowModel : ModelBase
{
    #region Fields

    private readonly MainWindow _mainWindow;

    #endregion Fields

    #region Properties

    public MainViewModel MainViewModel { get; }

    #endregion Properties

    #region Commands

    public RelayCommand MinimizeCommand { get; }

    public RelayCommand MaximizeCommand { get; }

    public RelayCommand RestoreCommand { get; }

    public RelayCommand CloseCommand { get; }

    #endregion Commands

    #region Constructor

    public MainWindowModel(
        MainWindow mainWindow,
        MainViewModel mainViewModel)
    {
        MainViewModel = mainViewModel;
        _mainWindow = mainWindow;

        MinimizeCommand = new(() => ChangeWindowState(WindowState.Minimized));
        MaximizeCommand = new(() => ChangeWindowState(WindowState.Maximized));
        RestoreCommand = new(() => ChangeWindowState(WindowState.Normal));
        CloseCommand = new(DoCloseCommand);
    }

    #endregion Constructor

    #region Methods

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
