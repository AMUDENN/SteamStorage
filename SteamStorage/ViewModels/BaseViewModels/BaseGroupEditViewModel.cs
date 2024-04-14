using SteamStorage.Models.BaseModels;

namespace SteamStorage.ViewModels.BaseViewModels;

public abstract class BaseGroupEditViewModel : BaseEditViewModel
{
    #region Fields

    private readonly BaseGroupEditModel _baseGroupEditModel;

    #endregion Fields

    #region Constructor

    protected BaseGroupEditViewModel(
        BaseGroupEditModel baseGroupEditModel) : base(baseGroupEditModel)
    {
        _baseGroupEditModel = baseGroupEditModel;

        baseGroupEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
