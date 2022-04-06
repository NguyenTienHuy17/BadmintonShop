import { Component, Injector, OnInit, ViewEncapsulation } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CartsServiceProxy, GetCartForViewDto } from '@shared/service-proxies/service-proxies';
import { runInThisContext } from 'vm';

@Component({
  selector: '[cartDetails]',
  templateUrl: './cart-detail.component.html',
  styleUrls: ['./cart-detail.component.css'],
  animations: [appModuleAnimation()]
})
export class CartDetailComponent extends AppComponentBase implements OnInit {

  listCart: GetCartForViewDto[];  
  defaultRouter ='../../../../assets/common/images/';
  totalPrice: number;
  constructor(injector: Injector,
    private _cartsServiceProxy: CartsServiceProxy) {
    super(injector);
    this.listCart = [];
   }

  ngOnInit() {
    this.getAllCart();
  }

  getAllCart(){
    this.totalPrice = 0;
    this._cartsServiceProxy.getAllForCart().subscribe(result => {
      this.listCart = result;
      this.listCart.forEach(x => {
        this.totalPrice += x.productPrice * x.cart.quantity
      });
    });
  }

  add(cart: GetCartForViewDto){
    cart.cart.quantity +=1;
  }

  minus(cart: GetCartForViewDto){
    if(cart.cart.quantity > 1)
      cart.cart.quantity -=1;
  }

  delete(cart: GetCartForViewDto){
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
          if (isConfirmed) {
              this._cartsServiceProxy.delete(cart.cart.id)
                  .subscribe(() => {
                    this.getAllCart();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                  });
          }
      }
  );
  }

}
