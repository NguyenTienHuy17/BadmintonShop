import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetColorItemForViewDto, ColorItemDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewColorItemModal',
    templateUrl: './view-colorItem-modal.component.html'
})
export class ViewColorItemModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetColorItemForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetColorItemForViewDto();
        this.item.colorItem = new ColorItemDto();
    }

    show(item: GetColorItemForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
