namespace SteamStorageAPI.ApiEntities;

public class ActiveGroups
{
    public record ActiveGroupsResponse(int Id, string Title, string Description, string Colour, decimal? GoalSum);

    public record ActiveGroupDynamicsResponse(int Id, DateTime DateUpdate, decimal Sum);

    public record ActiveGroupsCountResponse(int Count);
}