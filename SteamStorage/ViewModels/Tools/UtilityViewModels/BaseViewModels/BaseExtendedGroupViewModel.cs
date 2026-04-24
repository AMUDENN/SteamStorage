using System;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;

namespace SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;

public class BaseExtendedGroupViewModel : BaseGroupViewModel
{
    #region Fields

    private readonly ExtendedBaseGroupModel _model;

    #endregion Fields

    #region Properties

    public int Count => _model.Count;

    public DateTime DateCreation => _model.DateCreation;

    public string DateCreationString => _model.DateCreationString;

    #endregion Properties

    #region Constructor

    protected BaseExtendedGroupViewModel(
        ExtendedBaseGroupModel model) : base(model)
    {
        _model = model;
    }

    #endregion Constructor
}