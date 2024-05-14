using SteamStorage.Models.Tools.UtilityModels.BaseModels;

namespace SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;

public class BaseGroupViewModel : ViewModelBase
{
    #region Fields

    private readonly BaseGroupModel _model;

    #endregion Fields

    #region Properties

    public string Colour
    {
        get => _model.Colour;
    }

    public string Title
    {
        get => _model.Title;
    }

    #endregion Properties

    #region Constructor

    protected BaseGroupViewModel(
        BaseGroupModel model)
    {
        _model = model;
        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
