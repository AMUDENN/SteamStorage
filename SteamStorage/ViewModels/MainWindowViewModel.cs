using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Views;

namespace SteamStorage.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private readonly MainWindow _mainWindow;
        private readonly MainViewModel _mainVM;

        private readonly RelayCommand _minimizeCommand;
        private readonly RelayCommand _maximizeCommand;
        private readonly RelayCommand _restoreCommand;
        private readonly RelayCommand _closeCommand;
        #endregion Fields

        #region Properties
        public MainViewModel MainVM => _mainVM;
        #endregion Properties

        #region Commands
        public RelayCommand MinimizeCommand => _minimizeCommand;
        public RelayCommand MaximizeCommand => _maximizeCommand;
        public RelayCommand RestoreCommand => _restoreCommand;
        public RelayCommand CloseCommand => _closeCommand;
        #endregion Commands

        #region Constructor
        public MainWindowViewModel(MainWindow mainWindow, MainViewModel mainViewModel)
        {
            _mainVM = mainViewModel;
            _mainWindow = mainWindow;

            _minimizeCommand = new RelayCommand(() => ChangeWindowState(WindowState.Minimized));
            _maximizeCommand = new RelayCommand(() => ChangeWindowState(WindowState.Maximized));
            _restoreCommand = new RelayCommand(() => ChangeWindowState(WindowState.Normal));
            _closeCommand = new RelayCommand(DoCloseCommand);
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
