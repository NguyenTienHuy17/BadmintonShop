import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { BookingsServiceProxy, BookingDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditBookingModalComponent } from './create-or-edit-booking-modal.component';

import { ViewBookingModalComponent } from './view-booking-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';


@Component({
    templateUrl: './bookings.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class BookingsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditBookingModal', { static: true }) createOrEditBookingModal: CreateOrEditBookingModalComponent;
    @ViewChild('viewBookingModalComponent', { static: true }) viewBookingModal: ViewBookingModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxTimeFilter : moment.Moment;
		minTimeFilter : moment.Moment;
    descriptionFilter = '';






    constructor(
        injector: Injector,
        private _bookingsServiceProxy: BookingsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getBookings(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._bookingsServiceProxy.getAll(
            this.filterText,
            this.maxTimeFilter === undefined ? this.maxTimeFilter : moment(this.maxTimeFilter).endOf('day'),
            this.minTimeFilter === undefined ? this.minTimeFilter : moment(this.minTimeFilter).startOf('day'),
            this.descriptionFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createBooking(): void {
        this.createOrEditBookingModal.show();        
    }


    deleteBooking(booking: BookingDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._bookingsServiceProxy.delete(booking.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._bookingsServiceProxy.getBookingsToExcel(
        this.filterText,
            this.maxTimeFilter === undefined ? this.maxTimeFilter : moment(this.maxTimeFilter).endOf('day'),
            this.minTimeFilter === undefined ? this.minTimeFilter : moment(this.minTimeFilter).startOf('day'),
            this.descriptionFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    
}
