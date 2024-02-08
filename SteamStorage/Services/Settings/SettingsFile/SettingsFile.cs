﻿using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SteamStorage.Services.Settings.SettingsFile;

public class SettingsFile
{
    #region Fields

    private readonly string _filePath;

    #endregion Fields

    #region Constructor

    public SettingsFile(string programName)
    {
        _filePath = @$"{Environment.SpecialFolder.ApplicationData}\{programName}\appSettings.json";
    }

    #endregion Constructor

    #region Methods

    public void Write<T>(T value)
    {
        try
        {
            DeleteFile();
            using FileStream fs = new(_filePath, FileMode.OpenOrCreate);
            JsonSerializer.Serialize(fs, value);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Settings Write Error: {ex.Message}");
        }
    }

    public T? Read<T>()
    {
        try
        {
            using FileStream fs = new(_filePath, FileMode.OpenOrCreate);
            return JsonSerializer.Deserialize<T>(fs);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Settings Read Error: {ex.Message}");
        }

        return default;
    }

    private void DeleteFile()
    {
        if(!File.Exists(_filePath)) return;
        File.Delete(_filePath);
    }

    #endregion Methods
}
