namespace SteamStorage.Services.UserService;

public interface IUserService
{
    public bool LogIn(string token);
    public bool LogOut();
}