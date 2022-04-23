import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetOrderForViewDto, GetOrderItemForViewDto, OrderDto, OrderItemsServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap';

@Component({
  selector: 'adminOrderDetailModal',
  templateUrl: './admin-order-detail-modal.component.html',
  styleUrls: ['./admin-order-detail-modal.component.css']
})
export class AdminOrderDetailModalComponent extends AppComponentBase implements OnInit {

  @ViewChild('detailModal', { static: true }) modal: ModalDirective;
  listOrderItem: GetOrderItemForViewDto[] = [];
  item: GetOrderForViewDto;
  constructor(
    injector: Injector,
    private _orderItemsServiceProxy: OrderItemsServiceProxy,
  ) {
    super(injector);
    this.item = new GetOrderForViewDto();
    this.item.order = new OrderDto();
  }

  ngOnInit() {
  }
  show(orderId: number, item: GetOrderForViewDto) {
    this.modal.show();
    this.item = item;
    this._orderItemsServiceProxy.getOrderItemByOrderId(orderId).subscribe(result => {
      this.listOrderItem = result;
    });
  }

  close(): void {
    this.modal.hide();
  }

}
