using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using SteamStorage.ViewModels.UtilityViewModels.BaseViewModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models.BaseModels;

public abstract class ExtendedBaseItemEditModel : BaseItemEditModel
{
    #region Fields

    private readonly ApiClient _apiClient;

    private BaseSkinViewModel? _defaultSkinModel;
    private BaseSkinViewModel? _selectedSkinModel;
    private string? _filter;
    private AutoCompleteFilterPredicate<object?>? _itemFilter;
    private Func<string?, CancellationToken, Task<IEnumerable<object>>>? _asyncPopulator;
    private List<BaseSkinViewModel> _skinModels;

    private CancellationTokenSource _cancellationTokenSource;

    #endregion Fields

    #region Properties

    public BaseSkinViewModel? DefaultSkinModel
    {
        get => _defaultSkinModel;
        protected set => SetProperty(ref _defaultSkinModel, value);
    }

    public BaseSkinViewModel? SelectedSkinModel
    {
        get => _selectedSkinModel;
        set
        {
            SetProperty(ref _selectedSkinModel, value);
            SaveCommand.NotifyCanExecuteChanged();
            SetTitle(value);
        }
    }

    public string? Filter
    {
        get => _filter;
        set
        {
            SetProperty(ref _filter, value);
            GetSkins();
        }
    }

    public AutoCompleteFilterPredicate<object?>? ItemFilter
    {
        get => _itemFilter;
        private set => SetProperty(ref _itemFilter, value);
    }

    public Func<string?, CancellationToken, Task<IEnumerable<object>>>? AsyncPopulator
    {
        get => _asyncPopulator;
        private set => SetProperty(ref _asyncPopulator, value);
    }

    public List<BaseSkinViewModel> SkinModels
    {
        get => _skinModels;
        private set => SetProperty(ref _skinModels, value);
    }

    private CancellationTokenSource CancellationTokenSource
    {
        get => _cancellationTokenSource;
        set => SetProperty(ref _cancellationTokenSource, value);
    }

    #endregion Properties

    #region Constructor

    protected ExtendedBaseItemEditModel(
        ApiClient apiClient)
    {
        _apiClient = apiClient;

        _skinModels = [];
        _cancellationTokenSource = new();

        ItemFilter = ItemFilterPredicate;
        AsyncPopulator = PopulateAsync;
    }

    #endregion Constructor

    #region Methods
    
    protected abstract void SetTitle(BaseSkinViewModel? model);

    private bool ItemFilterPredicate(string? search, object? item)
    {
        return item is not null &&
               (string.IsNullOrEmpty(search) ||
                ((BaseSkinViewModel)item).Title.Contains(search, StringComparison.CurrentCultureIgnoreCase));
    }

    private async Task<IEnumerable<object>> PopulateAsync(string? searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(0.4), cancellationToken);

        return
            SkinModels.Where(data =>
                    string.IsNullOrEmpty(searchText) ||
                    data.Title.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
    }

    private async void GetSkins()
    {
        await CancellationTokenSource.CancelAsync();

        CancellationTokenSource = new();
        CancellationToken token = CancellationTokenSource.Token;

        Skins.BaseSkinsResponse? skinsResponse =
            await _apiClient.GetAsync<Skins.BaseSkinsResponse, Skins.GetBaseSkinsRequest>(
                ApiConstants.ApiControllers.Skins,
                ApiConstants.ApiMethods.GetBaseSkins,
                new(Filter),
                token);

        if (skinsResponse is null) return;

        SkinModels = skinsResponse.Skins.Select(x =>
                new BaseSkinViewModel(new(x.Id,
                    x.SkinIconUrl,
                    x.MarketUrl,
                    x.Title)))
            .ToList();
    }

    #endregion Methods
}
