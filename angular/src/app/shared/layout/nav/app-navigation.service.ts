import { PermissionCheckerService } from '@abp/auth/permission-checker.service';
import { AppSessionService } from '@shared/common/session/app-session.service';

import { Injectable } from '@angular/core';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';

@Injectable()
export class AppNavigationService {

    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService
    ) {

    }

    getMenu(): AppMenu {
        return new AppMenu('MainMenu', 'MainMenu', [
            new AppMenuItem('Dashboard', 'Pages.Administration.Host.Dashboard', 'pi pi-home', '/app/admin/hostDashboard'),
            new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard', 'pi pi-home', '/app/main/dashboard'),
            new AppMenuItem('Dashboard', 'Pages.User', 'pi pi-home', '/app/main/user-dashboard'),
            // new AppMenuItem('Tenants', 'Pages.Tenants', 'flaticon-list-3', '/app/admin/tenants'),
            // new AppMenuItem('Editions', 'Pages.Editions', 'flaticon-app', '/app/admin/editions'),
            new AppMenuItem('Product', 'Pages.User', 'pi pi-list', '', [
                new AppMenuItem('AllProduct', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/all-product'),
                new AppMenuItem('Brand', 'Pages.User', 'pi pi-arrow-circle-right', '', [
                    new AppMenuItem('Yonex', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/product-brand/1'),
                    new AppMenuItem('Victor', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/product-brand/2'),
                    new AppMenuItem('Li-Ning', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/product-brand/3'),
                    new AppMenuItem('Mizuno', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/product-brand/8'),
                    new AppMenuItem('Fleet', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/product-brand/9'),
                    new AppMenuItem('Kawasaki', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/product-brand/10'),
                    new AppMenuItem('Apacs', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/product-brand/11'),
                ]),
                new AppMenuItem('Category', 'Pages.User', 'pi pi-arrow-circle-right', '', [
                    new AppMenuItem('Vợt cầu lông', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/product-category/1'),
                    new AppMenuItem('Áo', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/product-category/2'),
                    new AppMenuItem('Quần', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/product-category/3'),
                    new AppMenuItem('Giày', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/product-category/4'),
                    new AppMenuItem('Phụ kiện ', 'Pages.User', 'pi pi-arrow-circle-right', '/app/main/user/product-category/5'),
                ]),

            ]),
            new AppMenuItem('Hẹn lịch', 'Pages.User', 'pi pi-calendar-plus', '/app/main/user/booking'),
            new AppMenuItem('Đổi trả hàng', 'Pages.User', 'pi pi-replay', '/app/main/user/returnProd'),
            new AppMenuItem('Store', '', 'pi pi-list', '', [

                new AppMenuItem('Bookings', 'Pages.Bookings', 'pi pi-arrow-circle-right', '/app/main/common/bookings'),

                new AppMenuItem('Blogs', 'Pages.Blogs', 'pi pi-arrow-circle-right', '/app/main/common/blogs'),

                new AppMenuItem('Discounts', 'Pages.Discounts', 'pi pi-arrow-circle-right', '/app/main/common/discounts'),


                new AppMenuItem('Brands', 'Pages.Brands', 'pi pi-arrow-circle-right', '/app/main/entity/brands'),

                new AppMenuItem('Categories', 'Pages.Categories', 'pi pi-arrow-circle-right', '/app/main/common/categories'),


                new AppMenuItem('Products', 'Pages.Products', 'pi pi-arrow-circle-right', '/app/main/entity/products'),

                new AppMenuItem('ProductImages', 'Pages.ProductImages', 'pi pi-arrow-circle-right', '/app/main/common/productImages'),

                new AppMenuItem('Statuses', 'Pages.Statuses', 'pi pi-arrow-circle-right', '/app/main/common/statuses'),

                new AppMenuItem('Orders', 'Pages.Orders', 'pi pi-arrow-circle-right', '/app/main/purchase/orders'),

                new AppMenuItem('OrderItems', 'Pages.OrderItems', 'pi pi-arrow-circle-right', '/app/main/purchase/orderItems'),

                new AppMenuItem('ReturnProds', 'Pages.ReturnProds', 'pi pi-arrow-circle-right', '/app/main/purchase/returnProds'),

                new AppMenuItem('Carts', 'Pages.Carts', 'pi pi-arrow-circle-right', '/app/main/common/carts'),
            ]),


            new AppMenuItem('Administration', '', 'pi pi-star-o', '', [
                // new AppMenuItem('OrganizationUnits', 'Pages.Administration.OrganizationUnits', 'flaticon-map', '/app/admin/organization-units'),
                new AppMenuItem('Roles', 'Pages.Administration.Roles', 'flaticon-suitcase', '/app/admin/roles'),
                new AppMenuItem('Users', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/users'),
                new AppMenuItem('Languages', 'Pages.Administration.Languages', 'flaticon-tabs', '/app/admin/languages'),
                // new AppMenuItem('AuditLogs', 'Pages.Administration.AuditLogs', 'flaticon-folder-1', '/app/admin/auditLogs'),
                // new AppMenuItem('Maintenance', 'Pages.Administration.Host.Maintenance', 'flaticon-lock', '/app/admin/maintenance'),
                new AppMenuItem('Subscription', 'Pages.Administration.Tenant.SubscriptionManagement', 'flaticon-refresh', '/app/admin/subscription-management'),
                new AppMenuItem('VisualSettings', 'Pages.Administration.UiCustomization', 'flaticon-medical', '/app/admin/ui-customization'),
                new AppMenuItem('Settings', 'Pages.Administration.Host.Settings', 'flaticon-settings', '/app/admin/hostSettings'),
                new AppMenuItem('Settings', 'Pages.Administration.Tenant.Settings', 'flaticon-settings', '/app/admin/tenantSettings')
            ]),
            // new AppMenuItem('DemoUiComponents', 'Pages.DemoUiComponents', 'flaticon-shapes', '/app/admin/demo-ui-components')
        ]);
    }

    checkChildMenuItemPermission(menuItem): boolean {

        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName && this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                return true;
            } else if (subMenuItem.items && subMenuItem.items.length) {
                return this.checkChildMenuItemPermission(subMenuItem);
            }
        }

        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' && this._appSessionService.tenant && !this._appSessionService.tenant.edition) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }
}
