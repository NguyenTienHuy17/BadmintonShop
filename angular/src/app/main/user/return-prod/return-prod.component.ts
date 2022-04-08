import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditReturnProdDto, GetOrderForViewDto, OrdersServiceProxy, ReturnProdsServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-return-prod',
  templateUrl: './return-prod.component.html',
  styleUrls: ['./return-prod.component.css'],
  animations: [appModuleAnimation()]
})
export class ReturnProdComponent extends AppComponentBase implements OnInit {

  returnProd: CreateOrEditReturnProdDto = new CreateOrEditReturnProdDto();
  listOrder: GetOrderForViewDto[] = [];
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
      console.log(result)
    }
    )
  }

  save(): void {
    this._returnProdsServiceProxy.createOrEdit(this.returnProd)
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
      });
  }

}
