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
            new("1 день", "1 д", 1),
            new("1 неделя", "1 н", 7),
            new("1 месяц", "1 м", 30),
            new("1 год", "1 г", 365)
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
