import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { SizeItemsServiceProxy, CreateOrEditSizeItemDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

import { SizeItemProductLookupTableModalComponent } from './sizeItem-product-lookup-table-modal.component';
import { SizeItemSizeLookupTableModalComponent } from './sizeItem-size-lookup-table-modal.component';



@Component({
    selector: 'createOrEditSizeItemModal',
    templateUrl: './create-or-edit-sizeItem-modal.component.html'
})
export class CreateOrEditSizeItemModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('sizeItemProductLookupTableModal', { static: true }) sizeItemProductLookupTableModal: SizeItemProductLookupTableModalComponent;
    @ViewChild('sizeItemSizeLookupTableModal', { static: true }) sizeItemSizeLookupTableModal: SizeItemSizeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    sizeItem: CreateOrEditSizeItemDto = new CreateOrEditSizeItemDto();

    productName = '';
    sizeDisplayName = '';



    constructor(
        injector: Injector,
        private _sizeItemsServiceProxy: SizeItemsServiceProxy
    ) {
        super(injector);
    }
    
    show(sizeItemId?: number): void {
    

        if (!sizeItemId) {
            this.sizeItem = new CreateOrEditSizeItemDto();
            this.sizeItem.id = sizeItemId;
            this.productName = '';
            this.sizeDisplayName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._sizeItemsServiceProxy.getSizeItemForEdit(sizeItemId).subscribe(result => {
                this.sizeItem = result.sizeItem;

                this.productName = result.productName;
                this.sizeDisplayName = result.sizeDisplayName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._sizeItemsServiceProxy.createOrEdit(this.sizeItem)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectProductModal() {
        this.sizeItemProductLookupTableModal.id = this.sizeItem.productId;
        this.sizeItemProductLookupTableModal.displayName = this.productName;
        this.sizeItemProductLookupTableModal.show();
    }
    openSelectSizeModal() {
        this.sizeItemSizeLookupTableModal.id = this.sizeItem.sizeId;
        this.sizeItemSizeLookupTableModal.displayName = this.sizeDisplayName;
        this.sizeItemSizeLookupTableModal.show();
    }


    setProductIdNull() {
        this.sizeItem.productId = null;
        this.productName = '';
    }
    setSizeIdNull() {
        this.sizeItem.sizeId = null;
        this.sizeDisplayName = '';
    }


    getNewProductId() {
        this.sizeItem.productId = this.sizeItemProductLookupTableModal.id;
        this.productName = this.sizeItemProductLookupTableModal.displayName;
    }
    getNewSizeId() {
        this.sizeItem.sizeId = this.sizeItemSizeLookupTableModal.id;
        this.sizeDisplayName = this.sizeItemSizeLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
