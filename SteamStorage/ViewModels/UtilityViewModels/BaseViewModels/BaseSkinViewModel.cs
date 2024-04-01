using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels.UtilityViewModels.BaseViewModels;

public class BaseSkinViewModel : ViewModelBase
{
    #region Fields

    private readonly BaseSkinModel _model;

    #endregion Fields

    #region Properties

    public int SkinId
    {
        get => _model.SkinId;
    }
    
    public string ImageUrl
    {
        get => _model.ImageUrl;
    }

    public string Title
    {
        get => _model.Title;
    }

    #endregion Properties

    #region Commands

    public RelayCommand OpenInSteamCommand
    {
        get => _model.OpenInSteamCommand;
    }

    #endregion Commands

    #region Constructor

    public BaseSkinViewModel(
        BaseSkinModel model)
    {
        _model = model;

        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public override string ToString()
    {
        return Title;
    }

    #endregion Methods
}
