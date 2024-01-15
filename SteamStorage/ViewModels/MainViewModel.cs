using SteamStorage.Models;
using System.Collections.Generic;
using System.Linq;

namespace SteamStorage.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        #region Fields
        private string? _imageUrl;
        private string _userName;
        private string _steamId;

        private ViewModelBase _currentVM;

        private readonly IEnumerable<NavigationModel> _navigationOptions;
        private NavigationModel _selectedNavigationModel;
        #endregion Fields

        #region Properties
        public string? ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        public string SteamId
        {
            get => _steamId;
            set => SetProperty(ref _steamId, value);
        }
        public ViewModelBase CurrentVM
        {
            get => _currentVM;
            set => SetProperty(ref _currentVM, value);
        }
        public IEnumerable<NavigationModel> NavigationOptions => _navigationOptions;
        public NavigationModel SelectedNavigationModel
        {
            get => _selectedNavigationModel;
            set
            {
                SetProperty(ref _selectedNavigationModel, value);
                CurrentVM = _selectedNavigationModel.Page;
                UserName = _selectedNavigationModel.Title;
            }
        }
        #endregion Properties

        #region Constructor
        public MainViewModel(ActivesViewModel activesVM, ArchiveViewModel archiveVM, HomeViewModel homeVM, InventoryViewModel inventoryVM, ProfileViewModel profileVM)
        {
            _navigationOptions =
            [
                new("HomeImage", "Главная", homeVM),
                new("ActivesImage", "Активы", activesVM),
                new("ArchiveImage", "Архив", archiveVM),
                new("InventoryImage", "Инвентарь", inventoryVM),
                new("ProfileImage", "Профиль", profileVM)
            ];

            _selectedNavigationModel = _navigationOptions.First();
            _currentVM = _navigationOptions.First().Page;

            _userName = "Username";
            _steamId = "SteamID";

            _imageUrl = @"https://avatars.steamstatic.com/5e53318650a6b75c4fa89e8f77711d3deca8aa51_full.jpg";
        }
        #endregion Constructor
    }
}
