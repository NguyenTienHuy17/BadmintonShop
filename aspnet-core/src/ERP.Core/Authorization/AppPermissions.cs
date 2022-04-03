namespace ERP.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        public const string Pages_Carts = "Pages.Carts";
        public const string Pages_Carts_Create = "Pages.Carts.Create";
        public const string Pages_Carts_Edit = "Pages.Carts.Edit";
        public const string Pages_Carts_Delete = "Pages.Carts.Delete";

        public const string Pages_ReturnProds = "Pages.ReturnProds";
        public const string Pages_ReturnProds_Create = "Pages.ReturnProds.Create";
        public const string Pages_ReturnProds_Edit = "Pages.ReturnProds.Edit";
        public const string Pages_ReturnProds_Delete = "Pages.ReturnProds.Delete";

        public const string Pages_OrderItems = "Pages.OrderItems";
        public const string Pages_OrderItems_Create = "Pages.OrderItems.Create";
        public const string Pages_OrderItems_Edit = "Pages.OrderItems.Edit";
        public const string Pages_OrderItems_Delete = "Pages.OrderItems.Delete";

        public const string Pages_Orders = "Pages.Orders";
        public const string Pages_Orders_Create = "Pages.Orders.Create";
        public const string Pages_Orders_Edit = "Pages.Orders.Edit";
        public const string Pages_Orders_Delete = "Pages.Orders.Delete";

        public const string Pages_Statuses = "Pages.Statuses";
        public const string Pages_Statuses_Create = "Pages.Statuses.Create";
        public const string Pages_Statuses_Edit = "Pages.Statuses.Edit";
        public const string Pages_Statuses_Delete = "Pages.Statuses.Delete";

        public const string Pages_ProductImages = "Pages.ProductImages";
        public const string Pages_ProductImages_Create = "Pages.ProductImages.Create";
        public const string Pages_ProductImages_Edit = "Pages.ProductImages.Edit";
        public const string Pages_ProductImages_Delete = "Pages.ProductImages.Delete";

        public const string Pages_ColorItems = "Pages.ColorItems";
        public const string Pages_ColorItems_Create = "Pages.ColorItems.Create";
        public const string Pages_ColorItems_Edit = "Pages.ColorItems.Edit";
        public const string Pages_ColorItems_Delete = "Pages.ColorItems.Delete";

        public const string Pages_SizeItems = "Pages.SizeItems";
        public const string Pages_SizeItems_Create = "Pages.SizeItems.Create";
        public const string Pages_SizeItems_Edit = "Pages.SizeItems.Edit";
        public const string Pages_SizeItems_Delete = "Pages.SizeItems.Delete";

        public const string Pages_Products = "Pages.Products";
        public const string Pages_Products_Create = "Pages.Products.Create";
        public const string Pages_Products_Edit = "Pages.Products.Edit";
        public const string Pages_Products_Delete = "Pages.Products.Delete";

        public const string Pages_Sizes = "Pages.Sizes";
        public const string Pages_Sizes_Create = "Pages.Sizes.Create";
        public const string Pages_Sizes_Edit = "Pages.Sizes.Edit";
        public const string Pages_Sizes_Delete = "Pages.Sizes.Delete";

        public const string Pages_Colors = "Pages.Colors";
        public const string Pages_Colors_Create = "Pages.Colors.Create";
        public const string Pages_Colors_Edit = "Pages.Colors.Edit";
        public const string Pages_Colors_Delete = "Pages.Colors.Delete";

        public const string Pages_Categories = "Pages.Categories";
        public const string Pages_Categories_Create = "Pages.Categories.Create";
        public const string Pages_Categories_Edit = "Pages.Categories.Edit";
        public const string Pages_Categories_Delete = "Pages.Categories.Delete";

        public const string Pages_Brands = "Pages.Brands";
        public const string Pages_Brands_Create = "Pages.Brands.Create";
        public const string Pages_Brands_Edit = "Pages.Brands.Edit";
        public const string Pages_Brands_Delete = "Pages.Brands.Delete";

        public const string Pages_Images = "Pages.Images";
        public const string Pages_Images_Create = "Pages.Images.Create";
        public const string Pages_Images_Edit = "Pages.Images.Edit";
        public const string Pages_Images_Delete = "Pages.Images.Delete";

        public const string Pages_Discounts = "Pages.Discounts";
        public const string Pages_Discounts_Create = "Pages.Discounts.Create";
        public const string Pages_Discounts_Edit = "Pages.Discounts.Edit";
        public const string Pages_Discounts_Delete = "Pages.Discounts.Delete";

        public const string Pages_Blogs = "Pages.Blogs";
        public const string Pages_Blogs_Create = "Pages.Blogs.Create";
        public const string Pages_Blogs_Edit = "Pages.Blogs.Edit";
        public const string Pages_Blogs_Delete = "Pages.Blogs.Delete";

        public const string Pages_Bookings = "Pages.Bookings";
        public const string Pages_Bookings_Create = "Pages.Bookings.Create";
        public const string Pages_Bookings_Edit = "Pages.Bookings.Edit";
        public const string Pages_Bookings_Delete = "Pages.Bookings.Delete";

        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_DemoUiComponents = "Pages.DemoUiComponents";
        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";
        public const string Pages_Administration_OrganizationUnits_ManageRoles = "Pages.Administration.OrganizationUnits.ManageRoles";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        public const string Pages_Administration_UiCustomization = "Pages.Administration.UiCustomization";

        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";
        public const string Pages_Editions_MoveTenantsToAnotherEdition = "Pages.Editions.MoveTenantsToAnotherEdition";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";

    }
}