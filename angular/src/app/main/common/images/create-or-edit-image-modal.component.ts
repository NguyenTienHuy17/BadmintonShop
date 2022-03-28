import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ImagesServiceProxy, CreateOrEditImageDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { FileUploader } from 'ng2-file-upload';
import { HttpClient, HttpRequest, HttpResponse } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';




@Component({
    selector: 'createOrEditImageModal',
    templateUrl: './create-or-edit-image-modal.component.html'
})
export class CreateOrEditImageModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    image: CreateOrEditImageDto = new CreateOrEditImageDto();

    public uploader: FileUploader;
    imageChangedEvent: any = '';
    private maxProfilPictureBytesUserFriendlyValue = 5;
    fileUpload: File = undefined
    uploadUrl = AppConsts.remoteServiceBaseUrl + '/UploadImage/UploadPicture';
    constructor(
        injector: Injector,
        private _imagesServiceProxy: ImagesServiceProxy,
        private http: HttpClient
    ) {
        super(injector);
    }
    
    show(imageId?: number): void {
    

        if (!imageId) {
            this.image = new CreateOrEditImageDto();
            this.image.id = imageId;


            this.active = true;
            this.modal.show();
        } else {
            this._imagesServiceProxy.getImageForEdit(imageId).subscribe(result => {
                this.image = result.image;
                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    async save() {
        this.saving = true;
        await this.uploadFileToServer(this.fileUpload);
        this._imagesServiceProxy.createOrEdit(this.image)
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

    async uploadFileToServer(fileUpload: File): Promise<void> {
		const formData: FormData = new FormData()
        
		formData.append('upload', fileUpload)

		const config = new HttpRequest('POST', this.uploadUrl, formData, {
			reportProgress: true,
		})

		var response = await this.http.request(config).toPromise()
		if (response instanceof HttpResponse) {
            if (response.status == 200) {
                let result = (response.body as any).result
                this.image.url = result;
			} else {
				this.notify.error(this.l('!upload fails'), '', { timeOut: 5000, extendedTimeOut: 1000, positionClass: 'toast-bottom-left' })
			}
		}
	}

    fileChangeEvent(event: any): void {
        if (event.target.files[0].size > 5242880) { //5MB
            this.message.warn(this.l('ProfilePicture_Warn_SizeLimit', this.maxProfilPictureBytesUserFriendlyValue));
            return;
        }
        this.fileUpload = event.target.files[0];
    }
}
