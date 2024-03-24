using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;

namespace SteamStorage.Models.BaseModels;

public abstract class BaseEditModel : ModelBase
{
    #region Fields

    private string _title;

    #endregion Fields

    #region Properties

    public string Title
    {
        get => _title;
        protected set => SetProperty(ref _title, value);
    }

    #endregion Properties

    #region Commands

    public RelayCommand BackCommand { get; }

    public RelayCommand DeleteCommand { get; }

    public RelayCommand SaveCommand { get; }

    #endregion Commands

    #region Constructor

    protected BaseEditModel()
    {
        _title = string.Empty;

        BackCommand = new(DoBackCommand);
        DeleteCommand = new(DoDeleteCommand);
        SaveCommand = new(DoSaveCommand, CanExecuteSaveCommand);
    }

    #endregion Constructor

    #region Methods

    protected abstract void DoBackCommand();

    protected abstract void DoDeleteCommand();

    protected abstract void DoSaveCommand();

    protected abstract bool CanExecuteSaveCommand();

    #endregion Methods
}
