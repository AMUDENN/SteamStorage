using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools.BaseModels;

namespace SteamStorage.ViewModels.Tools.BaseViewModels;

public abstract class BaseEditViewModel : ViewModelBase
{
    #region Fields

    private readonly BaseEditModel _baseEditModel;

    #endregion Fields

    #region Properties

    public string Title
    {
        get => _baseEditModel.Title;
    }

    #endregion Properties

    #region Commands

    public RelayCommand BackCommand
    {
        get => _baseEditModel.BackCommand;
    }

    public AsyncRelayCommand DeleteCommand
    {
        get => _baseEditModel.DeleteCommand;
    }

    public AsyncRelayCommand SaveCommand
    {
        get => _baseEditModel.SaveCommand;
    }

    #endregion Commands

    #region Constructor

    protected BaseEditViewModel(
        BaseEditModel baseEditModel)
    {
        _baseEditModel = baseEditModel;
        baseEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
