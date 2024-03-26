using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Utilities.Events.Edit;

namespace SteamStorage.Models.BaseModels;

public abstract class BaseEditModel : ModelBase
{
    #region Events

    public delegate void GoingBackEventHandler(object? sender, GoingBackEventArgs args);

    public event GoingBackEventHandler? GoingBack;

    #endregion Events

    #region Fields

    private string _title;

    private NavigationModel? _navigationModel;
    private SecondaryNavigationModel? _secondaryNavigationModel;

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

    public void SetSourceNavigationModel(
        NavigationModel navigationModel,
        SecondaryNavigationModel secondaryNavigationModel)
    {
        _navigationModel = navigationModel;
        _secondaryNavigationModel = secondaryNavigationModel;
    }

    protected virtual void DoBackCommand()
    {
        if (_navigationModel is not null && _secondaryNavigationModel is not null)
            OnGoingBack(_navigationModel, _secondaryNavigationModel);
    }

    protected abstract void DoDeleteCommand();

    protected abstract void DoSaveCommand();

    protected abstract bool CanExecuteSaveCommand();

    private void OnGoingBack(
        NavigationModel navigationModel,
        SecondaryNavigationModel secondaryNavigationModel)
    {
        GoingBack?.Invoke(this, new(navigationModel, secondaryNavigationModel));
    }

    #endregion Methods
}
