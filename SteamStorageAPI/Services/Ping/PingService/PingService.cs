using System.Net.NetworkInformation;

namespace SteamStorageAPI.Services.Ping.PingService;

public class PingService : IPingService
{
    #region Fields

    private readonly string _host;

    #endregion Fields

    #region Constructor

    public PingService(string host)
    {
        _host = host;
    }

    #endregion Constructor

    #region Methods

    public async Task<PingResult.PingResult> GetPing()
    {
        try
        {
            System.Net.NetworkInformation.Ping pingSender = new();
            PingReply reply = await pingSender.SendPingAsync(_host);

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

    #endregion Methods
}
