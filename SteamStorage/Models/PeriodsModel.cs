using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.Tools.UtilityModels;

namespace SteamStorage.Models;

public class PeriodsModel : ModelBase
{
    #region Fields

    private List<PeriodModel> _periodModels;

    #endregion Fields

    #region Properties

    public List<PeriodModel> PeriodModels
    {
        get => _periodModels;
        private set => SetProperty(ref _periodModels, value);
    }

    #endregion Properties

    #region Constructor

    public PeriodsModel()
    {
        _periodModels = [];

        PeriodModels =
        [
            new PeriodModel("1 day", "1 d", 1),
            new PeriodModel("1 week", "1 w", 7),
            new PeriodModel("1 month", "1 m", 30),
            new PeriodModel("1 year", "1 y", 365)
        ];
    }

    #endregion Constructor

    #region Methods

    public PeriodModel? GetDefault()
    {
        return PeriodModels.Count switch
        {
            0 => null,
            1 or 2 or 3 => PeriodModels.Last(),
            > 3 => PeriodModels[2],
            _ => null
        };
    }

    #endregion Methods
}