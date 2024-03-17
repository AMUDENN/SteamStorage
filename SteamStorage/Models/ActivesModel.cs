using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Utilities.Events.Actives;
using SteamStorage.ViewModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.Models;

public class ActivesModel : ModelBase
{
    #region Fields

    private ViewModelBase _currentViewModel;

    private SecondaryNavigationModel? _selectedSecondaryNavigationModel;

    #endregion Fields

    #region Properties

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public IEnumerable<SecondaryNavigationModel> SecondaryNavigationOptions { get; }

    public SecondaryNavigationModel? SelectedSecondaryNavigationModel
    {
        get => _selectedSecondaryNavigationModel;
        set
        {
            SetProperty(ref _selectedSecondaryNavigationModel, value);
            if (value is null) return;
            CurrentViewModel = value.Page;
        }
    }

    #endregion Properties

    #region Constructor

    public ActivesModel(
        ActivesReviewViewModel activesReviewViewModel,
        ListActivesViewModel listActivesViewModel,
        ActiveGroupEditViewModel activeGroupEditViewModel,
        ActiveGroupsModel activeGroupsModel)
    {
        SecondaryNavigationOptions =
        [
            new("Обзор", activesReviewViewModel, true),
            new("Список активов", listActivesViewModel, true),
            new("Управление группами", activeGroupEditViewModel, false),
            new("Управление активами", activesReviewViewModel, false),
            new("Продажа актива", activesReviewViewModel, false)
        ];

        _selectedSecondaryNavigationModel = SecondaryNavigationOptions.First();
        _currentViewModel = _selectedSecondaryNavigationModel.Page;

        activeGroupsModel.AddActive += AddActiveHandler;
        activeGroupsModel.OpenActives += OpenActivesHandler;
        activeGroupsModel.EditActiveGroup += EditActiveGroupHandler;
    }

    #endregion Constructor

    #region Methods

    private void AddActiveHandler(object? sender, AddActiveEventArgs args)
    {
        
    }
    
    private void OpenActivesHandler(object? sender, OpenActivesEventArgs args)
    {
        
    }
    
    private void EditActiveGroupHandler(object? sender, EditActiveGroupEventArgs args)
    {
        SecondaryNavigationModel? navigationModel =
            SecondaryNavigationOptions.FirstOrDefault(x => x.Page.GetType() == typeof(ActiveGroupEditViewModel));

        SelectedSecondaryNavigationModel = navigationModel;
    }

    #endregion Methods
}
