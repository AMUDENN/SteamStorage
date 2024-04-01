using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.BaseModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels.BaseViewModels;

public abstract class BaseDialogViewModel : ViewModelBase
{
    #region Fields

    private readonly BaseDialogModel _baseDialogModel;
    
    #endregion Fields
    
    #region Commands

    public RelayCommand<object> SetDialogResultCommand
    {
        get => _baseDialogModel.SetDialogResultCommand;
    }
    
    public RelayCommand SetDialogResultTrueCommand
    {
        get => _baseDialogModel.SetDialogResultTrueCommand;
    }
    
    public RelayCommand SetDialogResultFalseCommand
    {
        get => _baseDialogModel.SetDialogResultFalseCommand;
    }
    
    #endregion Commands
    
    #region Constructor

    public BaseDialogViewModel(
        BaseDialogModel baseDialogModel)
    {
        _baseDialogModel = baseDialogModel;
        baseDialogModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }
    
    #endregion Constructor
}
