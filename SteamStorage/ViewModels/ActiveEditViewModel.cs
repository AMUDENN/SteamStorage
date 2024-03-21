using System;
using System.Collections.Generic;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;
using SteamStorage.ViewModels.UtilityViewModels;

namespace SteamStorage.ViewModels;

public class ActiveEditViewModel : ViewModelBase
{
    #region Fields

    private readonly ActiveEditModel _activeEditModel;
    private readonly ActiveGroupsModel _activeGroupsModel;

    #endregion Fields

    #region Properties

    public IEnumerable<BaseGroupModel> ActiveGroupModels
    {
        get => _activeGroupsModel.ActiveGroupModels;
    }

    public string Title
    {
        get => _activeEditModel.Title;
    }

    public BaseGroupModel? DefaultGroupModel
    {
        get => _activeEditModel.DefaultGroupModel;
    }

    public BaseGroupModel? SelectedGroupModel
    {
        get => _activeEditModel.SelectedGroupModel;
        set => _activeEditModel.SelectedGroupModel = value;
    }

    public string DefaultCount
    {
        get => _activeEditModel.DefaultCount;
    }

    public string Count
    {
        get => _activeEditModel.Count;
        set => _activeEditModel.Count = value;
    }

    public string DefaultBuyPrice
    {
        get => _activeEditModel.DefaultBuyPrice;
    }

    public string BuyPrice
    {
        get => _activeEditModel.BuyPrice;
        set => _activeEditModel.BuyPrice = value;
    }

    public string? DefaultGoalPrice
    {
        get => _activeEditModel.DefaultGoalPrice;
    }

    public string? GoalPrice
    {
        get => _activeEditModel.GoalPrice;
        set => _activeEditModel.GoalPrice = value;
    }
    
    public BaseSkinViewModel? DefaultSkinModel
    {
        get => _activeEditModel.DefaultSkinModel;
    }

    public BaseSkinViewModel? SelectedSkinModel
    {
        get => _activeEditModel.SelectedSkinModel;
        set => _activeEditModel.SelectedSkinModel = value;
    }
    
    public string? Filter
    {
        get => _activeEditModel.Filter;
        set => _activeEditModel.Filter = value;
    }

    public AutoCompleteFilterPredicate<object?>? ItemFilter
    {
        get=> _activeEditModel.ItemFilter;
    }
    
    public IEnumerable<BaseSkinViewModel> SkinModels
    {
        get => _activeEditModel.SkinModels;
    }

    public string? DefaultDescription
    {
        get => _activeEditModel.DefaultDescription;
    }

    public string? Description
    {
        get => _activeEditModel.Description;
        set => _activeEditModel.Description = value;
    }

    public DateTimeOffset DefaultBuyDate
    {
        get => _activeEditModel.DefaultBuyDate;
    }

    public DateTimeOffset BuyDate
    {
        get => _activeEditModel.BuyDate;
        set => _activeEditModel.BuyDate = value;
    }

    #endregion Properties

    #region Commands

    public RelayCommand DeleteCommand
    {
        get => _activeEditModel.DeleteCommand;
    }
    
    public RelayCommand SaveCommand
    {
        get => _activeEditModel.SaveCommand;
    }

    #endregion Commands

    #region Constructor

    public ActiveEditViewModel(
        ActiveEditModel activeEditModel,
        ActiveGroupsModel activeGroupsModel)
    {
        _activeEditModel = activeEditModel;
        _activeGroupsModel = activeGroupsModel;

        activeEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        activeGroupsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void SetEditActive(ActiveModel? model)
    {
        _activeEditModel.SetEditActive(model);
    }

    public void SetAddActive(ActiveGroupModel? model)
    {
        _activeEditModel.SetAddActive(model);
    }

    public void SetAddActive(ListItemModel? model)
    {
        _activeEditModel.SetAddActive(model);
    }

    #endregion Methods
}
