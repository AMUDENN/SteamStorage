using System;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels.UtilityViewModels;

public class BaseGroupViewModel : ViewModelBase
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

    public string DateCreationString
    {
        get => _model.DateCreationString;
    }

    #endregion Properties

    #region Constructor

    public BaseGroupViewModel(
        ExtendedBaseGroupModel model)
    {
        _model = model;

        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
