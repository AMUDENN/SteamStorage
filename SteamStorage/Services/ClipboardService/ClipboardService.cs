using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using SteamStorage.Views.Windows;

namespace SteamStorage.Services.ClipboardService;

public class ClipboardService : IClipboardService
{
    #region Fields

    private readonly MainWindow _mainWindow;

    #endregion Fields

    #region Constructor

    public ClipboardService(
        MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }

    #endregion Constructor

    #region Methods

    public async Task SaveToClipboard(string text)
    {
        IClipboard? clipboard = TopLevel.GetTopLevel(_mainWindow)?.Clipboard;
        if (clipboard is null)
            return;
        await clipboard.SetTextAsync(text);
    }

    #endregion Methods
}
