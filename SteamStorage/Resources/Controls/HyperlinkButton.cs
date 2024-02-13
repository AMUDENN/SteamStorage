using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace SteamStorage.Resources.Controls;

public class HyperlinkButton : Button
{
    #region PropertiesDeclaration

    public static readonly StyledProperty<string> NavigateUrlProperty =
        AvaloniaProperty.Register<HyperlinkButton, string>(nameof(NavigateUrl));

    public static readonly StyledProperty<ICommand?> OpenUrlCommandProperty =
        AvaloniaProperty.Register<HyperlinkButton, ICommand?>(nameof(OpenUrlCommand));

    #endregion PropertiesDeclaration

    #region Properties

    public string NavigateUrl
    {
        get => GetValue(NavigateUrlProperty);
        set => SetValue(NavigateUrlProperty, value);
    }

    #endregion Properties

    #region Commands

    public ICommand? OpenUrlCommand
    {
        get => GetValue(OpenUrlCommandProperty);
        private set => SetValue(OpenUrlCommandProperty, value);
    }

    #endregion Commands

    #region Constructor

    public HyperlinkButton()
    {
        Initialized += InitializedHandler;
    }

    #endregion Constructor

    #region Methods

    private void InitializedHandler(object? sender, EventArgs args)
    {
        OpenUrlCommand = new RelayCommand(DoOpenUrlCommand);
    }

    private void DoOpenUrlCommand()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {NavigateUrl}"));
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", NavigateUrl);
        }
    }

    #endregion Methods
}
