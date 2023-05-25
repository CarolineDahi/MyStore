using MyStore.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MyStore;

[DependsOn(
    typeof(MyStoreEntityFrameworkCoreTestModule)
    )]
public class MyStoreDomainTestModule : AbpModule
{

}
