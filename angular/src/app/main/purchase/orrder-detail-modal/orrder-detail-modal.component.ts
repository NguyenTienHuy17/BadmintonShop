import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetCartForViewDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap';

@Component({
  selector: 'orrder-detail-modal',
  templateUrl: './orrder-detail-modal.component.html',
  styleUrls: ['./orrder-detail-modal.component.css']
})
export class OrrderDetailModalComponent extends AppComponentBase implements OnInit {
  @ViewChild('purchaseModal', { static: true }) modal: ModalDirective;
  @Input() listCart: GetCartForViewDto[];  
  active = false;
  saving = false;

  constructor(injector: Injector,) { 
    super(injector);
  }

  ngOnInit() {
  }
  show(){
    this.modal.show();
  }

  close(): void {
    this.active = false;
    this.modal.hide();
  }

  save(){

  }

}
