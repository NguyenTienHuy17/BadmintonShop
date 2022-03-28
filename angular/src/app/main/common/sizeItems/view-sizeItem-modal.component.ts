import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetSizeItemForViewDto, SizeItemDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewSizeItemModal',
    templateUrl: './view-sizeItem-modal.component.html'
})
export class ViewSizeItemModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetSizeItemForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetSizeItemForViewDto();
        this.item.sizeItem = new SizeItemDto();
    }

    show(item: GetSizeItemForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
