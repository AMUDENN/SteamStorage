﻿using SteamStorageAPI.ApiEntities.Tools;

namespace SteamStorageAPI.ApiEntities;

public static class Pages
{
    #region Records

    public record PageResponse(int Id, string Title);

    public record SetPageRequest(int PageId) : Request;

    #endregion Records
}