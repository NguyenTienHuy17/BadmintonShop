import { AppConsts } from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetBlogForViewDto, BlogDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBlogModal',
    templateUrl: './view-blog-modal.component.html'
})
export class ViewBlogModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBlogForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetBlogForViewDto();
        this.item.blog = new BlogDto();
    }

    show(item: BlogDto): void {
        this.item.blog = item;
        this.active = true;
        this.modal.show();
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
