using System.Net.NetworkInformation;

namespace SteamStorageAPI.Services.PingService;

public class PingService : IPingService
{
    public async Task<PingResult.PingResult> GetPing(string host)
    {
        try
        {
            Ping pingSender = new();
            PingReply reply = await pingSender.SendPingAsync(host);

            if (reply.Status == IPStatus.Success)
            {
                return new(reply.RoundtripTime);
            }
        }
        catch
        {
            return new(-1);
        }

        return new(-1);
    }
}