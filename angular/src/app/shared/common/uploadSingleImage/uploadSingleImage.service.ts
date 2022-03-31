import { Injectable, Inject } from '@angular/core'
import { HttpClient, HttpRequest } from '@angular/common/http'
import { Observable } from 'rxjs'

@Injectable({
	providedIn: 'root',
})
export class UploadSingleImageService {
	private http: HttpClient

	constructor(@Inject(HttpClient) http: HttpClient) {
		this.http = http
	}

	uploadSingleImage(file: File | null | undefined, uploadUrl: string | null | undefined, productId: string | null | undefined): Observable<any> {
		const formData: FormData = new FormData()
		formData.append(file.name, file)

		formData.append('productId', productId)
		const config = new HttpRequest('POST', uploadUrl, formData, {
			reportProgress: true,
		})

		return this.http.request(config)
	}
}
