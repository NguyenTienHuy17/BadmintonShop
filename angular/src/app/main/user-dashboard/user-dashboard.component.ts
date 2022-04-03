import { Component, Injector, OnInit} from "@angular/core";
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
                console.log(this.listProduct)
            });
    }
}
