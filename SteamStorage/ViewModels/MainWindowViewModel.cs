using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Views;

namespace SteamStorage.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly MainWindow _mainWindow;
        private readonly MainViewModel _mainViewModel;

        private readonly RelayCommand _minimizeCommand;
        private readonly RelayCommand _maximizeCommand;
        private readonly RelayCommand _restoreCommand;
        private readonly RelayCommand _closeCommand;

        #endregion Fields

        #region Properties

        public MainViewModel MainViewModel
        {
            get => _mainViewModel;
        }

        #endregion Properties

        #region Commands

        public RelayCommand MinimizeCommand
        {
            get => _minimizeCommand;
        }

        public RelayCommand MaximizeCommand
        {
            get => _maximizeCommand;
        }

        public RelayCommand RestoreCommand
        {
            get => _restoreCommand;
        }

        public RelayCommand CloseCommand
        {
            get => _closeCommand;
        }

        #endregion Commands

        #region Constructor

        public MainWindowViewModel(MainWindow mainWindow, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _mainWindow = mainWindow;

            _minimizeCommand = new(() => ChangeWindowState(WindowState.Minimized));
            _maximizeCommand = new(() => ChangeWindowState(WindowState.Maximized));
            _restoreCommand = new(() => ChangeWindowState(WindowState.Normal));
            _closeCommand = new(DoCloseCommand);
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
}
