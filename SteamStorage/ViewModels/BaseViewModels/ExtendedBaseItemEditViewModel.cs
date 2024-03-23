using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using SteamStorage.Models.BaseModels;
using SteamStorage.ViewModels.UtilityViewModels.BaseViewModels;

namespace SteamStorage.ViewModels.BaseViewModels;

public abstract class ExtendedBaseItemEditViewModel : BaseItemEditViewModel
{
    #region Fields

    private readonly ExtendedBaseItemEditModel _extendedBaseItemEditModel;

    #endregion Fields

    #region Properties

    public BaseSkinViewModel? DefaultSkinModel
    {
        get => _extendedBaseItemEditModel.DefaultSkinModel;
    }

    public BaseSkinViewModel? SelectedSkinModel
    {
        get => _extendedBaseItemEditModel.SelectedSkinModel;
        set => _extendedBaseItemEditModel.SelectedSkinModel = value;
    }

    public string? Filter
    {
        get => _extendedBaseItemEditModel.Filter;
        set => _extendedBaseItemEditModel.Filter = value;
    }

    public AutoCompleteFilterPredicate<object?>? ItemFilter
    {
        get => _extendedBaseItemEditModel.ItemFilter;
    }

    public Func<string?, CancellationToken, Task<IEnumerable<object>>>? AsyncPopulator
    {
        get => _extendedBaseItemEditModel.AsyncPopulator;
    }

    public IEnumerable<BaseSkinViewModel> SkinModels
    {
        get => _extendedBaseItemEditModel.SkinModels;
    }

    #endregion Properties

    #region Constructor

    protected ExtendedBaseItemEditViewModel(
        ExtendedBaseItemEditModel extendedBaseItemEditModel) : base(extendedBaseItemEditModel)
    {
        _extendedBaseItemEditModel = extendedBaseItemEditModel;

        extendedBaseItemEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
