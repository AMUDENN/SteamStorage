using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.BaseModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.ViewModels.Tools;
using SteamStorage.ViewModels.UtilityViewModels.BaseViewModels;

namespace SteamStorage.ViewModels.BaseViewModels;

public abstract class BaseItemEditViewModel : ViewModelBase
{
    #region Fields

    private readonly BaseItemEditModel _baseItemEditModel;

    #endregion Fields

    #region Properties

    public string Title
    {
        get => _baseItemEditModel.Title;
    }

    public BaseGroupModel? DefaultGroupModel
    {
        get => _baseItemEditModel.DefaultGroupModel;
    }

    public BaseGroupModel? SelectedGroupModel
    {
        get => _baseItemEditModel.SelectedGroupModel;
        set => _baseItemEditModel.SelectedGroupModel = value;
    }

    public BaseSkinViewModel? DefaultSkinModel
    {
        get => _baseItemEditModel.DefaultSkinModel;
    }

    public BaseSkinViewModel? SelectedSkinModel
    {
        get => _baseItemEditModel.SelectedSkinModel;
        set => _baseItemEditModel.SelectedSkinModel = value;
    }

    public string? Filter
    {
        get => _baseItemEditModel.Filter;
        set => _baseItemEditModel.Filter = value;
    }

    public AutoCompleteFilterPredicate<object?>? ItemFilter
    {
        get => _baseItemEditModel.ItemFilter;
    }

    public Func<string?, CancellationToken, Task<IEnumerable<object>>>? AsyncPopulator
    {
        get => _baseItemEditModel.AsyncPopulator;
    }

    public IEnumerable<BaseSkinViewModel> SkinModels
    {
        get => _baseItemEditModel.SkinModels;
    }

    #endregion Properties

    #region Commands

    public RelayCommand BackCommand
    {
        get => _baseItemEditModel.BackCommand;
    }

    public RelayCommand DeleteCommand
    {
        get => _baseItemEditModel.DeleteCommand;
    }

    public RelayCommand SaveCommand
    {
        get => _baseItemEditModel.SaveCommand;
    }

    #endregion Commands

    #region Constructor

    public BaseItemEditViewModel(
        BaseItemEditModel baseItemEditModel)
    {
        _baseItemEditModel = baseItemEditModel;

        baseItemEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
