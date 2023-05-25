using Microsoft.Extensions.Configuration;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace MyStore.Settings;

public class MyStoreSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(MyStoreSettings.MySetting1));
        context.Add(new SettingDefinition(MyStoreSettings.AutoApprove, "true"));

    }
}
