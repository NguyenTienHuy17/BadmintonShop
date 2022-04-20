import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProductDto, ProductsServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent, Paginator } from 'primeng/primeng';
import { Table } from 'primeng/table';


@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css'],
  animations: [appModuleAnimation()]
})
export class ProductListComponent extends AppComponentBase implements OnInit {

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  defaultRouter = '../../../../assets/common/images/';
  advancedFiltersAreShown = false;
  filterText = '';
  nameFilter = '';
  madeInFilter = '';
  codeFilter = '';
  maxPriceFilter: number;
  maxPriceFilterEmpty: number;
  minPriceFilter: number;
  minPriceFilterEmpty: number;
  maxInStockFilter: number;
  maxInStockFilterEmpty: number;
  minInStockFilter: number;
  minInStockFilterEmpty: number;
  descriptionFilter = '';
  titleFilter = '';
  imageNameFilter = '';
  brandNameFilter = '';
  categoryNameFilter = '';
  constructor(
    injector: Injector,
    private _productsServiceProxy: ProductsServiceProxy,
    private router: Router
  ) {
    super(injector);
  }

  ngOnInit() {
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
      console.log(result.items)
      this.primengTableHelper.hideLoadingIndicator();
    });
  }

  detail(product: ProductDto) {
    this.router.navigate(['/account/product-detail', product.id, product.name]);  // define your component where you want to go
  }

}
