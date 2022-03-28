import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ReturnProdsServiceProxy, CreateOrEditReturnProdDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

import { ReturnProdOrderLookupTableModalComponent } from './returnProd-order-lookup-table-modal.component';



@Component({
    selector: 'createOrEditReturnProdModal',
    templateUrl: './create-or-edit-returnProd-modal.component.html'
})
export class CreateOrEditReturnProdModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('returnProdOrderLookupTableModal', { static: true }) returnProdOrderLookupTableModal: ReturnProdOrderLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    returnProd: CreateOrEditReturnProdDto = new CreateOrEditReturnProdDto();

    orderOrderCode = '';



    constructor(
        injector: Injector,
        private _returnProdsServiceProxy: ReturnProdsServiceProxy
    ) {
        super(injector);
    }
    
    show(returnProdId?: number): void {
    

        if (!returnProdId) {
            this.returnProd = new CreateOrEditReturnProdDto();
            this.returnProd.id = returnProdId;
            this.orderOrderCode = '';


            this.active = true;
            this.modal.show();
        } else {
            this._returnProdsServiceProxy.getReturnProdForEdit(returnProdId).subscribe(result => {
                this.returnProd = result.returnProd;

                this.orderOrderCode = result.orderOrderCode;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._returnProdsServiceProxy.createOrEdit(this.returnProd)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectOrderModal() {
        this.returnProdOrderLookupTableModal.id = this.returnProd.orderId;
        this.returnProdOrderLookupTableModal.displayName = this.orderOrderCode;
        this.returnProdOrderLookupTableModal.show();
    }


    setOrderIdNull() {
        this.returnProd.orderId = null;
        this.orderOrderCode = '';
    }


    getNewOrderId() {
        this.returnProd.orderId = this.returnProdOrderLookupTableModal.id;
        this.orderOrderCode = this.returnProdOrderLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
