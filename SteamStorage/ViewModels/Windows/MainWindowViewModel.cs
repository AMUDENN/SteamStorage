using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Windows;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels.Windows;

public class MainWindowViewModel : ViewModelBase
{
    #region Fields

    private readonly MainWindowModel _model;

    #endregion Fields

    #region Properties

    public MainViewModel MainViewModel => _model.MainViewModel;

    #endregion Properties

    #region Commands

    public RelayCommand<KeyEventArgs> KeyDownCommand => _model.KeyDownCommand;

    public RelayCommand MinimizeCommand => _model.MinimizeCommand;

    public RelayCommand MaximizeCommand => _model.MaximizeCommand;

    public RelayCommand RestoreCommand => _model.RestoreCommand;

    public RelayCommand CloseCommand => _model.CloseCommand;

    #endregion Commands

    #region Constructor

    public MainWindowViewModel(
        MainWindowModel model)
    {
        _model = model;
        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}