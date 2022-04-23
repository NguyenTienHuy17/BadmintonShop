import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { async } from '@angular/core/testing';
import { OrrderDetailModalComponent } from '@app/main/purchase/orrder-detail-modal/orrder-detail-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CartsServiceProxy, GetCartForViewDto, GetProductForViewDto, ProductsServiceProxy } from '@shared/service-proxies/service-proxies';
import Swal from 'sweetalert2';
import { runInThisContext } from 'vm';

@Component({
  selector: '[cartDetails]',
  templateUrl: './cart-detail.component.html',
  styleUrls: ['./cart-detail.component.css'],
  animations: [appModuleAnimation()]
})
export class CartDetailComponent extends AppComponentBase implements OnInit {
  @ViewChild('orrderDetailModal', { static: true }) orrderDetailModal: OrrderDetailModalComponent;

  listCart: GetCartForViewDto[];
  defaultRouter = '../../../../assets/common/images/';
  totalPrice: number;
  product: GetProductForViewDto = new GetProductForViewDto();
  constructor(injector: Injector,
    private _cartsServiceProxy: CartsServiceProxy,
    private _productsServiceProxy: ProductsServiceProxy,
  ) {
    super(injector);
    this.listCart = [];
  }

  ngOnInit() {
    this.getAllCart();
  }

  getAllCart() {
    this.totalPrice = 0;
    this._cartsServiceProxy.getAllForCart().subscribe(result => {
      this.listCart = result;
      this.listCart.forEach(x => {
        this.totalPrice += x.productPrice * x.cart.quantity
      });
    });
  }

  async add(cart: GetCartForViewDto) {
    this.totalPrice = 0;
    this.product = await this._productsServiceProxy.getProductForView(cart.cart.productId, cart.productName).toPromise()
    if (cart.cart.quantity < this.product.product.inStock) {
      cart.cart.quantity += 1;
      this.listCart.forEach(x => {
        this.totalPrice += x.productPrice * x.cart.quantity
      });
    }

  }

  minus(cart: GetCartForViewDto) {
    this.totalPrice = 0;
    if (cart.cart.quantity > 1)
      cart.cart.quantity -= 1;
    this.listCart.forEach(x => {
      this.totalPrice += x.productPrice * x.cart.quantity
    });
  }

  delete(cart: GetCartForViewDto) {
    // this.message.confirm(
    //   '',
    //   this.l('AreYouSure'),
    //   (isConfirmed) => {
    //     if (isConfirmed) {
    //       this._cartsServiceProxy.delete(cart.cart.id)
    //         .subscribe(() => {
    //           this.getAllCart();
    //           this.notify.success(this.l('SuccessfullyDeleted'));
    //         });
    //     }
    //   }
    // );
    const swalWithBootstrapButtons = Swal.mixin({
      customClass: {
        confirmButton: 'btn btn-danger btn_delete',
        cancelButton: 'btn btn-primary btn_cancel',
      },
      buttonsStyling: false,
    })

    swalWithBootstrapButtons
      .fire({
        title: this.l('Hành động này sẽ xóa sản phẩm trong giỏ hàng'),
        text: this.l('Hãy chắc chắn bạn muốn xóa'),
        imageUrl: this.appRootUrl() + 'assets/common/images/danger.PNG',
        imageWidth: 60,
        imageHeight: 50,
        showCancelButton: true,
        showCloseButton: true,
        confirmButtonText: '<i class="fas fa-trash"></i>' + this.l('Xóa'),
        cancelButtonText: '<i class="fas fa-check"></i>' + this.l('Hủy'),
      })
      .then((result) => {
        if (result.value) {
          this._cartsServiceProxy.delete(cart.cart.id)
            .subscribe(() => {
              this.getAllCart();
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      })
  }
  purchase(listCart: GetCartForViewDto[]) {
    console.log(listCart)
  }

}
