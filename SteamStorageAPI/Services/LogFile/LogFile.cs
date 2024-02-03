using System.Text;

namespace SteamStorageAPI.Services.LogFile;

public class LogFile : IDisposable
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

    #region Destructor

    void IDisposable.Dispose()
    {
        _streamWriter.Flush();
        _streamWriter.Dispose();
    }

    #endregion Destructor

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
            System.Diagnostics.Debug.WriteLine("LogError");
        }
    }

    private void CreateFile()
    {
        if (File.Exists(_filePath)) return;
        string? directoryName = Path.GetDirectoryName(_filePath);
        if (directoryName is null) return;
        Directory.CreateDirectory(directoryName);
        File.Create(_filePath).Close();
        System.Diagnostics.Debug.WriteLine($"Created file: {_filePath}");
    }

    #endregion Methods
}
