using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls.Primitives;

namespace SteamStorage.Resources.Controls;

public class VectorImage : TemplatedControl
{
    static VectorImage()
    {
        AffectsRender<VectorImage>(ForegroundProperty);
        AffectsMeasure<VectorImage>(ForegroundProperty);
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<VectorImage>(AutomationControlType.Image);
    }
}
