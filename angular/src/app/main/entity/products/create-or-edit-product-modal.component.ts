import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ProductsServiceProxy, CreateOrEditProductDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

import { ProductImageLookupTableModalComponent } from './product-image-lookup-table-modal.component';
import { ProductBrandLookupTableModalComponent } from './product-brand-lookup-table-modal.component';
import { ProductCategoryLookupTableModalComponent } from './product-category-lookup-table-modal.component';



@Component({
    selector: 'createOrEditProductModal',
    templateUrl: './create-or-edit-product-modal.component.html'
})
export class CreateOrEditProductModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productImageLookupTableModal', { static: true }) productImageLookupTableModal: ProductImageLookupTableModalComponent;
    @ViewChild('productBrandLookupTableModal', { static: true }) productBrandLookupTableModal: ProductBrandLookupTableModalComponent;
    @ViewChild('productCategoryLookupTableModal', { static: true }) productCategoryLookupTableModal: ProductCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    product: CreateOrEditProductDto = new CreateOrEditProductDto();

    imageName = '';
    brandName = '';
    categoryName = '';



    constructor(
        injector: Injector,
        private _productsServiceProxy: ProductsServiceProxy
    ) {
        super(injector);
    }
    
    show(productId?: number): void {
    

        if (!productId) {
            this.product = new CreateOrEditProductDto();
            this.product.id = productId;
            this.imageName = '';
            this.brandName = '';
            this.categoryName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._productsServiceProxy.getProductForEdit(productId).subscribe(result => {
                this.product = result.product;

                this.imageName = result.imageName;
                this.brandName = result.brandName;
                this.categoryName = result.categoryName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._productsServiceProxy.createOrEdit(this.product)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectImageModal() {
        this.productImageLookupTableModal.id = this.product.imageId;
        this.productImageLookupTableModal.displayName = this.imageName;
        this.productImageLookupTableModal.show();
    }
    openSelectBrandModal() {
        this.productBrandLookupTableModal.id = this.product.brandId;
        this.productBrandLookupTableModal.displayName = this.brandName;
        this.productBrandLookupTableModal.show();
    }
    openSelectCategoryModal() {
        this.productCategoryLookupTableModal.id = this.product.categoryId;
        this.productCategoryLookupTableModal.displayName = this.categoryName;
        this.productCategoryLookupTableModal.show();
    }


    setImageIdNull() {
        this.product.imageId = null;
        this.imageName = '';
    }
    setBrandIdNull() {
        this.product.brandId = null;
        this.brandName = '';
    }
    setCategoryIdNull() {
        this.product.categoryId = null;
        this.categoryName = '';
    }


    getNewImageId() {
        this.product.imageId = this.productImageLookupTableModal.id;
        this.imageName = this.productImageLookupTableModal.displayName;
    }
    getNewBrandId() {
        this.product.brandId = this.productBrandLookupTableModal.id;
        this.brandName = this.productBrandLookupTableModal.displayName;
    }
    getNewCategoryId() {
        this.product.categoryId = this.productCategoryLookupTableModal.id;
        this.categoryName = this.productCategoryLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
