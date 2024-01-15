using Avalonia.Controls;
using SteamStorage.ViewModels;

namespace SteamStorage.Models
{
    public class NavigationModel
    {
        #region Properties
        public Image Image { get; set; }
        public string Title { get; set; }
        public ViewModelBase Page { get; set; }
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
}
