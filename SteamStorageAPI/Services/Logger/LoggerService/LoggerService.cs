using System.Reflection;
using System.Text;

namespace SteamStorageAPI.Services.Logger.LoggerService;

public class LoggerService : ILoggerService
{
    #region Fields

    private readonly LogFile.LogFile _file;

    #endregion Fields

    #region Constructor

    public LoggerService(
        string programName, 
        string dateFormat, 
        string dateTimeFormat)
    {
        _file = new(programName, dateFormat, dateTimeFormat);
    }

    #endregion Constructor

    #region Methods

    public async Task LogAsync(string message)
    {
        await _file.WriteAsync(message);
    }

    public async Task LogAsync(Exception exception)
    {
        await _file.WriteAsync($"\n{CreateErrorMessage(exception)}");
    }

    public async Task LogAsync(string message, Exception exception)
    {
        await _file.WriteAsync($"{message}\n{CreateErrorMessage(exception)}");
    }

    private static string CreateErrorMessage(Exception exception)
    {
        StringBuilder errorInfo = new();
        foreach (PropertyInfo property in exception.GetType().GetProperties())
            errorInfo.AppendLine($"\t{property.Name}: {property.GetValue(exception)}");
        errorInfo.AppendLine();
        return errorInfo.ToString();
    }

    #endregion Methods
}
