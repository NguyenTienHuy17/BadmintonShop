import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetOrderItemForViewDto, OrderItemDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOrderItemModal',
    templateUrl: './view-orderItem-modal.component.html'
})
export class ViewOrderItemModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOrderItemForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetOrderItemForViewDto();
        this.item.orderItem = new OrderItemDto();
    }

    show(item: GetOrderItemForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
