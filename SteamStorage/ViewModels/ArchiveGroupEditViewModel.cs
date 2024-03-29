using Avalonia.Media;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.BaseViewModels;

namespace SteamStorage.ViewModels;

public class ArchiveGroupEditViewModel : BaseEditViewModel
{
    #region Fields

    private readonly ArchiveGroupEditModel _archiveGroupEditModel;

    #endregion Fields

    #region Properties

    public string DefaultGroupTitle
    {
        get => _archiveGroupEditModel.DefaultGroupTitle;
    }

    public string GroupTitle
    {
        get => _archiveGroupEditModel.GroupTitle;
        set => _archiveGroupEditModel.GroupTitle = value;
    }

    public string? DefaultDescription
    {
        get => _archiveGroupEditModel.DefaultDescription;
    }

    public string? Description
    {
        get => _archiveGroupEditModel.Description;
        set => _archiveGroupEditModel.Description = value;
    }

    public Color DefaultColour
    {
        get => _archiveGroupEditModel.DefaultColour;
    }

    public Color Colour
    {
        get => _archiveGroupEditModel.Colour;
        set => _archiveGroupEditModel.Colour = value;
    }

    public bool IsNewGroup
    {
        get => _archiveGroupEditModel.IsNewGroup;
    }

    public string DateCreationString
    {
        get => _archiveGroupEditModel.DateCreationString;
    }

    public string BuySumString
    {
        get => _archiveGroupEditModel.BuySumString;
    }

    public string SoldSumString
    {
        get => _archiveGroupEditModel.SoldSumString;
    }

    public string CountString
    {
        get => _archiveGroupEditModel.CountString;
    }

    #endregion Properties

    #region Constructor

    public ArchiveGroupEditViewModel(
        ArchiveGroupEditModel archiveGroupEditModel) : base(archiveGroupEditModel)
    {
        _archiveGroupEditModel = archiveGroupEditModel;
        archiveGroupEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void SetEditGroup(ArchiveGroupModel? model)
    {
        _archiveGroupEditModel.SetEditGroup(model);
    }

    #endregion Methods
}
