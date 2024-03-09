using SteamStorage.ViewModels.Tools;

namespace SteamStorage.Models.UtilityModels;

public class SecondaryNavigationModel
{
    #region Properties

    public string Title { get; }

    public ViewModelBase Page { get; }

    public bool IsEnabled { get; }

    #endregion Properties

    #region Constructor

    public SecondaryNavigationModel(
        string title,
        ViewModelBase page,
        bool isEnabled)
    {
        Title = title;
        Page = page;
        IsEnabled = isEnabled;
    }

    #endregion Constructor
}