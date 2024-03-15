using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

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

        GetPages();
    }

    #endregion Constructor

    #region Methods

    private async void GetPages()
    {
        IEnumerable<Pages.PageResponse>? pageResponses =
            await _apiClient.GetAsync<IEnumerable<Pages.PageResponse>>(
                ApiConstants.ApiControllers.Pages,
                "GetPages");
        if (pageResponses is null) return;
        PageModels = pageResponses.Select(x => new PageModel(x.Id, x.Title)).ToList();
        OnPagesLoaded();
    }
    
    private void OnPagesLoaded()
    {
        PagesLoaded?.Invoke(this);
    }

    #endregion Methods
}
