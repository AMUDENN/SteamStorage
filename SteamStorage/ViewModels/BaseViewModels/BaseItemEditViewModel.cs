using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels.BaseViewModels;

public abstract class BaseItemEditViewModel : ViewModelBase
{
    #region Fields

    private readonly BaseItemEditModel _baseItemEditModel;

    #endregion Fields

    #region Properties

    public string Title
    {
        get => _baseItemEditModel.Title;
    }

    public BaseGroupModel? DefaultGroupModel
    {
        get => _baseItemEditModel.DefaultGroupModel;
    }

    public BaseGroupModel? SelectedGroupModel
    {
        get => _baseItemEditModel.SelectedGroupModel;
        set => _baseItemEditModel.SelectedGroupModel = value;
    }

    #endregion Properties

    #region Commands

    public RelayCommand BackCommand
    {
        get => _baseItemEditModel.BackCommand;
    }

    public RelayCommand DeleteCommand
    {
        get => _baseItemEditModel.DeleteCommand;
    }

    public RelayCommand SaveCommand
    {
        get => _baseItemEditModel.SaveCommand;
    }

    #endregion Commands

    #region Constructor

    protected BaseItemEditViewModel(
        BaseItemEditModel baseItemEditModel)
    {
        _baseItemEditModel = baseItemEditModel;

        baseItemEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
