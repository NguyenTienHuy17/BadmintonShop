import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ColorsServiceProxy, CreateOrEditColorDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';




@Component({
    selector: 'createOrEditColorModal',
    templateUrl: './create-or-edit-color-modal.component.html'
})
export class CreateOrEditColorModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    color: CreateOrEditColorDto = new CreateOrEditColorDto();




    constructor(
        injector: Injector,
        private _colorsServiceProxy: ColorsServiceProxy
    ) {
        super(injector);
    }
    
    show(colorId?: number): void {
    

        if (!colorId) {
            this.color = new CreateOrEditColorDto();
            this.color.id = colorId;


            this.active = true;
            this.modal.show();
        } else {
            this._colorsServiceProxy.getColorForEdit(colorId).subscribe(result => {
                this.color = result.color;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._colorsServiceProxy.createOrEdit(this.color)
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
