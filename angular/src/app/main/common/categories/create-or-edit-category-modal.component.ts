import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { CategoriesServiceProxy, CreateOrEditCategoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

import { CategoryImageLookupTableModalComponent } from './category-image-lookup-table-modal.component';



@Component({
    selector: 'createOrEditCategoryModal',
    templateUrl: './create-or-edit-category-modal.component.html'
})
export class CreateOrEditCategoryModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('categoryImageLookupTableModal', { static: true }) categoryImageLookupTableModal: CategoryImageLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    category: CreateOrEditCategoryDto = new CreateOrEditCategoryDto();

    imageName = '';



    constructor(
        injector: Injector,
        private _categoriesServiceProxy: CategoriesServiceProxy
    ) {
        super(injector);
    }
    
    show(categoryId?: number): void {
    

        if (!categoryId) {
            this.category = new CreateOrEditCategoryDto();
            this.category.id = categoryId;
            this.imageName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._categoriesServiceProxy.getCategoryForEdit(categoryId).subscribe(result => {
                this.category = result.category;

                this.imageName = result.imageName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._categoriesServiceProxy.createOrEdit(this.category)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectImageModal() {
        this.categoryImageLookupTableModal.id = this.category.imageId;
        this.categoryImageLookupTableModal.displayName = this.imageName;
        this.categoryImageLookupTableModal.show();
    }


    setImageIdNull() {
        this.category.imageId = null;
        this.imageName = '';
    }


    getNewImageId() {
        this.category.imageId = this.categoryImageLookupTableModal.id;
        this.imageName = this.categoryImageLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
