using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools.BaseModels;

namespace SteamStorage.ViewModels.Tools.BaseViewModels;

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

    protected BaseDialogViewModel(
        BaseDialogModel baseDialogModel)
    {
        _baseDialogModel = baseDialogModel;
        baseDialogModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }
    
    #endregion Constructor
}
