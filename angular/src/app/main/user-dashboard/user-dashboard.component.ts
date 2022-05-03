import { Options } from "@angular-slider/ngx-slider";
import { Component, Injector, OnInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AppComponentBase } from "@shared/common/app-component-base";
import {
    BlogsServiceProxy,
    Brand,
    BrandsServiceProxy,
    CategoriesServiceProxy,
    Category,
    GetBlogForViewDto,
    ProductDto,
    ProductsServiceProxy,
} from "@shared/service-proxies/service-proxies";
import { LazyLoadEvent } from "primeng/api";
import { Paginator } from "primeng/primeng";
import { Table } from "primeng/table";
import { ViewBlogModalComponent } from "../common/blogs/view-blog-modal.component";

@Component({
    selector: "app-user-dashboard",
    templateUrl: "./user-dashboard.component.html",
    styleUrls: ["./user-dashboard.component.css"],
    animations: [appModuleAnimation()]
})
export class UserDashboardComponent extends AppComponentBase implements OnInit {
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('viewBlogModalComponent', { static: true }) viewBlogModal: ViewBlogModalComponent;
    defaultRouter = '../../../../assets/common/images/';
    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    madeInFilter = '';
    codeFilter = '';
    maxPriceFilter: number = 8000000;
    maxPriceFilterEmpty: number = 10000000;
    minPriceFilter: number = 100000;
    minPriceFilterEmpty: number = 0;
    maxInStockFilter: number;
    maxInStockFilterEmpty: number;
    minInStockFilter: number;
    minInStockFilterEmpty: number;
    descriptionFilter = '';
    titleFilter = '';
    imageNameFilter = '';
    brandNameFilter = '';
    categoryNameFilter = '';
    options: Options = {
        floor: 0,
        ceil: 10000000
    };
    listCategory: Category[] = [];
    listBrand: Brand[] = [];
    listBlog: GetBlogForViewDto[] = [];
    bannerLeftUrl = '../../../../assets/common/images/bannerLeft.jpg';
    bannerTopUrl = '../../../../assets/common/images/bg-home-video-01.jpg';
    constructor(
        injector: Injector,
        private _productsServiceProxy: ProductsServiceProxy,
        private router: Router,
        private _brandsServiceProxy: BrandsServiceProxy,
        private _categoriesServiceProxy: CategoriesServiceProxy,
        private _blogsServiceProxy: BlogsServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit() {
        this._brandsServiceProxy.getAllForProduct().subscribe(result => {
            this.listBrand = result
        });
        this._categoriesServiceProxy.getAllForProduct().subscribe(result => {
            this.listCategory = result
        });
        this._blogsServiceProxy.getAllBlogForView().subscribe(result => {
            this.listBlog = result
        })
    }

    getProducts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productsServiceProxy.getAllProduct(
            this.filterText,
            this.nameFilter,
            this.madeInFilter,
            this.codeFilter,
            this.maxPriceFilter == null ? this.maxPriceFilterEmpty : this.maxPriceFilter,
            this.minPriceFilter == null ? this.minPriceFilterEmpty : this.minPriceFilter,
            this.maxInStockFilter == null ? this.maxInStockFilterEmpty : this.maxInStockFilter,
            this.minInStockFilter == null ? this.minInStockFilterEmpty : this.minInStockFilter,
            this.descriptionFilter,
            this.titleFilter,
            this.imageNameFilter,
            this.brandNameFilter,
            this.categoryNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    detail(product: ProductDto) {
        this.router.navigate(['/app/main/entity/product-detail', product.id, product.name]);  // define your component where you want to go
    }

}
