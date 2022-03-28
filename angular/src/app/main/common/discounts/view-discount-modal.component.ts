import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetDiscountForViewDto, DiscountDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewDiscountModal',
    templateUrl: './view-discount-modal.component.html'
})
export class ViewDiscountModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetDiscountForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetDiscountForViewDto();
        this.item.discount = new DiscountDto();
    }

    show(item: GetDiscountForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
