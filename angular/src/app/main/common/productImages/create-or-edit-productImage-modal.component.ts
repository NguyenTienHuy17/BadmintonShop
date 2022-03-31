import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ProductImagesServiceProxy, CreateOrEditProductImageDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

import { ProductImageProductLookupTableModalComponent } from './productImage-product-lookup-table-modal.component';
import { ProductImageImageLookupTableModalComponent } from './productImage-image-lookup-table-modal.component';



@Component({
    selector: 'createOrEditProductImageModal',
    templateUrl: './create-or-edit-productImage-modal.component.html'
})
export class CreateOrEditProductImageModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productImageProductLookupTableModal', { static: true }) productImageProductLookupTableModal: ProductImageProductLookupTableModalComponent;
    @ViewChild('productImageImageLookupTableModal', { static: true }) productImageImageLookupTableModal: ProductImageImageLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productImage: CreateOrEditProductImageDto = new CreateOrEditProductImageDto();

    productName = '';
    imageName = '';



    constructor(
        injector: Injector,
        private _productImagesServiceProxy: ProductImagesServiceProxy
    ) {
        super(injector);
    }
    
    show(productImageId?: number): void {
    

        if (!productImageId) {
            this.productImage = new CreateOrEditProductImageDto();
            this.productImage.id = productImageId;
            this.productName = '';
            this.imageName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._productImagesServiceProxy.getProductImageForEdit(productImageId).subscribe(result => {
                this.productImage = result.productImage;

                this.productName = result.productName;
                this.imageName = result.imageName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._productImagesServiceProxy.createOrEdit(this.productImage)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectProductModal() {
        this.productImageProductLookupTableModal.id = this.productImage.productId;
        this.productImageProductLookupTableModal.displayName = this.productName;
        this.productImageProductLookupTableModal.show();
    }
    openSelectImageModal() {
        this.productImageImageLookupTableModal.displayName = this.imageName;
        this.productImageImageLookupTableModal.show();
    }


    setProductIdNull() {
        this.productImage.productId = null;
        this.productName = '';
    }
    setImageIdNull() {
        this.imageName = '';
    }


    getNewProductId() {
        this.productImage.productId = this.productImageProductLookupTableModal.id;
        this.productName = this.productImageProductLookupTableModal.displayName;
    }
    getNewImageId() {
        this.imageName = this.productImageImageLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
