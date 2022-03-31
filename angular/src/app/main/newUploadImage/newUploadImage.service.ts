import { Injectable, Inject } from '@angular/core'
import { HttpClient, HttpRequest } from '@angular/common/http'
import { Observable } from 'rxjs'

@Injectable({
	providedIn: 'root',
})
export class NewUploadImageService {
	private http: HttpClient

	constructor(@Inject(HttpClient) http: HttpClient) {
		this.http = http
	}

	uploadImage(filesUpload: File[] | null | undefined, uploadUrl: string | null | undefined, productId: string | null | undefined): Observable<any> {
		const formData: FormData = new FormData()
		for (let i = 0; i < filesUpload.length; i++) {
			formData.append(filesUpload[i].name, filesUpload[i])
		}
		formData.append('productId', productId)
		const config = new HttpRequest('POST', uploadUrl, formData, {
			reportProgress: true,
		})

		return this.http.request(config)
	}

	uploadImageAndCreate(filesUpload: FileList | null | undefined, uploadUrl: string | null | undefined, productId: string | null | undefined): Observable<any> {
		const formData: FormData = new FormData()
		for (let i = 0; i < filesUpload.length; i++) {
			formData.append(filesUpload[i].name, filesUpload[i])
		}
		formData.append('productId', productId)
		const config = new HttpRequest('POST', uploadUrl, formData, {
			reportProgress: true,
		})

		return this.http.request(config)
	}
}
