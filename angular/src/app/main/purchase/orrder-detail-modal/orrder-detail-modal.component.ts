import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { async } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CartsServiceProxy, CreateOrEditOrderDto, CreateOrEditOrderItemDto, Discount, DiscountsServiceProxy, GetCartForViewDto, OrderItemsServiceProxy, OrdersServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap';
import Swal from 'sweetalert2';

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
  defaultRouter = '../../../assets/common/images/';
  order: CreateOrEditOrderDto = new CreateOrEditOrderDto();
  orderItem: CreateOrEditOrderItemDto = new CreateOrEditOrderItemDto();
  discountCode: string;
  discount: Discount = new Discount();
  actualPrice: number;
  orderId: number;
  constructor(injector: Injector,
    private _discountsServiceProxy: DiscountsServiceProxy,
    private _ordersServiceProxy: OrdersServiceProxy,
    private _orderItemsServiceProxy: OrderItemsServiceProxy,
    private _cartsServiceProxy: CartsServiceProxy,
    private router: Router
  ) {
    super(injector);
    this.actualPrice = 0;
    this.orderId = 0;
  }

  ngOnInit() {
  }
  show() {
    this.modal.show();
    this.actualPrice = this.totalPrice
  }

  close(): void {
    this.active = false;
    this.modal.hide();
  }

  save() {
    var randomChars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var result = '';
    for (var i = 0; i < 5; i++) {
      result += randomChars.charAt(Math.floor(Math.random() * randomChars.length));
    }
    this.order.statusId = 1;
    this.order.orderCode = result;
    this.order.totalPrice = this.totalPrice;
    this.order.actualPrice = this.actualPrice;
    this.order.discountAmount = this.totalPrice - this.actualPrice;
    const swalWithBootstrapButtons = Swal.mixin({
      customClass: {
        confirmButton: 'btn btn-success btn_success',
        cancelButton: 'btn btn-primary btn_cancel',
      },
      buttonsStyling: false,
    })

    swalWithBootstrapButtons
      .fire({
        title: this.l('AreYouSureToPurchase'),
        // text: this.l('Hãy chắc chắn bạn muốn xóa'),
        imageUrl: this.appRootUrl() + 'assets/common/images/danger.PNG',
        imageWidth: 60,
        imageHeight: 50,
        showCancelButton: true,
        showCloseButton: true,
        confirmButtonText: '<i class="fas fa-check"></i>' + this.l('Purchase'),
        cancelButtonText: '<i class="fas fa-trash"></i>' + this.l('Cancel'),
      })
      .then(async (result) => {
        if (result.value) {
          this.orderId = await this._ordersServiceProxy.createAndGetId(this.order).toPromise()
          this.listCart.forEach(x => {
            this.orderItem.orderId = this.orderId;
            this.orderItem.productId = x.cart.productId;
            this.orderItem.quantity = x.cart.quantity;
            this._cartsServiceProxy.delete(x.cart.id).toPromise();
            this._orderItemsServiceProxy.createOrEdit(this.orderItem).toPromise();
            this.notify.success(this.l('SuccessfullyPurchased'));
            this.close();
            this.router.navigate(['/app/main/user-dashboard']);  // define your component where you want to go
          });
        }
      })
    // this.message.confirm(
    //   '',
    //   this.l('AreYouSureToPurchase'),
    //   async (isConfirmed) => {
    //     if (isConfirmed) {
    //       this.orderId = await this._ordersServiceProxy.createAndGetId(this.order).toPromise()
    //       this.listCart.forEach(x => {
    //         this.orderItem.orderId = this.orderId;
    //         this.orderItem.productId = x.cart.productId;
    //         this.orderItem.quantity = x.cart.quantity;
    //         this._cartsServiceProxy.delete(x.cart.id).toPromise();
    //         this._orderItemsServiceProxy.createOrEdit(this.orderItem).toPromise();
    //         this.notify.success(this.l('SuccessfullyPurchased'));
    //         this.close();
    //         this.router.navigate(['/app/main/user-dashboard']);  // define your component where you want to go
    //       });
    //     }
    //   }
    // );
  }

  getDiscountId() {
    if (this.discountCode != "") {
      this._discountsServiceProxy.getDiscount(this.discountCode).subscribe(result => {
        this.discount = result
        if (this.discount.discountNum != null) {
          this.actualPrice -= this.totalPrice * (this.discount.discountNum / 100)
        }
      }
      )
    }
    else {
      this.actualPrice = this.totalPrice
    }
  }

}
