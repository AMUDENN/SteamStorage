using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;

namespace SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;

public class BaseSkinViewModel : ViewModelBase
{
    #region Fields

    private readonly BaseSkinModel _model;

    #endregion Fields

    #region Properties

    public int SkinId => _model.SkinId;

    public string ImageUrl => _model.ImageUrl;

    public string Title => _model.Title;

    #endregion Properties

    #region Commands

    public RelayCommand OpenInSteamCommand => _model.OpenInSteamCommand;

    #endregion Commands

    #region Constructor

    public BaseSkinViewModel(
        BaseSkinModel model)
    {
        _model = model;
        model.PropertyChanged += (_, e) => {
            if (e.PropertyName?.Contains("Checked") == true)
                System.Diagnostics.Debug.WriteLine($"Prop: {e.PropertyName}");
            OnPropertyChanged(e.PropertyName);
        };
    }

    #endregion Constructor

    #region Methods

    public override string ToString()
    {
        return Title;
    }

    #endregion Methods
}