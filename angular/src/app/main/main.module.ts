import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { CartsComponent } from './common/carts/carts.component';
import { ViewCartModalComponent } from './common/carts/view-cart-modal.component';
import { CreateOrEditCartModalComponent } from './common/carts/create-or-edit-cart-modal.component';
import { CartProductLookupTableModalComponent } from './common/carts/cart-product-lookup-table-modal.component';

import { ReturnProdsComponent } from './purchase/returnProds/returnProds.component';
import { ViewReturnProdModalComponent } from './purchase/returnProds/view-returnProd-modal.component';
import { CreateOrEditReturnProdModalComponent } from './purchase/returnProds/create-or-edit-returnProd-modal.component';
import { ReturnProdOrderLookupTableModalComponent } from './purchase/returnProds/returnProd-order-lookup-table-modal.component';

import { OrderItemsComponent } from './purchase/orderItems/orderItems.component';
import { ViewOrderItemModalComponent } from './purchase/orderItems/view-orderItem-modal.component';
import { CreateOrEditOrderItemModalComponent } from './purchase/orderItems/create-or-edit-orderItem-modal.component';

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

import { ProductsComponent } from './entity/products/products.component';
import { ViewProductModalComponent } from './entity/products/view-product-modal.component';
import { CreateOrEditProductModalComponent } from './entity/products/create-or-edit-product-modal.component';

import { CategoriesComponent } from './common/categories/categories.component';
import { ViewCategoryModalComponent } from './common/categories/view-category-modal.component';
import { CreateOrEditCategoryModalComponent } from './common/categories/create-or-edit-category-modal.component';
import { CategoryImageLookupTableModalComponent } from './common/categories/category-image-lookup-table-modal.component';

import { BrandsComponent } from './entity/brands/brands.component';
import { ViewBrandModalComponent } from './entity/brands/view-brand-modal.component';
import { CreateOrEditBrandModalComponent } from './entity/brands/create-or-edit-brand-modal.component';

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
import { InputMaskModule } from 'primeng/inputmask'; import { FileUploadModule } from 'primeng/fileupload';
import { TableModule } from 'primeng/table';

import { UtilsModule } from '@shared/utils/utils.module';
import { CountoModule } from 'angular2-counto';
import { ModalModule, TabsModule, TooltipModule, BsDropdownModule, PopoverModule } from 'ngx-bootstrap';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainRoutingModule } from './main-routing.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { BsDatepickerModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { ImageCropperModule } from 'ngx-image-cropper';
import { NewUploadImageComponent } from './newUploadImage/newUploadImage.component';
import { ShowImageModalComponent } from './newUploadImage/showImageModal/showImageModal.component';
import { DragDropDirective } from './newUploadImage/drag-and-drop.directive';
import { UploadSingleImageComponent } from './uploadSingleImage/uploadSingleImage.component';
import { NewUploadImageService } from './newUploadImage/newUploadImage.service';
import { UploadSingleImageService } from './uploadSingleImage/uploadSingleImage.service';;
import { ProductDetailComponent } from './entity/product-detail/product-detail.component'
	;
import { CartDetailComponent } from './common/cart-detail/cart-detail.component'
	;
import { OrrderDetailModalComponent } from './purchase/orrder-detail-modal/orrder-detail-modal.component'
	;
import { ProductBrandComponent } from './user/product-brand/product-brand.component';
import { ProductCategoryComponent } from './user/product-category/product-category.component'
	;
import { AllProductComponent } from './user/all-product/all-product.component'
	;
import { BookingComponent } from './user/booking/booking.component'
	;
import { ReturnProdComponent } from './user/return-prod/return-prod.component';
import { AdminOrderDetailModalComponent } from './purchase/admin-order-detail-modal/admin-order-detail-modal.component'
import { NgImageSliderModule } from 'ng-image-slider';
import { UserSignUpComponent } from './user/user-sign-up/user-sign-up.component';
import { AppModule } from '..';
import { DataViewModule } from 'primeng/dataview';
NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales();

@NgModule({
	imports: [
		FileUploadModule,
		AutoCompleteModule,
		PaginatorModule,
		EditorModule,
		InputMaskModule,
		TableModule,
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
		PopoverModule.forRoot(),
		ImageCropperModule,
		DataViewModule,
		NgImageSliderModule,
		AppModule
	],
	declarations: [
		CartsComponent,

		ViewCartModalComponent,
		CreateOrEditCartModalComponent,
		CartProductLookupTableModalComponent,
		ReturnProdsComponent,

		ViewReturnProdModalComponent,
		CreateOrEditReturnProdModalComponent,
		ReturnProdOrderLookupTableModalComponent,
		OrderItemsComponent,

		ViewOrderItemModalComponent,
		CreateOrEditOrderItemModalComponent,
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
		ProductsComponent,

		ViewProductModalComponent,
		CreateOrEditProductModalComponent,
		CategoriesComponent,

		ViewCategoryModalComponent,
		CreateOrEditCategoryModalComponent,
		CategoryImageLookupTableModalComponent,
		BrandsComponent,

		ViewBrandModalComponent,
		CreateOrEditBrandModalComponent,
		DiscountsComponent,

		ViewDiscountModalComponent,
		CreateOrEditDiscountModalComponent,
		BlogsComponent,

		ViewBlogModalComponent,
		CreateOrEditBlogModalComponent,
		BookingsComponent,

		ViewBookingModalComponent,
		CreateOrEditBookingModalComponent,
		DashboardComponent,
		NewUploadImageComponent,
		ShowImageModalComponent,
		DragDropDirective,
		UploadSingleImageComponent
		,
		ProductDetailComponent
		,
		CartDetailComponent
		,
		OrrderDetailModalComponent
		,
		ProductBrandComponent,
		ProductCategoryComponent,
		AllProductComponent,
		BookingComponent,
		ReturnProdComponent,
		AdminOrderDetailModalComponent
		,
		UserSignUpComponent
	],
	providers: [
		NewUploadImageService,
		UploadSingleImageService,
		{ provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
		{ provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
		{ provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }

	]
})
export class MainModule { }
