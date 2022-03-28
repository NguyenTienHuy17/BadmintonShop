import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { StatusesServiceProxy, CreateOrEditStatusDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';




@Component({
    selector: 'createOrEditStatusModal',
    templateUrl: './create-or-edit-status-modal.component.html'
})
export class CreateOrEditStatusModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    status: CreateOrEditStatusDto = new CreateOrEditStatusDto();




    constructor(
        injector: Injector,
        private _statusesServiceProxy: StatusesServiceProxy
    ) {
        super(injector);
    }
    
    show(statusId?: number): void {
    

        if (!statusId) {
            this.status = new CreateOrEditStatusDto();
            this.status.id = statusId;


            this.active = true;
            this.modal.show();
        } else {
            this._statusesServiceProxy.getStatusForEdit(statusId).subscribe(result => {
                this.status = result.status;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._statusesServiceProxy.createOrEdit(this.status)
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
