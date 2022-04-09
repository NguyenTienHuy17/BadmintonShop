import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { BookingsServiceProxy, CreateOrEditBookingDto } from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css'],
  animations: [appModuleAnimation()]
})
export class BookingComponent extends AppComponentBase implements OnInit {

  booking: CreateOrEditBookingDto = new CreateOrEditBookingDto();
  paginator: any;
  constructor(
    injector: Injector,
    private _bookingsServiceProxy: BookingsServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
  }

  save(): void {
    this._bookingsServiceProxy.createOrEdit(this.booking)
      .subscribe(() => {
        this.notify.info(this.l('CreateBookingRequestSuccessfully'));
        window.location.reload();
      });
  }

  getBookings(event?: LazyLoadEvent) {
    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      return;
    }

    this.primengTableHelper.showLoadingIndicator();

    this._bookingsServiceProxy.getAllForUser().subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
    });
  }

}
