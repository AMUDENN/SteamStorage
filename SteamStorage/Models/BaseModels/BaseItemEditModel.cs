using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels.BaseModels;

namespace SteamStorage.Models.BaseModels;

public abstract class BaseItemEditModel : ModelBase
{
    #region Fields

    private string _title;

    private BaseGroupModel? _defaultGroupModel;
    private BaseGroupModel? _selectedGroupModel;

    #endregion Fields

    #region Properties

    public string Title
    {
        get => _title;
        protected set => SetProperty(ref _title, value);
    }

    public BaseGroupModel? DefaultGroupModel
    {
        get => _defaultGroupModel;
        protected set => SetProperty(ref _defaultGroupModel, value);
    }

    public BaseGroupModel? SelectedGroupModel
    {
        get => _selectedGroupModel;
        set
        {
            SetProperty(ref _selectedGroupModel, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    #endregion Properties

    #region Commands

    public RelayCommand BackCommand { get; }

    public RelayCommand DeleteCommand { get; }

    public RelayCommand SaveCommand { get; }

    #endregion Commands

    #region Constructor

    protected BaseItemEditModel()
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
