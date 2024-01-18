using SteamStorageAPI;

namespace SteamStorage.Services.UserService;

public class UserService : IUserService
{
    public bool LogIn(string token)
    {
        ApiClient.Token = token;
        return true;
    }

    public bool LogOut()
    {
        ApiClient.Token = string.Empty;
        return true;
    }
}