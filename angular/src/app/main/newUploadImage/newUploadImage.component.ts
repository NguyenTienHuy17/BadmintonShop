import { Component, OnInit, ViewChild, Injector, Input, OnChanges, Output, EventEmitter } from '@angular/core'
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop'
import { ShowImageModalComponent } from './showImageModal/showImageModal.component'
import Swal from 'sweetalert2'
import { AppComponentBase } from '@shared/common/app-component-base'
import { HttpClient } from '@angular/common/http'
import { ModeImage } from './modeImage'
import { NewUploadImageService } from './newUploadImage.service'
import { Observable } from 'rxjs'
import { ImageProduct } from './imageProduct'
import { filter } from 'lodash'

@Component({
	selector: 'app-newUploadImage',
	templateUrl: './newUploadImage.component.html',
	styleUrls: ['./newUploadImage.component.less'],
})
export class NewUploadImageComponent extends AppComponentBase implements OnInit, OnChanges {
	@ViewChild('imageModal', { static: true }) imageModal: ShowImageModalComponent
	@Input() modeImage: ModeImage = ModeImage.View
	@Input() listUrlImage: ImageProduct[] = []
	@Input() urlUpload: string
	@Input() productId: string

	@Output() deleteImageWhenEdit: EventEmitter<any> = new EventEmitter<any>()
	@Output() filesNeedUpload: EventEmitter<any> = new EventEmitter<any>()
	@Output() changeOrdinalImage: EventEmitter<any> = new EventEmitter<any>()
	@Output() changeMainImage: EventEmitter<any> = new EventEmitter<any>()

	set mainImage(file: File) {
		this.setMainImage(file)
	}

	disableUpload = false

	filesToUpload: File[] = []

	maxLength = 10485760 //10MB

	constructor(injector: Injector, public http: HttpClient, private uploadService: NewUploadImageService) {
		super(injector)
	}

	ngOnInit() {
	}

	ngOnChanges() {
		if (this.modeImage == ModeImage.View) {
			this.disableUpload = true
		} else {
			this.disableUpload = false
		}
	}

	setMainImage(file: File) {
		if (file) {
			var src = URL.createObjectURL(file) // set src to blob url

			let maxId = this.generateMaxId(this.listUrlImage)
			this.listUrlImage[0] = {
				id: maxId,
				url: src,
				name: file.name
			}

			this.filesToUpload[0] = file
		} else {
			if (this.listUrlImage.length > 0) {
				this.listUrlImage.shift()
				this.filesToUpload.shift()
			}
		}
	}

	handleFileInput(files: FileList) {
		// Add mode
		if (this.modeImage == ModeImage.AddNew) {
			if (files != null) {
				if (this.listUrlImage.length + files.length > 20) {
					this.notify.error(this.l('Upload_FileCount_Err'), '', { timeOut: 5000, extendedTimeOut: 1000, positionClass: 'toast-bottom-left' })
					return
				}

				let isError = false
				let tempFiles = []
				let temps = []
				for (let i = 0; i < files.length; i++) {
					if (files[i].size > this.maxLength) {
						isError = true
						this.notify.error(this.l('Upload_FileSize_Err'), '', { timeOut: 5000, extendedTimeOut: 1000, positionClass: 'toast-bottom-left' })
						return
					}

					let isImage = this.checkFileIsImage(files[i].name)
					if (isImage) {
						var src = URL.createObjectURL(files[i]) // set src to blob url
						let maxId = this.generateMaxId(this.listUrlImage)
						temps.push({
							id: maxId,
							url: src,
						})
						tempFiles.push(files[i])
					} else {
						this.notify.error(this.l('IncorrectImageFormat'), '', { timeOut: 5000, extendedTimeOut: 1000, positionClass: 'toast-bottom-left' })
						break
					}
				}
				if (!isError) {
					this.listUrlImage = [...this.listUrlImage, ...temps]
					this.filesToUpload = [...this.filesToUpload, ...tempFiles]

					if (this.listUrlImage.length > 0) {
						this.changeMainImage.emit(this.listUrlImage)
					}
				}
			}
		}

		//Edit mode
		if (this.modeImage == ModeImage.Edit) {
			if (this.listUrlImage.length + files.length > 20) {
				this.notify.error(this.l('Upload_FileCount_Err'), '', { timeOut: 5000, extendedTimeOut: 1000, positionClass: 'toast-bottom-left' })
				return
			}

			let isError = false
			for (let i = 0; i < files.length; i++) {
				if (files[i].size > this.maxLength) {
					isError = true
					this.notify.error(this.l('Upload_FileSize_Err'), '', { timeOut: 5000, extendedTimeOut: 1000, positionClass: 'toast-bottom-left' })
					return
				}
			}
			if (!isError && files.length) this.filesNeedUpload.emit(files)
		}
	}

