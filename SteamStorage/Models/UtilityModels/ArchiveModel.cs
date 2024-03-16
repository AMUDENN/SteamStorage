﻿using System;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities;
using SteamStorageAPI;

namespace SteamStorage.Models.UtilityModels;

public class ArchiveModel : BaseSkinModel
{
    #region Properties

    public int Count { get; }

    public string BuyPriceString { get; }

    public string SoldPriceString { get; }

    public string SoldSumString { get; }

    public double Change { get; }

    public string BuyDateString { get; }

    public string SoldDateString { get; }

    #endregion Properties

    #region Commands

    public RelayCommand EditCommand { get; }

    public RelayCommand DeleteCommand { get; }

    #endregion Commands

    #region Constructor

    public ArchiveModel(
        int skinId,
        string imageUrl,
        string marketUrl,
        string title,
        int count,
        decimal buyPrice,
        decimal soldPrice,
        decimal soldSum,
        string currencyMark,
        double change,
        DateTime buyDate,
        DateTime soldDate) : base(skinId, imageUrl, marketUrl, title)
    {
        Count = count;

        BuyPriceString = $"{buyPrice:N2} {currencyMark}";
        SoldPriceString = $"{soldPrice:N2} {currencyMark}";
        SoldSumString = $"{soldSum:N2} {currencyMark}";

        Change = change;
        
        BuyDateString = buyDate.ToString(ProgramConstants.VIEW_DATE_FORMAT);
        SoldDateString = soldDate.ToString(ProgramConstants.VIEW_DATE_FORMAT);

        EditCommand = new(DoEditCommand);
        DeleteCommand = new(DoDeleteCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoEditCommand()
    {

    }

    private void DoDeleteCommand()
    {

    }

    #endregion Methods
}
