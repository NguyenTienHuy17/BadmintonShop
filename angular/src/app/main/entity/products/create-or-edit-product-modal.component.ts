import { Component, Injector, Output, EventEmitter, OnInit, ElementRef, ViewChild} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ProductsServiceProxy, CreateOrEditProductDto, CreateOrEditProductImageDto, BrandsServiceProxy, GetBrandForViewDto, Brand, Category, CategoriesServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { AppConsts } from '@shared/AppConsts';
import { HttpResponse, HttpClient } from '@angular/common/http'
import { ModeImage } from '@app/main/newUploadImage/modeImage';
import { ImageProduct } from '@app/main/newUploadImage/imageProduct';
import { UploadSingleImageComponent } from '@app/main/uploadSingleImage/uploadSingleImage.component';
import { NewUploadImageComponent } from '@app/main/newUploadImage/newUploadImage.component';


@Component({
    selector: 'createOrEditProductModal',
    templateUrl: './create-or-edit-product-modal.component.html'
})
export class CreateOrEditProductModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('singleUploadImage', { static: true }) singleUploadImage: UploadSingleImageComponent
	@ViewChild('newUploadImage', { static: true }) newUploadImage: NewUploadImageComponent

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

	uploadUrl = AppConsts.remoteServiceBaseUrl + '/UploadImage/UploadMultipleFileToServer'

    active = false;
    saving = false;

    product: CreateOrEditProductDto = new CreateOrEditProductDto();
    listProductImage: CreateOrEditProductImageDto = new CreateOrEditProductImageDto();
    listBrand: Brand [] = []
    listCategory: Category [] = []
    imageName = '';
    brandName = '';
    categoryName = '';

    modeImage: ModeImage;
	listUrlImage: ImageProduct[] = []
    
	urlUploadAndCreate = AppConsts.remoteServiceBaseUrl + '/UploadImage/UploadMultipleFileToServerAndCreate'
	newProductId: number = undefined
	viewImageUrl = AppConsts.remoteServiceBaseUrl + '/UploadImage/GetImageProduct'
    newlistProductImage = [];
    

    constructor(
        injector: Injector,
        private _productsServiceProxy: ProductsServiceProxy,
        private _brandsServiceProxy: BrandsServiceProxy,
        private _categoriesServiceProxy: CategoriesServiceProxy,
    ) {
        super(injector);
    }
    
    show(productId?: number): void {
    

        if (!productId) {
            this.product = new CreateOrEditProductDto();
            this.product.id = productId;
            this.imageName = '';
            this.brandName = '';
            this.categoryName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._productsServiceProxy.getProductForEdit(productId).subscribe(result => {
                this.product = result.product;

                this.imageName = result.imageName;
                this.brandName = result.brandName;
                this.categoryName = result.categoryName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            this.listUrlImage.forEach(e => {
                this.listProductImage.url = e.url;
                this.listProductImage.name = e.name;
                this.newlistProductImage.push(this.listProductImage)
            });
            this.product.listProductImage = this.newlistProductImage;
            this._productsServiceProxy.createOrEdit(this.product)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe((result) => {
                this.newProductId = result
                this.uploadFileToServer()
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    async uploadFileToServer(): Promise<void> {
		var response = await this.newUploadImage.uploadImage(this.uploadUrl, this.newProductId.toString()).toPromise()
		if (response instanceof HttpResponse) {
			if (response.status == 200) {
				let result = (response.body as any).result
				this.product.listProductImage = result
				this.product.id = this.newProductId
			} else {
				this.notify.error(this.l('UploadImageFails'), '', { timeOut: 5000, extendedTimeOut: 1000, positionClass: 'toast-bottom-left' })
			}
		}
	}


    setImageIdNull() {
        this.product.imageId = null;
        this.imageName = '';
    }
    setBrandIdNull() {
        this.product.brandId = null;
        this.brandName = '';
    }
    setCategoryIdNull() {
        this.product.categoryId = null;
        this.categoryName = '';
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.listUrlImage = []
    }
    
     ngOnInit(): void {
        console.log("newUploadImage",this.newUploadImage)
        console.log("singleUploadImage",this.singleUploadImage)
        console.log("modal",this.modal)
		this.modeImage = ModeImage.AddNew
        this._brandsServiceProxy.getAllForProduct().subscribe(result => {
            this.listBrand = result
        });
        this._categoriesServiceProxy.getAllForProduct().subscribe(result => {
            this.listCategory = result
        });
     }    

     changeMainImage(event) {
		if (event) {
            this.listUrlImage = event
			// this.singleUploadImage.previewUrl = event.url
		} else {
            // this.listUrlImage = undefined    
			// this.singleUploadImage.previewUrl = undefined
		}
	}
    uploadFileWhenEdit(files: FileList) {
		this.newUploadImage.uploadImageWhenEdit(files, this.urlUploadAndCreate, this.newProductId.toString()).subscribe((result) => {
			if (result instanceof HttpResponse) {
				if (result.status == 200) {
					let srcImageUploaded = (result.body as any).result
					// this.srcImageUploadSuccess.emit(srcImageUploaded)

					// this._sellingProductsServiceProxy.getPathFolderForProduct(this.newProductId).subscribe((pathFolder) => {
					// 	let pathTenant = pathFolder.pathTenantFolder
					// 	let pathStore = pathFolder.pathStoreFolder
					// 	let pathProduct = pathFolder.pathProductFolder
					// 	srcImageUploaded.forEach((item) => {
					// 		var src = this.viewImageUrl + '?pathTenant=' + pathTenant + '&pathStore=' + pathStore + '&pathProduct=' + pathProduct + '&fileName=' + item.imageLink
					// 		this.listUrlImage.push({
					// 			id: item.id,
					// 			url: src,
					// 		})
					// 	})
					// })

					this.notify.success(this.l('UploadImageSuccess'), '', { timeOut: 5000, extendedTimeOut: 1000, positionClass: 'toast-bottom-left' })
				}
			}
		})
	}

}
