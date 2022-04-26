import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { OrdersServiceProxy, CreateOrEditOrderDto, StatusesServiceProxy, GetStatusForViewDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

import { OrderStatusLookupTableModalComponent } from './order-status-lookup-table-modal.component';
import { OrderDiscountLookupTableModalComponent } from './order-discount-lookup-table-modal.component';



@Component({
    selector: 'createOrEditOrderModal',
    templateUrl: './create-or-edit-order-modal.component.html'
})
export class CreateOrEditOrderModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderStatusLookupTableModal', { static: true }) orderStatusLookupTableModal: OrderStatusLookupTableModalComponent;
    @ViewChild('orderDiscountLookupTableModal', { static: true }) orderDiscountLookupTableModal: OrderDiscountLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    order: CreateOrEditOrderDto = new CreateOrEditOrderDto();

    statusName = '';
    discountDiscountCode = '';

    listStatus: GetStatusForViewDto[] = [];


    constructor(
        injector: Injector,
        private _ordersServiceProxy: OrdersServiceProxy,
        private _statusesServiceProxy: StatusesServiceProxy
    ) {
        super(injector);
    }

    show(orderId?: number): void {

        this._statusesServiceProxy.getAllStatusForSelect().subscribe(result => {
            this.listStatus = result;
        })
        if (!orderId) {
            this.order = new CreateOrEditOrderDto();
            this.order.id = orderId;
            this.statusName = '';
            this.discountDiscountCode = '';


            this.active = true;
            this.modal.show();
        } else {
            this._ordersServiceProxy.getOrderForEdit(orderId).subscribe(result => {
                this.order = result.order;

                this.statusName = result.statusName;
                this.discountDiscountCode = result.discountDiscountCode;


                this.active = true;
                this.modal.show();
            });
        }


    }

    save(): void {
        this.saving = true;



        this._ordersServiceProxy.createOrEdit(this.order)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectStatusModal() {
        this.orderStatusLookupTableModal.id = this.order.statusId;
        this.orderStatusLookupTableModal.displayName = this.statusName;
        this.orderStatusLookupTableModal.show();
    }
    openSelectDiscountModal() {
        this.orderDiscountLookupTableModal.id = this.order.discountId;
        this.orderDiscountLookupTableModal.displayName = this.discountDiscountCode;
        this.orderDiscountLookupTableModal.show();
    }


    setStatusIdNull() {
        this.order.statusId = null;
        this.statusName = '';
    }
    setDiscountIdNull() {
        this.order.discountId = null;
        this.discountDiscountCode = '';
    }


    getNewStatusId() {
        this.order.statusId = this.orderStatusLookupTableModal.id;
        this.statusName = this.orderStatusLookupTableModal.displayName;
    }
    getNewDiscountId() {
        this.order.discountId = this.orderDiscountLookupTableModal.id;
        this.discountDiscountCode = this.orderDiscountLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
        this.listStatus = [];
    }

    ngOnInit(): void {

    }
}
