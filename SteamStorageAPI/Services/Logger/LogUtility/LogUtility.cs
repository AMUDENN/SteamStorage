using System.Diagnostics;

namespace SteamStorageAPI.Services.Logger.LogUtility;

internal static class LogUtility
{
    #region Methods

    public static void DeleteEmptyLogFiles(string directory)
    {
        try
        {
            string[] logs = Directory.GetFiles(directory, "*.txt");

            foreach (string logFilePath in logs)
            {
                FileInfo file = new(logFilePath);
                long size = file.Length;
                if (size <= 0) File.Delete(logFilePath);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"An error occurred while trying to delete empty log files {ex.Message}");
        }
    }

    public static void DeleteOldLogFiles(string directory, int daysAgo)
    {
        try
        {
            string[] logs = Directory.GetFiles(directory, "*.txt");

            foreach (string logFilePath in logs)
            {
                FileInfo file = new(logFilePath);
                DateTime lastWrite = file.LastWriteTime;
                if (lastWrite <= DateTime.Now.AddDays(-daysAgo)) File.Delete(logFilePath);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"An error occurred while trying to delete old log files {ex.Message}");
        }
    }

    #endregion Methods
}
