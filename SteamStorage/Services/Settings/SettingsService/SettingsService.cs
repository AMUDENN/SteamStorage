using System;
using System.ComponentModel;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Events;
using SteamStorage.Utilities.Events.Settings;
using SteamStorageAPI;
using SteamStorageAPI.Events;

namespace SteamStorage.Services.Settings.SettingsService;

public class SettingsService : ISettingsService, IDisposable
{
    #region Events

    public event ISettingsService.SettingsPropertyChangedEventHandler? SettingsPropertyChanged;

    #endregion Events

    #region Fields
    
    private readonly SettingsFile.SettingsFile _file;

    #endregion Fields

    #region Properties

    public UserSettings.UserSettings UserSettings { get; }

    #endregion Properties

    #region Constructor

    public SettingsService(
        string programName, 
        ApiClient apiClient, 
        IThemeService themeService)
    { 
        _file = new(programName);

        UserSettings = _file.Read<UserSettings.UserSettings>() ?? new();
        
        UserSettings.PropertyChanged += PropertyChangedHandler;
        
        apiClient.TokenChanged += TokenChangedHandler;
        themeService.ThemeChanged += ThemeChangedHandler;
        
        apiClient.Token = UserSettings.Token ?? string.Empty;
    }

    #endregion Constructor

    #region Destructor

    void IDisposable.Dispose()
    {
        SaveSettings();
    }

    #endregion Desctructor

    #region Methods

    private void PropertyChangedHandler(object? sender, PropertyChangedEventArgs args)
    {
        SaveSettings();
        OnSettingsPropertyChanged(args.PropertyName);
    }

    private void TokenChangedHandler(object? sender, TokenChangedEventArgs args)
    {
        UserSettings.Token = args.Token;
    }

    private void ThemeChangedHandler(object? sender, ThemeChangedEventArgs args)
    {
        UserSettings.Theme = args.NewTheme;
    }

    private void SaveSettings()
    {
        _file.Write(UserSettings);
    }

    private void OnSettingsPropertyChanged(string? property)
    {
        SettingsPropertyChanged?.Invoke(this, new(property));
    }

    #endregion Methods
}
