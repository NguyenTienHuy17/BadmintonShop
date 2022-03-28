import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { ReturnProdsComponent } from './purchase/returnProds/returnProds.component';
import { ViewReturnProdModalComponent } from './purchase/returnProds/view-returnProd-modal.component';
import { CreateOrEditReturnProdModalComponent } from './purchase/returnProds/create-or-edit-returnProd-modal.component';
import { ReturnProdOrderLookupTableModalComponent } from './purchase/returnProds/returnProd-order-lookup-table-modal.component';

import { OrderItemsComponent } from './purchase/orderItems/orderItems.component';
import { ViewOrderItemModalComponent } from './purchase/orderItems/view-orderItem-modal.component';
import { CreateOrEditOrderItemModalComponent } from './purchase/orderItems/create-or-edit-orderItem-modal.component';
import { OrderItemProductLookupTableModalComponent } from './purchase/orderItems/orderItem-product-lookup-table-modal.component';
import { OrderItemOrderLookupTableModalComponent } from './purchase/orderItems/orderItem-order-lookup-table-modal.component';

import { OrdersComponent } from './purchase/orders/orders.component';
import { ViewOrderModalComponent } from './purchase/orders/view-order-modal.component';
import { CreateOrEditOrderModalComponent } from './purchase/orders/create-or-edit-order-modal.component';
import { OrderStatusLookupTableModalComponent } from './purchase/orders/order-status-lookup-table-modal.component';
import { OrderDiscountLookupTableModalComponent } from './purchase/orders/order-discount-lookup-table-modal.component';

import { StatusesComponent } from './common/statuses/statuses.component';
import { ViewStatusModalComponent } from './common/statuses/view-status-modal.component';
import { CreateOrEditStatusModalComponent } from './common/statuses/create-or-edit-status-modal.component';

import { ProductImagesComponent } from './common/productImages/productImages.component';
import { ViewProductImageModalComponent } from './common/productImages/view-productImage-modal.component';
import { CreateOrEditProductImageModalComponent } from './common/productImages/create-or-edit-productImage-modal.component';
import { ProductImageProductLookupTableModalComponent } from './common/productImages/productImage-product-lookup-table-modal.component';
import { ProductImageImageLookupTableModalComponent } from './common/productImages/productImage-image-lookup-table-modal.component';

import { ColorItemsComponent } from './common/colorItems/colorItems.component';
import { ViewColorItemModalComponent } from './common/colorItems/view-colorItem-modal.component';
import { CreateOrEditColorItemModalComponent } from './common/colorItems/create-or-edit-colorItem-modal.component';
import { ColorItemProductLookupTableModalComponent } from './common/colorItems/colorItem-product-lookup-table-modal.component';
import { ColorItemColorLookupTableModalComponent } from './common/colorItems/colorItem-color-lookup-table-modal.component';

import { SizeItemsComponent } from './common/sizeItems/sizeItems.component';
import { ViewSizeItemModalComponent } from './common/sizeItems/view-sizeItem-modal.component';
import { CreateOrEditSizeItemModalComponent } from './common/sizeItems/create-or-edit-sizeItem-modal.component';
import { SizeItemProductLookupTableModalComponent } from './common/sizeItems/sizeItem-product-lookup-table-modal.component';
import { SizeItemSizeLookupTableModalComponent } from './common/sizeItems/sizeItem-size-lookup-table-modal.component';

import { ProductsComponent } from './entity/products/products.component';
import { ViewProductModalComponent } from './entity/products/view-product-modal.component';
import { CreateOrEditProductModalComponent } from './entity/products/create-or-edit-product-modal.component';
import { ProductImageLookupTableModalComponent } from './entity/products/product-image-lookup-table-modal.component';
import { ProductBrandLookupTableModalComponent } from './entity/products/product-brand-lookup-table-modal.component';
import { ProductCategoryLookupTableModalComponent } from './entity/products/product-category-lookup-table-modal.component';

import { SizesComponent } from './common/sizes/sizes.component';
import { ViewSizeModalComponent } from './common/sizes/view-size-modal.component';
import { CreateOrEditSizeModalComponent } from './common/sizes/create-or-edit-size-modal.component';

import { ColorsComponent } from './common/colors/colors.component';
import { ViewColorModalComponent } from './common/colors/view-color-modal.component';
import { CreateOrEditColorModalComponent } from './common/colors/create-or-edit-color-modal.component';

import { CategoriesComponent } from './common/categories/categories.component';
import { ViewCategoryModalComponent } from './common/categories/view-category-modal.component';
import { CreateOrEditCategoryModalComponent } from './common/categories/create-or-edit-category-modal.component';
import { CategoryImageLookupTableModalComponent } from './common/categories/category-image-lookup-table-modal.component';

