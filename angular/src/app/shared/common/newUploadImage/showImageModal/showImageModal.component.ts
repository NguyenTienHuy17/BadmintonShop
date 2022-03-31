import { Component, OnInit, ViewChild, Injector } from '@angular/core'
import { ModalDirective } from 'ngx-bootstrap'
import { AppComponentBase } from '@shared/common/app-component-base'
@Component({
	selector: 'app-showImageModal',
	templateUrl: './showImageModal.component.html',
	styleUrls: ['./showImageModal.component.less'],
})
export class ShowImageModalComponent extends AppComponentBase implements OnInit {
	@ViewChild('showImageModal', { static: true }) modal: ModalDirective
	srcImage: any
	constructor(injector: Injector) {
		super(injector)
	}

	ngOnInit() {}

	show(item: any) {
		this.srcImage = item
		this.modal.show()
	}

	close() {
		this.srcImage = undefined
		this.modal.hide()
	}
}
