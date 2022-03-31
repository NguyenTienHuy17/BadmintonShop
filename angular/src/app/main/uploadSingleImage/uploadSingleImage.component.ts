import { Component, OnInit, Input, Output, EventEmitter, ElementRef, ViewChild, Injector } from '@angular/core'
import { UploadSingleImageService } from './uploadSingleImage.service'
import { Observable } from 'rxjs'
import { AppComponentBase } from '@shared/common/app-component-base'

@Component({
	selector: 'app-uploadSingleImage',
	templateUrl: './uploadSingleImage.component.html',
	styleUrls: ['./uploadSingleImage.component.less'],
})
export class UploadSingleImageComponent extends AppComponentBase implements OnInit {
	@ViewChild('inputFile', { static: true }) inputFile: ElementRef

	@Input() previewUrl: any = null
	@Input() disabled: boolean = false

	@Output() fileOutput: EventEmitter<any> = new EventEmitter<any>()
	@Output() isDelete: EventEmitter<boolean> = new EventEmitter<boolean>()

	fileData: File = null
	fileUploadProgress: string = null
	uploadedFilePath: string = null

	constructor(injector: Injector, private uploadSingleService: UploadSingleImageService) {
		super(injector)
	}

	ngOnInit() {}

	fileProgress(fileInput: any) {
		this.fileData = <File>fileInput.target.files[0]
		this.isDelete.emit(false)
		this.preview()
	}

	preview() {
		var mimeType = this.fileData.type
		if (mimeType.match(/image\/*/) == null) {
			this.notify.error(this.l('IncorrectImageFormat'), '', { timeOut: 5000, extendedTimeOut: 1000, positionClass: 'toast-bottom-left' })
			this.fileData = null
			this.fileOutput.emit(this.fileData)
		} else {
			var reader = new FileReader()
			reader.readAsDataURL(this.fileData)
			reader.onload = (_event) => {
				this.previewUrl = reader.result
			}
			this.fileOutput.emit(this.fileData)
		}
	}

	deleteImage() {
		this.previewUrl = null
		this.fileData = null
		this.inputFile.nativeElement.value = ''
		this.isDelete.emit(true)
	}

	uploadImage(uploadUrl: string, productId: string): Observable<any> {
		return this.uploadSingleService.uploadSingleImage(this.fileData, uploadUrl, productId)
	}
}
