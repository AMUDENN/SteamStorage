﻿using System;
using System.Collections.Generic;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;

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
    
    public BaseGroupModel? DefaultGroupModel
    {
        get => _activeEditModel.DefaultGroupModel;
    }

    public BaseGroupModel? SelectedGroupModel
    {
        get => _activeEditModel.SelectedGroupModel;
        set => _activeEditModel.SelectedGroupModel = value;
    }

    public int DefaultCount
    {
        get => _activeEditModel.DefaultCount;
    }

    public int Count
    {
        get => _activeEditModel.Count;
        set => _activeEditModel.Count = value;
    }

    public decimal DefaultBuyPrice
    {
        get => _activeEditModel.DefaultBuyPrice;
    }

    public decimal BuyPrice
    {
        get => _activeEditModel.BuyPrice;
        set => _activeEditModel.BuyPrice = value;
    }

    public decimal? DefaultGoalPrice
    {
        get => _activeEditModel.DefaultGoalPrice;
    }

    public decimal? GoalPrice
    {
        get => _activeEditModel.GoalPrice;
        set => _activeEditModel.GoalPrice = value;
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
