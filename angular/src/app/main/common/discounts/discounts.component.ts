import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { DiscountsServiceProxy, DiscountDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditDiscountModalComponent } from './create-or-edit-discount-modal.component';

import { ViewDiscountModalComponent } from './view-discount-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';


@Component({
    templateUrl: './discounts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class DiscountsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditDiscountModal', { static: true }) createOrEditDiscountModal: CreateOrEditDiscountModalComponent;
    @ViewChild('viewDiscountModalComponent', { static: true }) viewDiscountModal: ViewDiscountModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    discountCodeFilter = '';
    maxDiscountFilter : number;
		maxDiscountFilterEmpty : number;
		minDiscountFilter : number;
		minDiscountFilterEmpty : number;
    descriptionFilter = '';






    constructor(
        injector: Injector,
        private _discountsServiceProxy: DiscountsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getDiscounts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._discountsServiceProxy.getAll(
            this.filterText,
            this.discountCodeFilter,
            this.maxDiscountFilter == null ? this.maxDiscountFilterEmpty: this.maxDiscountFilter,
            this.minDiscountFilter == null ? this.minDiscountFilterEmpty: this.minDiscountFilter,
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

    createDiscount(): void {
        this.createOrEditDiscountModal.show();        
    }


    deleteDiscount(discount: DiscountDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._discountsServiceProxy.delete(discount.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._discountsServiceProxy.getDiscountsToExcel(
        this.filterText,
            this.discountCodeFilter,
            this.maxDiscountFilter == null ? this.maxDiscountFilterEmpty: this.maxDiscountFilter,
            this.minDiscountFilter == null ? this.minDiscountFilterEmpty: this.minDiscountFilter,
            this.descriptionFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    
}
