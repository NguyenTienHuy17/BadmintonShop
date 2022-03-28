import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ColorItemsServiceProxy, CreateOrEditColorItemDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

import { ColorItemProductLookupTableModalComponent } from './colorItem-product-lookup-table-modal.component';
import { ColorItemColorLookupTableModalComponent } from './colorItem-color-lookup-table-modal.component';



@Component({
    selector: 'createOrEditColorItemModal',
    templateUrl: './create-or-edit-colorItem-modal.component.html'
})
export class CreateOrEditColorItemModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('colorItemProductLookupTableModal', { static: true }) colorItemProductLookupTableModal: ColorItemProductLookupTableModalComponent;
    @ViewChild('colorItemColorLookupTableModal', { static: true }) colorItemColorLookupTableModal: ColorItemColorLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    colorItem: CreateOrEditColorItemDto = new CreateOrEditColorItemDto();

    productName = '';
    colorColorName = '';



    constructor(
        injector: Injector,
        private _colorItemsServiceProxy: ColorItemsServiceProxy
    ) {
        super(injector);
    }
    
    show(colorItemId?: number): void {
    

        if (!colorItemId) {
            this.colorItem = new CreateOrEditColorItemDto();
            this.colorItem.id = colorItemId;
            this.productName = '';
            this.colorColorName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._colorItemsServiceProxy.getColorItemForEdit(colorItemId).subscribe(result => {
                this.colorItem = result.colorItem;

                this.productName = result.productName;
                this.colorColorName = result.colorColorName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._colorItemsServiceProxy.createOrEdit(this.colorItem)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectProductModal() {
        this.colorItemProductLookupTableModal.id = this.colorItem.productId;
        this.colorItemProductLookupTableModal.displayName = this.productName;
        this.colorItemProductLookupTableModal.show();
    }
    openSelectColorModal() {
        this.colorItemColorLookupTableModal.id = this.colorItem.colorId;
        this.colorItemColorLookupTableModal.displayName = this.colorColorName;
        this.colorItemColorLookupTableModal.show();
    }


    setProductIdNull() {
        this.colorItem.productId = null;
        this.productName = '';
    }
    setColorIdNull() {
        this.colorItem.colorId = null;
        this.colorColorName = '';
    }


    getNewProductId() {
        this.colorItem.productId = this.colorItemProductLookupTableModal.id;
        this.productName = this.colorItemProductLookupTableModal.displayName;
    }
    getNewColorId() {
        this.colorItem.colorId = this.colorItemColorLookupTableModal.id;
        this.colorColorName = this.colorItemColorLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
