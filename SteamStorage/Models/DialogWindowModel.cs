using SteamStorage.Models.Tools;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.Models;

public class DialogWindowModel : ModelBase
{
    #region Fields

    private ViewModelBase? _content;

    #endregion Fields
    
    #region Properties

    public ViewModelBase? Content
    {
        get => _content;
        private set => SetProperty(ref _content, value);
    }
    
    #endregion Properties
    
    #region Methods

    public void SetContent(ViewModelBase content)
    {
        Content = content;
    }
    
    #endregion Methods
}
