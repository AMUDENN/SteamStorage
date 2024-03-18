using Avalonia.Styling;
using SteamStorage.ViewModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.Models.UtilityModels;

public class ThemeModel
{
    #region Properties

    public string Title { get; }

    public ThemeVariant ThemeVariant { get; }

    public ViewModelBase Page { get; }

    #endregion Properties

    #region Constructor

    public ThemeModel(
        string title,
        ThemeVariant themeVariant)
    {
        Title = title;
        ThemeVariant = themeVariant;
        Page = new ThemeViewModel(themeVariant);
    }

    #endregion Constructor
}
