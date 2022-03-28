import {AppConsts} from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute , Router} from '@angular/router';
import { ColorsServiceProxy, ColorDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditColorModalComponent } from './create-or-edit-color-modal.component';

import { ViewColorModalComponent } from './view-color-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';


@Component({
    templateUrl: './colors.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ColorsComponent extends AppComponentBase {
    
    
    @ViewChild('createOrEditColorModal', { static: true }) createOrEditColorModal: CreateOrEditColorModalComponent;
    @ViewChild('viewColorModalComponent', { static: true }) viewColorModal: ViewColorModalComponent;   
    
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    colorNameFilter = '';
    maxColorCodeFilter : number;
		maxColorCodeFilterEmpty : number;
		minColorCodeFilter : number;
		minColorCodeFilterEmpty : number;






    constructor(
        injector: Injector,
        private _colorsServiceProxy: ColorsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getColors(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._colorsServiceProxy.getAll(
            this.filterText,
            this.colorNameFilter,
            this.maxColorCodeFilter == null ? this.maxColorCodeFilterEmpty: this.maxColorCodeFilter,
            this.minColorCodeFilter == null ? this.minColorCodeFilterEmpty: this.minColorCodeFilter,
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

    createColor(): void {
        this.createOrEditColorModal.show();        
    }


    deleteColor(color: ColorDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._colorsServiceProxy.delete(color.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._colorsServiceProxy.getColorsToExcel(
        this.filterText,
            this.colorNameFilter,
            this.maxColorCodeFilter == null ? this.maxColorCodeFilterEmpty: this.maxColorCodeFilter,
            this.minColorCodeFilter == null ? this.minColorCodeFilterEmpty: this.minColorCodeFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
    
    
    
    
    
}
