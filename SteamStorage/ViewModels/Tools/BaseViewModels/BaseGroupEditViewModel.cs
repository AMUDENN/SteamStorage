using Avalonia.Media;
using SteamStorage.Models.Tools.BaseModels;

namespace SteamStorage.ViewModels.Tools.BaseViewModels;

public class BaseGroupEditViewModel : BaseEditViewModel
{
    #region Fields

    private readonly BaseGroupEditModel _baseGroupEditModel;

    #endregion Fields

    #region Properties

    public string DefaultGroupTitle
    {
        get => _baseGroupEditModel.DefaultGroupTitle;
    }

    public string GroupTitle
    {
        get => _baseGroupEditModel.GroupTitle;
        set => _baseGroupEditModel.GroupTitle = value;
    }

    public string? DefaultDescription
    {
        get => _baseGroupEditModel.DefaultDescription;
    }

    public string? Description
    {
        get => _baseGroupEditModel.Description;
        set => _baseGroupEditModel.Description = value;
    }

    public Color DefaultColour
    {
        get => _baseGroupEditModel.DefaultColour;
    }

    public Color Colour
    {
        get => _baseGroupEditModel.Colour;
        set => _baseGroupEditModel.Colour = value;
    }

    public bool IsNewGroup
    {
        get => _baseGroupEditModel.IsNewGroup;
    }

    #endregion Properties

    #region Constructor

    protected BaseGroupEditViewModel(
        BaseGroupEditModel baseGroupEditModel) : base(baseGroupEditModel)
    {
        _baseGroupEditModel = baseGroupEditModel;
    }

    #endregion Constructor
}
