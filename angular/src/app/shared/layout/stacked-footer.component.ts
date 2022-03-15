import { Component, Injector, OnInit, Input } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    templateUrl: './stacked-footer.component.html',
    selector: 'stacked-footer-bar'
})
export class StackedFooterComponent extends AppComponentBase implements OnInit {

    releaseDate: string;

    constructor(
        injector: Injector
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.releaseDate = this.appSession.application.releaseDate.format('YYYYMMDD');
    }
}
