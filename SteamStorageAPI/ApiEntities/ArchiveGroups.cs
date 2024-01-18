namespace SteamStorageAPI.ApiEntities;

public class ArchiveGroups
{
    public record ArchiveGroupsResponse(int Id, string Title, string Description, string Colour);

    public record ArchiveGroupsCountResponse(int Count);
}