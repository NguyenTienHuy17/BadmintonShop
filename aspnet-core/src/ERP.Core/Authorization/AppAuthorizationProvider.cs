using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ERP.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var carts = pages.CreateChildPermission(AppPermissions.Pages_Carts, L("Carts"));
            carts.CreateChildPermission(AppPermissions.Pages_Carts_Create, L("CreateNewCart"));
            carts.CreateChildPermission(AppPermissions.Pages_Carts_Edit, L("EditCart"));
            carts.CreateChildPermission(AppPermissions.Pages_Carts_Delete, L("DeleteCart"));

            var returnProds = pages.CreateChildPermission(AppPermissions.Pages_ReturnProds, L("ReturnProds"));
            returnProds.CreateChildPermission(AppPermissions.Pages_ReturnProds_Create, L("CreateNewReturnProd"));
            returnProds.CreateChildPermission(AppPermissions.Pages_ReturnProds_Edit, L("EditReturnProd"));
            returnProds.CreateChildPermission(AppPermissions.Pages_ReturnProds_Delete, L("DeleteReturnProd"));

            var orderItems = pages.CreateChildPermission(AppPermissions.Pages_OrderItems, L("OrderItems"));
            orderItems.CreateChildPermission(AppPermissions.Pages_OrderItems_Create, L("CreateNewOrderItem"));
            orderItems.CreateChildPermission(AppPermissions.Pages_OrderItems_Edit, L("EditOrderItem"));
            orderItems.CreateChildPermission(AppPermissions.Pages_OrderItems_Delete, L("DeleteOrderItem"));

            var orders = pages.CreateChildPermission(AppPermissions.Pages_Orders, L("Orders"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_Create, L("CreateNewOrder"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_Edit, L("EditOrder"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_Delete, L("DeleteOrder"));

            var statuses = pages.CreateChildPermission(AppPermissions.Pages_Statuses, L("Statuses"));
            statuses.CreateChildPermission(AppPermissions.Pages_Statuses_Create, L("CreateNewStatus"));
            statuses.CreateChildPermission(AppPermissions.Pages_Statuses_Edit, L("EditStatus"));
            statuses.CreateChildPermission(AppPermissions.Pages_Statuses_Delete, L("DeleteStatus"));

            var productImages = pages.CreateChildPermission(AppPermissions.Pages_ProductImages, L("ProductImages"));
            productImages.CreateChildPermission(AppPermissions.Pages_ProductImages_Create, L("CreateNewProductImage"));
            productImages.CreateChildPermission(AppPermissions.Pages_ProductImages_Edit, L("EditProductImage"));
            productImages.CreateChildPermission(AppPermissions.Pages_ProductImages_Delete, L("DeleteProductImage"));

            var colorItems = pages.CreateChildPermission(AppPermissions.Pages_ColorItems, L("ColorItems"));
            colorItems.CreateChildPermission(AppPermissions.Pages_ColorItems_Create, L("CreateNewColorItem"));
            colorItems.CreateChildPermission(AppPermissions.Pages_ColorItems_Edit, L("EditColorItem"));
            colorItems.CreateChildPermission(AppPermissions.Pages_ColorItems_Delete, L("DeleteColorItem"));

            var sizeItems = pages.CreateChildPermission(AppPermissions.Pages_SizeItems, L("SizeItems"));
            sizeItems.CreateChildPermission(AppPermissions.Pages_SizeItems_Create, L("CreateNewSizeItem"));
            sizeItems.CreateChildPermission(AppPermissions.Pages_SizeItems_Edit, L("EditSizeItem"));
            sizeItems.CreateChildPermission(AppPermissions.Pages_SizeItems_Delete, L("DeleteSizeItem"));

            var products = pages.CreateChildPermission(AppPermissions.Pages_Products, L("Products"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Create, L("CreateNewProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Edit, L("EditProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Delete, L("DeleteProduct"));

            var sizes = pages.CreateChildPermission(AppPermissions.Pages_Sizes, L("Sizes"));
            sizes.CreateChildPermission(AppPermissions.Pages_Sizes_Create, L("CreateNewSize"));
            sizes.CreateChildPermission(AppPermissions.Pages_Sizes_Edit, L("EditSize"));
            sizes.CreateChildPermission(AppPermissions.Pages_Sizes_Delete, L("DeleteSize"));

            var colors = pages.CreateChildPermission(AppPermissions.Pages_Colors, L("Colors"));
            colors.CreateChildPermission(AppPermissions.Pages_Colors_Create, L("CreateNewColor"));
            colors.CreateChildPermission(AppPermissions.Pages_Colors_Edit, L("EditColor"));
            colors.CreateChildPermission(AppPermissions.Pages_Colors_Delete, L("DeleteColor"));

            var categories = pages.CreateChildPermission(AppPermissions.Pages_Categories, L("Categories"));
            categories.CreateChildPermission(AppPermissions.Pages_Categories_Create, L("CreateNewCategory"));
            categories.CreateChildPermission(AppPermissions.Pages_Categories_Edit, L("EditCategory"));
            categories.CreateChildPermission(AppPermissions.Pages_Categories_Delete, L("DeleteCategory"));

            var brands = pages.CreateChildPermission(AppPermissions.Pages_Brands, L("Brands"));
            brands.CreateChildPermission(AppPermissions.Pages_Brands_Create, L("CreateNewBrand"));
            brands.CreateChildPermission(AppPermissions.Pages_Brands_Edit, L("EditBrand"));
            brands.CreateChildPermission(AppPermissions.Pages_Brands_Delete, L("DeleteBrand"));

            var images = pages.CreateChildPermission(AppPermissions.Pages_Images, L("Images"));
            images.CreateChildPermission(AppPermissions.Pages_Images_Create, L("CreateNewImage"));
            images.CreateChildPermission(AppPermissions.Pages_Images_Edit, L("EditImage"));
            images.CreateChildPermission(AppPermissions.Pages_Images_Delete, L("DeleteImage"));

            var discounts = pages.CreateChildPermission(AppPermissions.Pages_Discounts, L("Discounts"));
            discounts.CreateChildPermission(AppPermissions.Pages_Discounts_Create, L("CreateNewDiscount"));
            discounts.CreateChildPermission(AppPermissions.Pages_Discounts_Edit, L("EditDiscount"));
            discounts.CreateChildPermission(AppPermissions.Pages_Discounts_Delete, L("DeleteDiscount"));

            var blogs = pages.CreateChildPermission(AppPermissions.Pages_Blogs, L("Blogs"));
            blogs.CreateChildPermission(AppPermissions.Pages_Blogs_Create, L("CreateNewBlog"));
            blogs.CreateChildPermission(AppPermissions.Pages_Blogs_Edit, L("EditBlog"));
            blogs.CreateChildPermission(AppPermissions.Pages_Blogs_Delete, L("DeleteBlog"));

            var bookings = pages.CreateChildPermission(AppPermissions.Pages_Bookings, L("Bookings"));
            bookings.CreateChildPermission(AppPermissions.Pages_Bookings_Create, L("CreateNewBooking"));
            bookings.CreateChildPermission(AppPermissions.Pages_Bookings_Edit, L("EditBooking"));
            bookings.CreateChildPermission(AppPermissions.Pages_Bookings_Delete, L("DeleteBooking"));

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ERPConsts.LocalizationSourceName);
        }
    }
}