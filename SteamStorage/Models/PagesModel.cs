using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models;

public class PagesModel : ModelBase
{
    #region Events

    public delegate void PagesLoadedEventHandler(object? sender);

    public event PagesLoadedEventHandler? PagesLoaded;

    #endregion Events

    #region Fields

    private readonly ApiClient _apiClient;

    private List<PageModel> _pageModels;

    #endregion Fields

    #region Properties

    public List<PageModel> PageModels
    {
        get => _pageModels;
        private set => SetProperty(ref _pageModels, value);
    }

    #endregion Properties

    #region Constructor

    public PagesModel(
        ApiClient apiClient)
    {
        _apiClient = apiClient;

        _pageModels = [];

        GetPagesAsync();
    }

    #endregion Constructor

    #region Methods

    public async void GetPagesAsync()
    {
        Pages.PagesResponse? pageResponses =
            await _apiClient.GetAsync<Pages.PagesResponse>(
                ApiConstants.ApiMethods.GetPages);
        if (pageResponses?.Pages is null) return;
        PageModels = pageResponses.Pages.Select(x => new PageModel(x.Id, x.Title)).ToList();
        OnPagesLoaded();
    }

    private void OnPagesLoaded()
    {
        PagesLoaded?.Invoke(this);
    }

    #endregion Methods
}
