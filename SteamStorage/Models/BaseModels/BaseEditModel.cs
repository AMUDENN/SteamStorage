using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorageAPI;

namespace SteamStorage.Models.BaseModels;

public abstract class BaseEditModel : ModelBase
{
    #region Events

    public delegate void GoingBackEventHandler(object? sender);

    public event GoingBackEventHandler? GoingBack;

    #endregion Events

    #region Fields

    protected readonly ApiClient _apiClient;
    
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

    public AsyncRelayCommand DeleteCommand { get; }

    public RelayCommand SaveCommand { get; }

    #endregion Commands

    #region Constructor

    protected BaseEditModel(
        ApiClient apiClient)
    {
        _apiClient = apiClient;
        
        _title = string.Empty;
        
        BackCommand = new(DoBackCommand);
        DeleteCommand = new(DoDeleteCommand);
        SaveCommand = new(DoSaveCommand, CanExecuteSaveCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoBackCommand()
    {
        OnGoingBack();
    }

    protected abstract Task DoDeleteCommand();

    protected abstract void DoSaveCommand();

    protected abstract bool CanExecuteSaveCommand();

    protected void OnGoingBack()
    {
        GoingBack?.Invoke(this);
    }

    #endregion Methods
}
