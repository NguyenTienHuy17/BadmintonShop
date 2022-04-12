import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { OrderItemsServiceProxy, CreateOrEditOrderItemDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';



@Component({
    selector: 'createOrEditOrderItemModal',
    templateUrl: './create-or-edit-orderItem-modal.component.html'
})
export class CreateOrEditOrderItemModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderItem: CreateOrEditOrderItemDto = new CreateOrEditOrderItemDto();

    productName = '';
    orderOrderCode = '';



    constructor(
        injector: Injector,
        private _orderItemsServiceProxy: OrderItemsServiceProxy
    ) {
        super(injector);
    }

    show(orderItemId?: number): void {


        if (!orderItemId) {
            this.orderItem = new CreateOrEditOrderItemDto();
            this.orderItem.id = orderItemId;
            this.productName = '';
            this.orderOrderCode = '';


            this.active = true;
            this.modal.show();
        } else {
            this._orderItemsServiceProxy.getOrderItemForEdit(orderItemId).subscribe(result => {
                this.orderItem = result.orderItem;

                this.productName = result.productName;
                this.orderOrderCode = result.orderOrderCode;


                this.active = true;
                this.modal.show();
            });
        }


    }

    save(): void {
        this.saving = true;



        this._orderItemsServiceProxy.createOrEdit(this.orderItem)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }


    setProductIdNull() {
        this.orderItem.productId = null;
        this.productName = '';
    }
    setOrderIdNull() {
        this.orderItem.orderId = null;
        this.orderOrderCode = '';
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {

    }
}
