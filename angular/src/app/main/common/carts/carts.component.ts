import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { CartsServiceProxy, CartDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditCartModalComponent } from './create-or-edit-cart-modal.component';

import { ViewCartModalComponent } from './view-cart-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';


@Component({
    templateUrl: './carts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CartsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditCartModal', { static: true }) createOrEditCartModal: CreateOrEditCartModalComponent;
    @ViewChild('viewCartModalComponent', { static: true }) viewCartModal: ViewCartModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxquantityFilter : number;
		maxquantityFilterEmpty : number;
		minquantityFilter : number;
		minquantityFilterEmpty : number;
        productNameFilter = '';






    constructor(
        injector: Injector,
        private _cartsServiceProxy: CartsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getCarts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._cartsServiceProxy.getAll(
            this.filterText,
            this.maxquantityFilter == null ? this.maxquantityFilterEmpty: this.maxquantityFilter,
            this.minquantityFilter == null ? this.minquantityFilterEmpty: this.minquantityFilter,
            this.productNameFilter,
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

    createCart(): void {
        this.createOrEditCartModal.show();        
    }


    deleteCart(cart: CartDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._cartsServiceProxy.delete(cart.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._cartsServiceProxy.getCartsToExcel(
        this.filterText,
            this.maxquantityFilter == null ? this.maxquantityFilterEmpty: this.maxquantityFilter,
            this.minquantityFilter == null ? this.minquantityFilterEmpty: this.minquantityFilter,
            this.productNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    
}
