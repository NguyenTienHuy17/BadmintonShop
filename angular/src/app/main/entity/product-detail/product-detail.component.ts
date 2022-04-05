import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CartsServiceProxy, CreateOrEditCartDto, GetProductForViewDto, ProductDto, ProductsServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css'],
  animations: [appModuleAnimation()]
})
export class ProductDetailComponent extends AppComponentBase implements OnInit {

  productId: number;
  productName: string;
  product: GetProductForViewDto;
  defaultRouter ='../../../../assets/common/images/';
  quantity: number = 1;
  cart: CreateOrEditCartDto;
  constructor(injector: Injector,
    private _activatedRoute: ActivatedRoute,
    private _productsServiceProxy: ProductsServiceProxy,
    private _cartServiceProxy: CartsServiceProxy,
    private router: Router
    ) { 
    super(injector);
    this.product = new GetProductForViewDto();
    this.product.product = new ProductDto();
    this.product.product.price = 0;
    this.product.productSize = [];
    this.product.productColor = [];
    this.cart = new CreateOrEditCartDto();
  }

  ngOnInit() {
    this._activatedRoute.params.subscribe((params: Params) => {
      this.productId = params['id'];
      this.productName = params['name'];
    });
    this._productsServiceProxy.getProductForView(this.productId, this.productName)
    .subscribe(result => {
      this.product = result
    });
  }
  
  addToCart(){
    this._productsServiceProxy.getProductId(this.productName, 
      this.product.product.size, 
      this.product.product.color)
      .subscribe(result => {
                this.cart.quantity = this.quantity;
                this.cart.productId = result
              });
    this._cartServiceProxy.createOrEdit(this.cart);
    this.notify.success(this.l('SuccessfullyRegistered'));
    this.router.navigate(['/app/main/user-dashboard']);  // define your component where you want to go
  }

  add(){
    this.quantity += 1;
  }

  minus(){
    if(this.quantity > 1)
      this.quantity -=1;
  }

}
