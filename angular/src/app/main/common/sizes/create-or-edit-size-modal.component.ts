import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { SizesServiceProxy, CreateOrEditSizeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';




@Component({
    selector: 'createOrEditSizeModal',
    templateUrl: './create-or-edit-size-modal.component.html'
})
export class CreateOrEditSizeModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    size: CreateOrEditSizeDto = new CreateOrEditSizeDto();




    constructor(
        injector: Injector,
        private _sizesServiceProxy: SizesServiceProxy
    ) {
        super(injector);
    }
    
    show(sizeId?: number): void {
    

        if (!sizeId) {
            this.size = new CreateOrEditSizeDto();
            this.size.id = sizeId;


            this.active = true;
            this.modal.show();
        } else {
            this._sizesServiceProxy.getSizeForEdit(sizeId).subscribe(result => {
                this.size = result.size;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._sizesServiceProxy.createOrEdit(this.size)
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
