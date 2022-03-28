import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { DiscountsServiceProxy, CreateOrEditDiscountDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';




@Component({
    selector: 'createOrEditDiscountModal',
    templateUrl: './create-or-edit-discount-modal.component.html'
})
export class CreateOrEditDiscountModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    discount: CreateOrEditDiscountDto = new CreateOrEditDiscountDto();




    constructor(
        injector: Injector,
        private _discountsServiceProxy: DiscountsServiceProxy
    ) {
        super(injector);
    }
    
    show(discountId?: number): void {
    

        if (!discountId) {
            this.discount = new CreateOrEditDiscountDto();
            this.discount.id = discountId;


            this.active = true;
            this.modal.show();
        } else {
            this._discountsServiceProxy.getDiscountForEdit(discountId).subscribe(result => {
                this.discount = result.discount;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._discountsServiceProxy.createOrEdit(this.discount)
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
