import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditOrderDto, CreateOrEditOrderItemDto, Discount, DiscountsServiceProxy, GetCartForViewDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap';

@Component({
  selector: 'orrder-detail-modal',
  templateUrl: './orrder-detail-modal.component.html',
  styleUrls: ['./orrder-detail-modal.component.css']
})
export class OrrderDetailModalComponent extends AppComponentBase implements OnInit {
  @ViewChild('purchaseModal', { static: true }) modal: ModalDirective;
  @Input() listCart: GetCartForViewDto[];  
  @Input() totalPrice: number;  
  active = false;
  saving = false;
  defaultRouter ='../../../assets/common/images/';
  order: CreateOrEditOrderDto = new CreateOrEditOrderDto();
  orderItem: CreateOrEditOrderItemDto = new CreateOrEditOrderItemDto();
  discountCode: string;
  discount: Discount = new Discount();
  actualPrice: number;
  constructor(injector: Injector,
    private _discountsServiceProxy: DiscountsServiceProxy
    ) { 
    super(injector);
    this.actualPrice = 0;
  }

  ngOnInit() {
  }
  show(){
    this.modal.show();
    this.actualPrice = this.totalPrice
  }

  close(): void {
    this.active = false;
    this.modal.hide();
  }

  save(){
    var randomChars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
      var result = '';
      for ( var i = 0; i < 5; i++ ) {
          result += randomChars.charAt(Math.floor(Math.random() * randomChars.length));
      }
    this.order.orderCode = result;
  }

  getDiscountId(){
    this._discountsServiceProxy.getDiscount(this.discountCode).subscribe(result =>{
        this.discount = result
        if(this.discount.discountNum != null){
          this.actualPrice -= this.totalPrice * (this.discount.discountNum / 100)
        }
      }
    )
  }

}
