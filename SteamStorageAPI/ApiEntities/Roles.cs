using SteamStorageAPI.ApiEntities.Tools;

namespace SteamStorageAPI.ApiEntities;

public static class Roles
{
    #region Records

    public record RoleResponse(
        int Id, 
        string Title) : Response;

    #endregion Records
}