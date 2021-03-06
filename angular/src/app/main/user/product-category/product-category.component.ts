import { LabelType, Options } from '@angular-slider/ngx-slider';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProductDto, ProductsServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent, Paginator } from 'primeng/primeng';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-product-category',
  templateUrl: './product-category.component.html',
  styleUrls: ['./product-category.component.css'],
  animations: [appModuleAnimation()]
})
export class ProductCategoryComponent extends AppComponentBase implements OnInit {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
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
  categoryId: number;
  options: Options = {
    floor: 0,
    ceil: 10000000,
    translate: (value: number, label: LabelType): string => {
      switch (label) {
        case LabelType.Low:
          return value.toLocaleString();
        case LabelType.High:
          return value.toLocaleString();
        default:
          return value.toLocaleString();
      }
    }
  };
  bannerUrl = '../../../../assets/common/images/bannerLeft.jpg';
  constructor(
    injector: Injector,
    private _productsServiceProxy: ProductsServiceProxy,
    private router: Router,
    private _activatedRoute: ActivatedRoute,
  ) {
    super(injector);
  }

  ngOnInit() {
    this._activatedRoute.params.subscribe((params: Params) => {
      this.categoryId = params['categoryId'];
    });
    this.router.events.subscribe((event) => {
      this.getProductsOnLoading()
    });
  }

  getProductsOnLoading() {
    this.primengTableHelper.showLoadingIndicator();

    this._productsServiceProxy.getAllByCategoryId(
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
      0,
      12,
      this.categoryId
    ).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }

  getProducts(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    this._productsServiceProxy.getAllByCategoryId(
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
      this.primengTableHelper.getMaxResultCount(this.paginator, event),
      this.categoryId
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
