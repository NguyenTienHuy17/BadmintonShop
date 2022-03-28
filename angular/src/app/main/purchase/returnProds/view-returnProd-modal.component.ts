import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetReturnProdForViewDto, ReturnProdDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewReturnProdModal',
    templateUrl: './view-returnProd-modal.component.html'
})
export class ViewReturnProdModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetReturnProdForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetReturnProdForViewDto();
        this.item.returnProd = new ReturnProdDto();
    }

    show(item: GetReturnProdForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
