import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ImagesServiceProxy, CreateOrEditImageDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';




@Component({
    selector: 'createOrEditImageModal',
    templateUrl: './create-or-edit-image-modal.component.html'
})
export class CreateOrEditImageModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    image: CreateOrEditImageDto = new CreateOrEditImageDto();




    constructor(
        injector: Injector,
        private _imagesServiceProxy: ImagesServiceProxy
    ) {
        super(injector);
    }
    
    show(imageId?: number): void {
    

        if (!imageId) {
            this.image = new CreateOrEditImageDto();
            this.image.id = imageId;


            this.active = true;
            this.modal.show();
        } else {
            this._imagesServiceProxy.getImageForEdit(imageId).subscribe(result => {
                this.image = result.image;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._imagesServiceProxy.createOrEdit(this.image)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }













    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
