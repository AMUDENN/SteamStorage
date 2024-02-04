using Avalonia.Styling;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class ThemeViewModel : ViewModelBase
{
    #region Properties

    public ThemeVariant ThemeVariant { get; }

    #endregion Properties

    #region Constructor

    public ThemeViewModel(ThemeVariant themeVariant)
    {
        ThemeVariant = themeVariant;
    }

    #endregion Constructor
}