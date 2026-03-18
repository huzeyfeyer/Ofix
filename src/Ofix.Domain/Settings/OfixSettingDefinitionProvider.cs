using Volo.Abp.Settings;

namespace Ofix.Settings;

public class OfixSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(OfixSettings.MySetting1));
    }
}
