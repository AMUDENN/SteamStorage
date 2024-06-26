﻿using System;
using System.IO;
using System.Diagnostics;
using System.Text.Json;

namespace SteamStorage.Services.Settings.SettingsFile;

public class SettingsFile
{
    #region Fields

    private readonly string _filePath;

    #endregion Fields

    #region Constructor

    public SettingsFile(
        string programName)
    {
        _filePath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{programName}\appSettings.json";
    }

    #endregion Constructor

    #region Methods

    public void Write<T>(T value)
    {
        try
        {
            DeleteFile();
            CreateFile();
            using FileStream fs = new(_filePath, FileMode.OpenOrCreate, FileAccess.Write);
            JsonSerializer.Serialize(fs, value);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Settings Write Error: {ex.Message}");
        }
    }

    public T? Read<T>()
    {
        try
        {
            using FileStream fs = new(_filePath, FileMode.OpenOrCreate, FileAccess.Read);
            return JsonSerializer.Deserialize<T>(fs);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Settings Read Error: {ex.Message}");
        }

        return default;
    }
    
    private void CreateFile()
    {
        if (File.Exists(_filePath)) return;
        string? directoryName = Path.GetDirectoryName(_filePath);
        if (directoryName is null) return;
        Directory.CreateDirectory(directoryName);
        File.Create(_filePath).Close();
        Debug.WriteLine($"Created file: {_filePath}");
    }

    private void DeleteFile()
    {
        if (!File.Exists(_filePath)) return;
        File.Delete(_filePath);
    }

    #endregion Methods
}
