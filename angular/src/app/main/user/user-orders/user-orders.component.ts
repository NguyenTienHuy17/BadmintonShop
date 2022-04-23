import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetOrderForViewDto, OrdersServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-user-orders',
  templateUrl: './user-orders.component.html',
  styleUrls: ['./user-orders.component.css'],
  animations: [appModuleAnimation()]
})
export class UserOrdersComponent extends AppComponentBase implements OnInit {
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('dataTable', { static: true }) dataTable: Table;

  listOrder: GetOrderForViewDto[] = [];
  advancedFiltersAreShown = false;
  filterText = '';
  orderCodeFilter = '';
  totalPriceFilter = '';
  shippingAddressFilter = '';
  shippingNumberFilter = '';
  maxDiscountAmountFilter: number;
  maxDiscountAmountFilterEmpty: number;
  minDiscountAmountFilter: number;
  minDiscountAmountFilterEmpty: number;
  maxActualPriceFilter: number;
  maxActualPriceFilterEmpty: number;
  minActualPriceFilter: number;
  minActualPriceFilterEmpty: number;
  statusNameFilter = '';
  discountDiscountCodeFilter = '';


  constructor(
    injector: Injector,
    private _ordersServiceProxy: OrdersServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit() {
  }

  getOrders(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    this._ordersServiceProxy.getAllOrderForUser(
      this.filterText,
      this.orderCodeFilter,
      this.totalPriceFilter,
      this.shippingAddressFilter,
      this.shippingNumberFilter,
      this.maxDiscountAmountFilter == null ? this.maxDiscountAmountFilterEmpty : this.maxDiscountAmountFilter,
      this.minDiscountAmountFilter == null ? this.minDiscountAmountFilterEmpty : this.minDiscountAmountFilter,
      this.maxActualPriceFilter == null ? this.maxActualPriceFilterEmpty : this.maxActualPriceFilter,
      this.minActualPriceFilter == null ? this.minActualPriceFilterEmpty : this.minActualPriceFilter,
      this.statusNameFilter,
      this.discountDiscountCodeFilter,
      this.primengTableHelper.getSorting(this.dataTable),
      this.primengTableHelper.getSkipCount(this.paginator, event),
      this.primengTableHelper.getMaxResultCount(this.paginator, event)
    ).subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      console.log(result)
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }

}
