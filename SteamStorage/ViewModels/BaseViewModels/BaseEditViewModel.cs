using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.BaseModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels.BaseViewModels;

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

    public RelayCommand SaveCommand
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
