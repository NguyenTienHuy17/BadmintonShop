import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { CartsServiceProxy, CreateOrEditCartDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';


@Component({
    selector: 'createOrEditCartModal',
    templateUrl: './create-or-edit-cart-modal.component.html'
})
export class CreateOrEditCartModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    cart: CreateOrEditCartDto = new CreateOrEditCartDto();

    productName = '';



    constructor(
        injector: Injector,
        private _cartsServiceProxy: CartsServiceProxy
    ) {
        super(injector);
    }

    show(cartId?: number): void {


        if (!cartId) {
            this.cart = new CreateOrEditCartDto();
            this.cart.id = cartId;
            this.productName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._cartsServiceProxy.getCartForEdit(cartId).subscribe(result => {
                this.cart = result.cart;

                this.productName = result.productName;


                this.active = true;
                this.modal.show();
            });
        }


    }

    save(): void {
        this.saving = true;



        this._cartsServiceProxy.createOrEdit(this.cart)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }


    setProductIdNull() {
        this.cart.productId = null;
        this.productName = '';
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {

    }
}
