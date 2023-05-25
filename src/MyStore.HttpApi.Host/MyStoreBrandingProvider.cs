using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace MyStore;

[Dependency(ReplaceServices = true)]
public class MyStoreBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "MyStore";
}
