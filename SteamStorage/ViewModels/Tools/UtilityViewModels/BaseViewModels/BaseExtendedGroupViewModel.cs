using System;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;

namespace SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;

public class BaseExtendedGroupViewModel : ViewModelBase
{
    #region Fields

    private readonly ExtendedBaseGroupModel _model;

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

    public int Count
    {
        get => _model.Count;
    }
    
    public DateTime DateCreation
    {
        get => _model.DateCreation;
    }

    public string DateCreationString
    {
        get => _model.DateCreationString;
    }

    #endregion Properties

    #region Constructor

    protected BaseExtendedGroupViewModel(
        ExtendedBaseGroupModel model)
    {
        _model = model;
        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
