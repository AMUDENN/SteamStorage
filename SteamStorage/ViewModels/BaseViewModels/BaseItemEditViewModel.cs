using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using SteamStorage.Models.BaseModels;
using SteamStorage.ViewModels.UtilityViewModels.BaseViewModels;

namespace SteamStorage.ViewModels.BaseViewModels;

public abstract class BaseItemEditViewModel : BaseEditViewModel
{
    #region Fields

    private readonly BaseItemEditModel _baseItemEditModel;

    #endregion Fields

    #region Properties
    
    public bool IsNewItem
    {
        get => _baseItemEditModel.IsNewItem;
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

    #region Constructor

    protected BaseItemEditViewModel(
        BaseItemEditModel baseItemEditModel) : base(baseItemEditModel)
    {
        _baseItemEditModel = baseItemEditModel;

        baseItemEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
