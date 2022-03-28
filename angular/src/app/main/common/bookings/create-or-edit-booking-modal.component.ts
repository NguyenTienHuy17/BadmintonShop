import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { BookingsServiceProxy, CreateOrEditBookingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';




@Component({
    selector: 'createOrEditBookingModal',
    templateUrl: './create-or-edit-booking-modal.component.html'
})
export class CreateOrEditBookingModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    booking: CreateOrEditBookingDto = new CreateOrEditBookingDto();




    constructor(
        injector: Injector,
        private _bookingsServiceProxy: BookingsServiceProxy
    ) {
        super(injector);
    }
    
    show(bookingId?: number): void {
    

        if (!bookingId) {
            this.booking = new CreateOrEditBookingDto();
            this.booking.id = bookingId;
            this.booking.time = moment().startOf('day');


            this.active = true;
            this.modal.show();
        } else {
            this._bookingsServiceProxy.getBookingForEdit(bookingId).subscribe(result => {
                this.booking = result.booking;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._bookingsServiceProxy.createOrEdit(this.booking)
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