	generateMaxId(list: any[]): number {
		let values = []
		if (list.length == 0) {
			return 1
		} else if (list.length == 1) {
			return +list[0].id + 1
		} else {
			values = list.map((item) => {
				return item.id
			})

			let max = Math.max(...values)
			return max + 1
		}
	}

	drop(event: CdkDragDrop<any>) {
		console.log(event)
		if (event.previousContainer.data['index'] != event.container.data['index']) {
			this.listUrlImage[event.previousContainer.data['index']] = { ...event.container.data['item'] }
			this.listUrlImage[event.container.data['index']] = { ...event.previousContainer.data['item'] }
			event.currentIndex = 0

			if (this.modeImage == ModeImage.AddNew) {
				;[this.filesToUpload[event.previousContainer.data['index']], this.filesToUpload[event.container.data['index']]] = [
					this.filesToUpload[event.container.data['index']],
					this.filesToUpload[event.previousContainer.data['index']],
				]
			}

			if (event.previousContainer.data['index'] == 0 || event.container.data['index'] == 0) {
				this.dropChangedMainImage()
			}

			this.dropChangedOrdinalImage()
		}
	}

	dropChangedMainImage() {
		this.changeMainImage.emit(this.listUrlImage[0])
	}

	dropChangedOrdinalImage() {
		if (this.modeImage == ModeImage.Edit) {
			this.changeOrdinalImage.emit(this.listUrlImage)
		}
	}

	showImageModal(item: any) {
		this.imageModal.show(item)
	}

	deleteImage(id: number, isMainImage: boolean) {
		const swalWithBootstrapButtons = Swal.mixin({
			customClass: {
				confirmButton: 'btn btn-danger btn_delete',
				cancelButton: 'btn btn-primary btn_cancel',
			},
			buttonsStyling: false,
		})

		swalWithBootstrapButtons
			.fire({
				title: this.l('Hành động này sẽ xóa ảnh'),
				text: this.l('Hãy chắc chắn bạn muốn xóa'),
				imageUrl: this.appRootUrl() + 'assets/common/images/danger.PNG',
				imageWidth: 60,
				imageHeight: 50,
				showCancelButton: true,
				showCloseButton: true,
				confirmButtonText: '<i class="fas fa-trash"></i>' + this.l('Xóa'),
				cancelButtonText: '<i class="fas fa-check"></i>' + this.l('Hủy'),
			})
			.then((result) => {
				if (result.value) {
					var index = this.listUrlImage.findIndex((e) => e.id == id)
					if (index > -1) {
						this.listUrlImage.splice(index, 1)
						this.filesToUpload.splice(index, 1)

						if (isMainImage) {
							this.changeMainImage.emit(this.listUrlImage[0])
						}
					}

					// Remove file from FileList dependency modeImage
					if (this.modeImage == ModeImage.Edit) {
						this.deleteImageWhenEdit.emit(id)

						if (isMainImage) {
							this.changeOrdinalImage.emit(this.listUrlImage[0])
						}
					}
				}
			})
	}

	uploadImageWhenEdit(files: FileList, uploadUrl: string, productId: string): Observable<any> {
		return this.uploadService.uploadImageAndCreate(files, uploadUrl, productId)
	}

	uploadImage(uploadUrl: string, productId: string): Observable<any> {
		return this.uploadService.uploadImage(this.filesToUpload, uploadUrl, productId)
	}

	getExtension(filename) {
		var parts = filename.split('.')
		return parts[parts.length - 1]
	}

	checkFileIsImage(filename) {
		var ext = this.getExtension(filename)
		switch (ext.toLowerCase()) {
			case 'jpg':
			case 'jpeg':
			case 'png':
				return true
		}
		return false
	}
}
