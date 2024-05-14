using System.Threading.Tasks;

namespace SteamStorage.Services.ClipboardService;

public interface IClipboardService
{
    public Task SaveToClipboard(string text);
}
