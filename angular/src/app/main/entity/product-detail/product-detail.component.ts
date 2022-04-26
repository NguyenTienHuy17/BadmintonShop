import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CartsServiceProxy, CreateOrEditCartDto, GetProductForViewDto, ProductDto, ProductsServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { Slide } from 'react-slideshow-image';
import 'react-slideshow-image/dist/styles.css'
import { async } from '@angular/core/testing';

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
  defaultRouter = '../../../../assets/common/images/';
  quantity: number = 1;
  cart: CreateOrEditCartDto;
  saving = false;
  position: number;
  slideImages = [];
  slideImage = {
    image: '',
    thumbImage: ''
  }
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
    this.position = 0;
    this.cart = new CreateOrEditCartDto();
  }

  ngOnInit() {
    this._activatedRoute.params.subscribe((params: Params) => {
      this.productId = params['id'];
      this.productName = params['name'];
    });
    this._productsServiceProxy.getProductForView(this.productId, this.productName)
      .subscribe(result => {
        this.product = result;
        result.productImageUrl.forEach(i => {
          this.slideImage = {
            image: '',
            thumbImage: ''
          }
          this.slideImage.image = '../../../../assets/common/images/' + i;
          this.slideImage.thumbImage = '../../../../assets/common/images/' + i;
          this.slideImages.push(this.slideImage)
        });
      });
  }

  async addToCart() {
    this.cart.productId = await this._productsServiceProxy.getProductId(this.productName,
      this.product.product.size,
      this.product.product.color)
      .toPromise();
    this.cart.quantity = this.quantity;
    this.saving = true;
    await this._cartServiceProxy.addProductToCart(this.cart)
      .pipe(finalize(() => { this.saving = false; }))
      .subscribe(() => {
        this.notify.success(this.l('SuccessfullyAddedToCart'));
        this.router.navigate(['/app/main/user-dashboard']);
      });
  }

  add() {
    if (this.quantity < this.product.product.inStock) {
      this.quantity += 1;
    }
  }

  minus() {
    if (this.quantity > 1)
      this.quantity -= 1;
  }

}