import { BrandsComponent } from './entity/brands/brands.component';
import { ViewBrandModalComponent } from './entity/brands/view-brand-modal.component';
import { CreateOrEditBrandModalComponent } from './entity/brands/create-or-edit-brand-modal.component';
import { BrandImageLookupTableModalComponent } from './entity/brands/brand-image-lookup-table-modal.component';

import { ImagesComponent } from './common/images/images.component';
import { ViewImageModalComponent } from './common/images/view-image-modal.component';
import { CreateOrEditImageModalComponent } from './common/images/create-or-edit-image-modal.component';

import { DiscountsComponent } from './common/discounts/discounts.component';
import { ViewDiscountModalComponent } from './common/discounts/view-discount-modal.component';
import { CreateOrEditDiscountModalComponent } from './common/discounts/create-or-edit-discount-modal.component';

import { BlogsComponent } from './common/blogs/blogs.component';
import { ViewBlogModalComponent } from './common/blogs/view-blog-modal.component';
import { CreateOrEditBlogModalComponent } from './common/blogs/create-or-edit-blog-modal.component';

import { BookingsComponent } from './common/bookings/bookings.component';
import { ViewBookingModalComponent } from './common/bookings/view-booking-modal.component';
import { CreateOrEditBookingModalComponent } from './common/bookings/create-or-edit-booking-modal.component';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { PaginatorModule } from 'primeng/paginator';
import { EditorModule } from 'primeng/editor';
import { InputMaskModule } from 'primeng/inputmask';import { FileUploadModule } from 'primeng/fileupload';
import { TableModule } from 'primeng/table';

import { UtilsModule } from '@shared/utils/utils.module';
import { CountoModule } from 'angular2-counto';
import { ModalModule, TabsModule, TooltipModule, BsDropdownModule, PopoverModule } from 'ngx-bootstrap';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainRoutingModule } from './main-routing.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { BsDatepickerModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';

NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales();

@NgModule({
    imports: [
		FileUploadModule,
		AutoCompleteModule,
		PaginatorModule,
		EditorModule,
		InputMaskModule,		TableModule,

        CommonModule,
        FormsModule,
        ModalModule,
        TabsModule,
        TooltipModule,
        AppCommonModule,
        UtilsModule,
        MainRoutingModule,
        CountoModule,
        NgxChartsModule,
        BsDatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        PopoverModule.forRoot()
    ],
    declarations: [
		ReturnProdsComponent,

		ViewReturnProdModalComponent,
		CreateOrEditReturnProdModalComponent,
    ReturnProdOrderLookupTableModalComponent,
		OrderItemsComponent,

		ViewOrderItemModalComponent,
		CreateOrEditOrderItemModalComponent,
    OrderItemProductLookupTableModalComponent,
    OrderItemOrderLookupTableModalComponent,
		OrdersComponent,

		ViewOrderModalComponent,
		CreateOrEditOrderModalComponent,
    OrderStatusLookupTableModalComponent,
    OrderDiscountLookupTableModalComponent,
		StatusesComponent,

		ViewStatusModalComponent,
		CreateOrEditStatusModalComponent,
		ProductImagesComponent,

		ViewProductImageModalComponent,
		CreateOrEditProductImageModalComponent,
    ProductImageProductLookupTableModalComponent,
    ProductImageImageLookupTableModalComponent,
		ColorItemsComponent,

		ViewColorItemModalComponent,
		CreateOrEditColorItemModalComponent,
    ColorItemProductLookupTableModalComponent,
    ColorItemColorLookupTableModalComponent,
		SizeItemsComponent,

		ViewSizeItemModalComponent,
		CreateOrEditSizeItemModalComponent,
    SizeItemProductLookupTableModalComponent,
    SizeItemSizeLookupTableModalComponent,
		ProductsComponent,

		ViewProductModalComponent,
		CreateOrEditProductModalComponent,
    ProductImageLookupTableModalComponent,
    ProductBrandLookupTableModalComponent,
    ProductCategoryLookupTableModalComponent,
		SizesComponent,

		ViewSizeModalComponent,
		CreateOrEditSizeModalComponent,
		ColorsComponent,

		ViewColorModalComponent,
		CreateOrEditColorModalComponent,
		CategoriesComponent,

		ViewCategoryModalComponent,
		CreateOrEditCategoryModalComponent,
    CategoryImageLookupTableModalComponent,
		BrandsComponent,

		ViewBrandModalComponent,
		CreateOrEditBrandModalComponent,
    BrandImageLookupTableModalComponent,
		ImagesComponent,

		ViewImageModalComponent,
		CreateOrEditImageModalComponent,
		DiscountsComponent,

		ViewDiscountModalComponent,
		CreateOrEditDiscountModalComponent,
		BlogsComponent,

		ViewBlogModalComponent,
		CreateOrEditBlogModalComponent,
		BookingsComponent,

		ViewBookingModalComponent,
		CreateOrEditBookingModalComponent,
        DashboardComponent
    ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
    ]
})
export class MainModule { }
