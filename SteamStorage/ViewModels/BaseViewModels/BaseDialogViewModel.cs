using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.BaseModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels.BaseViewModels;

public class BaseDialogViewModel : ViewModelBase
{
    #region Fields

    private readonly BaseDialogModel _baseDialogModel;
    
    #endregion Fields
    
    #region Commands

    public RelayCommand ReturnTrueCommand
    {
        get => _baseDialogModel.ReturnTrueCommand;
    }

    public RelayCommand ReturnFalseCommand
    {
        get => _baseDialogModel.ReturnFalseCommand;
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
