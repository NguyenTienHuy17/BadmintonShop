import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { BlogsServiceProxy, CreateOrEditBlogDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';




@Component({
    selector: 'createOrEditBlogModal',
    templateUrl: './create-or-edit-blog-modal.component.html'
})
export class CreateOrEditBlogModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    blog: CreateOrEditBlogDto = new CreateOrEditBlogDto();




    constructor(
        injector: Injector,
        private _blogsServiceProxy: BlogsServiceProxy
    ) {
        super(injector);
    }
    
    show(blogId?: number): void {
    

        if (!blogId) {
            this.blog = new CreateOrEditBlogDto();
            this.blog.id = blogId;


            this.active = true;
            this.modal.show();
        } else {
            this._blogsServiceProxy.getBlogForEdit(blogId).subscribe(result => {
                this.blog = result.blog;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._blogsServiceProxy.createOrEdit(this.blog)
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
