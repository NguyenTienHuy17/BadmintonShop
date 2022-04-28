import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditReturnProdDto, GetOrderForViewDto, OrdersServiceProxy, ReturnProdsServiceProxy } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';

@Component({
  selector: 'app-return-prod',
  templateUrl: './return-prod.component.html',
  styleUrls: ['./return-prod.component.css'],
  animations: [appModuleAnimation()]
})
export class ReturnProdComponent extends AppComponentBase implements OnInit {

  returnProd: CreateOrEditReturnProdDto = new CreateOrEditReturnProdDto();
  listOrder: GetOrderForViewDto[] = [];
  paginator: any;
  constructor(
    injector: Injector,
    private _returnProdsServiceProxy: ReturnProdsServiceProxy,
    private _ordersServiceProxy: OrdersServiceProxy,
  ) {
    super(injector);
    this.returnProd.orderId = 0;
  }

  ngOnInit() {
    this._ordersServiceProxy.getAllUserOrder().subscribe(result => {
      this.listOrder = result
    }
    )
  }

  save(): void {
    this._returnProdsServiceProxy.createOrEdit(this.returnProd)
      .subscribe(() => {
        this.notify.info(this.l('CreateReturnProdSuccessfully'));
        window.location.reload();
      });
  }

  getReturnProds(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    this._returnProdsServiceProxy.getAllForUser().subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }

}
