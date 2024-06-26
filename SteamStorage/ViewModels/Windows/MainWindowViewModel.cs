﻿using Avalonia.Input;
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

    public MainViewModel MainViewModel
    {
        get => _model.MainViewModel;
    }

    #endregion Properties

    #region Commands

    public RelayCommand<KeyEventArgs> KeyDownCommand
    {
        get => _model.KeyDownCommand;
    }
    
    public RelayCommand MinimizeCommand
    {
        get => _model.MinimizeCommand;
    }

    public RelayCommand MaximizeCommand
    {
        get => _model.MaximizeCommand;
    }

    public RelayCommand RestoreCommand
    {
        get => _model.RestoreCommand;
    }

    public RelayCommand CloseCommand
    {
        get => _model.CloseCommand;
    }

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
