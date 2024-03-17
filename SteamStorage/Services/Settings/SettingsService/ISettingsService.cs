using SteamStorage.Utilities.Events;
using SteamStorage.Utilities.Events.Settings;

namespace SteamStorage.Services.Settings.SettingsService;

public interface ISettingsService
{
    public delegate void SettingsPropertyChangedEventHandler(object? sender, SettingsPropertyChangedEventArgs args);
    
    public event SettingsPropertyChangedEventHandler? SettingsPropertyChanged;
    
    public UserSettings.UserSettings UserSettings { get; }
}