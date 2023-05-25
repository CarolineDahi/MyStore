using MyStore.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace MyStore.Permissions;

public class MyStorePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(MyStorePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(MyStorePermissions.MyPermission1, L("Permission:MyPermission1"));''

        //var MyStoreGroup = context.AddGroup(MyStorePermissions.GroupName, L("Permission:MyStore"));

        //var productsPermission = MyStoreGroup.AddPermission(MyStorePermissions.Products.Default, L("Permission:Products"));
        //productsPermission.AddChild(MyStorePermissions.Products.Create, L("Permission:Products.Create"));
        //productsPermission.AddChild(MyStorePermissions.Products.Edit, L("Permission:Products.Edit"));
        //productsPermission.AddChild(MyStorePermissions.Products.Delete, L("Permission:Products.Delete"));


    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MyStoreResource>(name);
    }
}
