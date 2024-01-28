using System.Text;

namespace SteamStorageAPI.Services.LogFile;

public class LogFile
{
    #region Fields

    private readonly string _dateTimeFormat;

    private readonly string _filePath;
    private readonly StreamWriter _streamWriter;

    #endregion Fields

    #region Constructor

    public LogFile(string programName, string dateFormat, string dateTimeFormat)
    {
        _dateTimeFormat = dateTimeFormat;

        _filePath = $"{Path.GetTempPath()}/{programName}/{DateTime.Now.ToString(dateFormat)}#{Guid.NewGuid()}.txt";
        CreateFile();
        _streamWriter = new(_filePath, true, Encoding.UTF8, 8192);
    }

    #endregion Constructor

    #region Methods

    public async Task Write(string message)
    {
        try
        {
            CreateFile();
            await _streamWriter.WriteLineAsync($"[{DateTime.Now.ToString(_dateTimeFormat)}]: {message}");
            await _streamWriter.FlushAsync();
        }
        catch
        {
            // ignored
        }
    }

    private void CreateFile()
    {
        if (File.Exists(_filePath)) return;
        string? directoryName = Path.GetDirectoryName(_filePath);
        if (directoryName is null) return;
        Directory.CreateDirectory(directoryName);
        File.Create(_filePath).Close();
    }

    #endregion Methods
}