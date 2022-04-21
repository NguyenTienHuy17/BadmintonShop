import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CartsComponent } from './common/carts/carts.component';
import { ReturnProdsComponent } from './purchase/returnProds/returnProds.component';
import { OrderItemsComponent } from './purchase/orderItems/orderItems.component';
import { OrdersComponent } from './purchase/orders/orders.component';
import { StatusesComponent } from './common/statuses/statuses.component';
import { ProductImagesComponent } from './common/productImages/productImages.component';
import { ProductsComponent } from './entity/products/products.component';
import { CategoriesComponent } from './common/categories/categories.component';
import { BrandsComponent } from './entity/brands/brands.component';
import { DiscountsComponent } from './common/discounts/discounts.component';
import { BlogsComponent } from './common/blogs/blogs.component';
import { BookingsComponent } from './common/bookings/bookings.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserDashboardComponent } from './user-dashboard/user-dashboard.component';
import { ProductDetailComponent } from './entity/product-detail/product-detail.component';
import { CartDetailComponent } from './common/cart-detail/cart-detail.component';
import { ProductBrandComponent } from './user/product-brand/product-brand.component';
import { ProductCategoryComponent } from './user/product-category/product-category.component';
import { AllProductComponent } from './user/all-product/all-product.component';
import { BookingComponent } from './user/booking/booking.component';
import { ReturnProdComponent } from './user/return-prod/return-prod.component';
import { UserSignUpComponent } from './user/user-sign-up/user-sign-up.component';
import { AboutUsComponent } from './about-us/about-us.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'common/carts', component: CartsComponent, data: { permission: 'Pages.Carts' } },
                    { path: 'purchase/returnProds', component: ReturnProdsComponent, data: { permission: 'Pages.ReturnProds' } },
                    { path: 'purchase/orderItems', component: OrderItemsComponent, data: { permission: 'Pages.OrderItems' } },
                    { path: 'purchase/orders', component: OrdersComponent, data: { permission: 'Pages.Orders' } },
                    { path: 'common/statuses', component: StatusesComponent, data: { permission: 'Pages.Statuses' } },
                    { path: 'common/productImages', component: ProductImagesComponent, data: { permission: 'Pages.ProductImages' } },
                    { path: 'entity/products', component: ProductsComponent, data: { permission: 'Pages.Products' } },
                    { path: 'entity/product-detail/:id/:name', component: ProductDetailComponent, data: { permission: 'Pages.User' } },
                    { path: 'common/categories', component: CategoriesComponent, data: { permission: 'Pages.Categories' } },
                    { path: 'entity/brands', component: BrandsComponent, data: { permission: 'Pages.Brands' } },
                    { path: 'common/discounts', component: DiscountsComponent, data: { permission: 'Pages.Discounts' } },
                    { path: 'common/blogs', component: BlogsComponent, data: { permission: 'Pages.Blogs' } },
                    { path: 'common/bookings', component: BookingsComponent, data: { permission: 'Pages.Bookings' } },
                    { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Tenant.Dashboard' } },
                    { path: 'user-dashboard', component: UserDashboardComponent, data: { permission: 'Pages.User' } },
                    { path: 'cart-detail', component: CartDetailComponent, data: { permission: 'Pages.User' } },
                    { path: 'user/all-product', component: AllProductComponent, data: { permission: 'Pages.User' } },
                    { path: 'user/product-brand/:brandId', component: ProductBrandComponent, data: { permission: 'Pages.User' } },
                    { path: 'user/booking', component: BookingComponent, data: { permission: 'Pages.User' } },
                    { path: 'user/returnProd', component: ReturnProdComponent, data: { permission: 'Pages.User' } },
                    { path: 'user/signIn', component: UserSignUpComponent, data: { permission: '' } },
                    { path: 'about-us', component: AboutUsComponent, data: { permission: '' } },
                    { path: 'user/product-category/:categoryId', component: ProductCategoryComponent, data: { permission: 'Pages.User' } }
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class MainRoutingModule { }
