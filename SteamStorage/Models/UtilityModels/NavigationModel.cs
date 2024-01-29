using Avalonia.Controls;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.Models.UtilityModels;

public class NavigationModel
{
    #region Properties

    public Image Image { get; }
    public string Title { get; }
    public ViewModelBase Page { get; }

    #endregion Properties

    #region Constructor

    public NavigationModel(string image, string title, ViewModelBase page)
    {
        Image newImage = new();
        newImage.Classes.Add(image);
        Image = newImage;
        Title = title;
        Page = page;
    }

    #endregion Constructor
}
