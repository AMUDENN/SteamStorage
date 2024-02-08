using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SteamStorage.Services.Settings.UserSettings;

public class UserSettings : ObservableObject
{
    #region Fields

    private string? _token;
    private ThemeVariant? _theme;

    #endregion Fields

    #region Properties

    public string? Token
    {
        get => _token;
        set => SetProperty(ref _token, value);
    }

    public ThemeVariant? Theme
    {
        get => _theme;
        set => SetProperty(ref _theme, value);
    }

    #endregion Properties
}
