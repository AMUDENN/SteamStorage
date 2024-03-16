using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities;
using SteamStorage.Utilities.Events;
using SteamStorage.Utilities.ThemeVariants;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models.UtilityModels;

public class ArchiveGroupModel : ExtendedBaseGroupModel
{
    #region Properties

    public string BuySumString { get; }

    public string SoldSumString { get; }

    public double Change { get; }
    
    #endregion Properties

    #region Constructor

    public ArchiveGroupModel(
        int groupId,
        string colour,
        string title,
        int count,
        decimal buySum,
        decimal soldSum,
        string currencyMark,
        double change,
        DateTime dateCreation) : base(groupId, colour, title, count, dateCreation)
    {
        BuySumString = $"{buySum:N2} {currencyMark}";
        SoldSumString = $"{soldSum:N2} {currencyMark}";

        Change = change;
    }

    #endregion Constructor
}
