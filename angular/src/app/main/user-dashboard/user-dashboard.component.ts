import { Component, Injector, OnInit} from "@angular/core";
import { Router } from "@angular/router";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AppComponentBase } from "@shared/common/app-component-base";
import {
    ProductDto,
    ProductsServiceProxy,
} from "@shared/service-proxies/service-proxies";

@Component({
    selector: "app-user-dashboard",
    templateUrl: "./user-dashboard.component.html",
    styleUrls: ["./user-dashboard.component.css"],
    animations: [appModuleAnimation()]
})
export class UserDashboardComponent extends AppComponentBase implements OnInit {
    listProduct: ProductDto[] = [];
    defaultRouter ='../../../assets/common/images/';
    constructor(
        injector: Injector,
        private _productsServiceProxy: ProductsServiceProxy,
        private router: Router
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.getProducts();
    }

    getProducts() {
        this._productsServiceProxy.getProductForDashBoard()
            .subscribe((result) => {
                this.listProduct = result
            });
    }

    detail(product: ProductDto){
        this.router.navigate(['/app/main/entity/product-detail', product.id, product.name]);  // define your component where you want to go
    }
}
