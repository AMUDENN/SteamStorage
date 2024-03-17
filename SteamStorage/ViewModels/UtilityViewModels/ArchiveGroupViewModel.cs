using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;

namespace SteamStorage.ViewModels.UtilityViewModels;

public class ArchiveGroupViewModel : BaseGroupViewModel
{
    #region Fields

    private readonly ArchiveGroupModel _model;
    private readonly ArchiveGroupsModel _archiveGroupsModel;

    #endregion Fields

    #region Properties

    public ArchiveGroupModel ArchiveGroupModel
    {
        get => _model;
    }

    public string BuySumString
    {
        get => _model.BuySumString;
    }

    public string SoldSumString
    {
        get => _model.SoldSumString;
    }

    public double Change
    {
        get => _model.Change;
    }
    
    #endregion Properties

    #region Commands

    public RelayCommand<ArchiveGroupModel> OpenArchivesCommand
    {
        get => _archiveGroupsModel.OpenArchivesCommand;
    }

    public RelayCommand<ArchiveGroupModel> AddArchiveCommand
    {
        get => _archiveGroupsModel.AddArchiveCommand;
    }

    public RelayCommand AddArchiveGroupCommand
    {
        get => _archiveGroupsModel.AddArchiveGroupCommand; 
    }
    
    public RelayCommand<ArchiveGroupModel> EditArchiveGroupCommand
    {
        get => _archiveGroupsModel.EditArchiveGroupCommand;
    }
    
    public RelayCommand<ArchiveGroupModel> DeleteArchiveGroupCommand
    {
        get => _archiveGroupsModel.DeleteArchiveGroupCommand;
    }

    #endregion Commands

    #region Constructor

    public ArchiveGroupViewModel(
        ArchiveGroupModel model,
        ArchiveGroupsModel archiveGroupsModel) : base(model)
    {
        _model = model;
        _archiveGroupsModel = archiveGroupsModel;

        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        archiveGroupsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
