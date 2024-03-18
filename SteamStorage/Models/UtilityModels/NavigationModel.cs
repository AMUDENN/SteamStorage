using SteamStorage.Resources.Controls;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.Models.UtilityModels;

public class NavigationModel
{
    #region Constants

    private const string FOREGROUND_VECTOR_IMAGE_CLASS = "ForegroundVectorImage";

    #endregion Constants

    #region Properties

    public VectorImage VectorImage { get; }

    public string Title { get; }

    public ViewModelBase Page { get; }

    #endregion Properties

    #region Constructor

    public NavigationModel(
        string image,
        string title,
        ViewModelBase page)
    {
        VectorImage newImage = new();
        newImage.Classes.Add(image);
        newImage.Classes.Add(FOREGROUND_VECTOR_IMAGE_CLASS);
        VectorImage = newImage;
        Title = title;
        Page = page;
    }

    #endregion Constructor
}
