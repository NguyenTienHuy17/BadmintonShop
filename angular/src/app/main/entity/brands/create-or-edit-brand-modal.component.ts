import { Component, ViewChild, Injector, Output, EventEmitter, OnInit} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { BrandsServiceProxy, CreateOrEditBrandDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ModeImage } from '@app/main/newUploadImage/modeImage';
import { ImageProduct } from '@app/main/newUploadImage/imageProduct';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient, HttpRequest, HttpResponse } from '@angular/common/http';

@Component({
    selector: 'createOrEditBrandModal',
    templateUrl: './create-or-edit-brand-modal.component.html'
})
export class CreateOrEditBrandModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    brand: CreateOrEditBrandDto = new CreateOrEditBrandDto();
    imageChangedEvent: any = '';
    imageName = '';
    private maxProfilPictureBytesUserFriendlyValue = 5;
    
    modeImage: ModeImage;
	listUrlImage: ImageProduct[] = []
	uploadUrl = AppConsts.remoteServiceBaseUrl + '/UploadImage/UploadPicture'
    fileUpload: File;

    constructor(
        injector: Injector,
        private _brandsServiceProxy: BrandsServiceProxy,
        private http: HttpClient
    ) {
        super(injector);
    }
    
    show(brandId?: number): void {
    

        if (!brandId) {
            this.brand = new CreateOrEditBrandDto();
            this.brand.id = brandId;
            this.imageName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._brandsServiceProxy.getBrandForEdit(brandId).subscribe(result => {
                this.brand = result.brand;

                this.imageName = result.imageName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    async save(){
        this.saving = true;
        if (this.fileUpload) {
            await this.uploadFileToServer(this.fileUpload)
        }
        this._brandsServiceProxy.createOrEdit(this.brand)
            .pipe(finalize(() => { this.saving = false;}))
            .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.close();
            this.modalSave.emit(null);
            });
    }


    setImageIdNull() {
        // this.brand.imageId = null;
        this.imageName = '';
    }
    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
    ngOnInit(): void {
    
    }    

    async uploadFileToServer(fileUpload: File): Promise<void> {
        const formData: FormData = new FormData()
        formData.append('upload', fileUpload, fileUpload.name)
        const config = new HttpRequest('POST', this.uploadUrl, formData, {
            reportProgress: true,
        })

        var response = await this.http.request(config).toPromise()

        if (response instanceof HttpResponse) {
            if (response.status == 200) {
                let result = (response.body as any).result
                this.brand.imageUrl = result
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
        this.fileUpload = event.target.files[0]
    }
}
