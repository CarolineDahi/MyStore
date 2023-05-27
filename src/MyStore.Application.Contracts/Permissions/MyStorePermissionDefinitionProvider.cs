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

        var productsPermission = myGroup.AddPermission(MyStorePermissions.Products.Default, L("Permission:Products"));
        productsPermission.AddChild(MyStorePermissions.Products.Create, L("Permission:Products.Create"));
        productsPermission.AddChild(MyStorePermissions.Products.Edit, L("Permission:Products.Edit"));
        productsPermission.AddChild(MyStorePermissions.Products.Delete, L("Permission:Products.Delete"));

        var customersPermission = myGroup.AddPermission(MyStorePermissions.Customers.Default, L("Permission:Customers"));
        customersPermission.AddChild(MyStorePermissions.Customers.Create, L("Permission:Customers.Create"));
        customersPermission.AddChild(MyStorePermissions.Customers.Edit, L("Permission:Customers.Edit"));
        customersPermission.AddChild(MyStorePermissions.Customers.Delete, L("Permission:Customers.Delete"));

        var productViewsPermission = myGroup.AddPermission(MyStorePermissions.ProductViews.Default, L("Permission:ProductViews"));
        productViewsPermission.AddChild(MyStorePermissions.ProductViews.Create, L("Permission:ProductViews.Create"));
        productViewsPermission.AddChild(MyStorePermissions.ProductViews.Edit, L("Permission:ProductViews.Edit"));
        productViewsPermission.AddChild(MyStorePermissions.ProductViews.Delete, L("Permission:ProductViews.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MyStoreResource>(name);
    }
}
