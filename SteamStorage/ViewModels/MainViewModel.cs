using SteamStorage.Models;
using System.Collections.Generic;
using System.Linq;

namespace SteamStorage.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private string? _imageUrl;
        private string _userName;
        private string _steamId;

        private ViewModelBase _currentViewModel;
        private readonly ViewModelBase _settingsViewModel;

        private readonly IEnumerable<NavigationModel> _navigationOptions;
        private NavigationModel? _selectedNavigationModel;
        private bool _isSettingsChecked;

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

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public IEnumerable<NavigationModel> NavigationOptions
        {
            get => _navigationOptions;
        }

        public NavigationModel? SelectedNavigationModel
        {
            get => _selectedNavigationModel;
            set
            {
                SetProperty(ref _selectedNavigationModel, value);
                if (value is not null)
                {
                    CurrentViewModel = value.Page;
                    IsSettingsChecked = false;
                }
            }
        }

        public bool IsSettingsChecked
        {
            get => _isSettingsChecked;
            set
            {
                SetProperty(ref _isSettingsChecked, value);
                if (value)
                {
                    SelectedNavigationModel = null;
                    CurrentViewModel = _settingsViewModel;
                }
            }
        }

        #endregion Properties

        #region Constructor

        public MainViewModel(ActivesViewModel activesViewModel,
            ArchiveViewModel archiveViewModel,
            HomeViewModel homeViewModel,
            InventoryViewModel inventoryViewModel,
            ProfileViewModel profileViewModel,
            SettingsViewModel settingsViewModel)
        {
            _navigationOptions =
            [
                new("HomeImage", "Главная", homeViewModel),
                new("ActivesImage", "Активы", activesViewModel),
                new("ArchiveImage", "Архив", archiveViewModel),
                new("InventoryImage", "Инвентарь", inventoryViewModel),
                new("ProfileImage", "Профиль", profileViewModel)
            ];

            _settingsViewModel = settingsViewModel;

            _selectedNavigationModel = _navigationOptions.First();
            _currentViewModel = _navigationOptions.First().Page;
            _isSettingsChecked = false;

            _userName = "Username";
            _steamId = "SteamID";

            //_imageUrl = @"https://avatars.steamstatic.com/5e53318650a6b75c4fa89e8f77711d3deca8aa51_full.jpg";
        }

        #endregion Constructor
    }
}
