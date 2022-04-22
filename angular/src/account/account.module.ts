import { AbpModule } from '@abp/abp.module';
import * as ngCommon from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClientJsonpModule } from '@angular/common/http';
import { CommonModule } from '@shared/common/common.module';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { RecaptchaModule } from 'ng-recaptcha';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AccountRoutingModule } from './account-routing.module';
import { AccountComponent } from './account.component';
import { AccountRouteGuard } from './auth/account-route-guard';
import { ConfirmEmailComponent } from './email-activation/confirm-email.component';
import { EmailActivationComponent } from './email-activation/email-activation.component';
import { LanguageSwitchComponent } from './language-switch.component';
import { LoginComponent } from './login/login.component';
import { LoginService } from './login/login.service';
import { SendTwoFactorCodeComponent } from './login/send-two-factor-code.component';
import { ValidateTwoFactorCodeComponent } from './login/validate-two-factor-code.component';
import { ForgotPasswordComponent } from './password/forgot-password.component';
import { ResetPasswordComponent } from './password/reset-password.component';
import { PayPalPurchaseComponent } from './payment/paypal/paypal-purchase.component';
import { StripePurchaseComponent } from './payment/stripe/stripe-purchase.component';
import { StripeSubscribeComponent } from './payment/stripe/stripe-subscribe.component';
import { StripeUpdateSubscriptionComponent } from './payment/stripe/stripe-update-subscription.component';
import { BuyEditionComponent } from './payment/buy.component';
import { UpgradeEditionComponent } from './payment/upgrade.component';
import { ExtendEditionComponent } from './payment/extend.component';
import { RegisterTenantResultComponent } from './register/register-tenant-result.component';
import { RegisterTenantComponent } from './register/register-tenant.component';
import { RegisterComponent } from './register/register.component';
import { SelectEditionComponent } from './register/select-edition.component';
import { TenantRegistrationHelperService } from './register/tenant-registration-helper.service';
import { TenantChangeModalComponent } from './shared/tenant-change-modal.component';
import { TenantChangeComponent } from './shared/tenant-change.component';
import { OAuthModule } from 'angular-oauth2-oidc';
import { PaymentHelperService } from './payment/payment-helper.service';
import { ProductListComponent } from './product/product-list/product-list.component';
import { EditorModule, InputMaskModule, PaginatorModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';
import { ProductDetailComponent } from './product/product-detail/product-detail.component';
import { NgImageSliderModule } from 'ng-image-slider';
import { DataViewModule } from 'primeng/dataview';
import { UserRegisterComponent } from './user-register/user-register.component';
import { AboutUsComponent } from './about-us/about-us.component';
import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { BsDatepickerConfig, BsDatepickerModule, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { ViewBlogModalComponent } from './product/view-blog-modal/view-blog-modal.component';
import { TabsModule } from 'ngx-bootstrap';

@NgModule({
    imports: [
        ngCommon.CommonModule,
        FormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        RecaptchaModule.forRoot(),
        ModalModule.forRoot(),
        AbpModule,
        CommonModule,
        UtilsModule,
        ServiceProxyModule,
        AccountRoutingModule,
        OAuthModule.forRoot(),
        PaginatorModule,
        EditorModule,
        InputMaskModule,
        TableModule,
        NgImageSliderModule,
        DataViewModule,
        NgxSliderModule,
        BsDatepickerModule.forRoot(),
        TabsModule
    ],
    declarations: [
        AccountComponent,
        TenantChangeComponent,
        TenantChangeModalComponent,
        LoginComponent,
        RegisterComponent,
        RegisterTenantComponent,
        RegisterTenantResultComponent,
        SelectEditionComponent,
        ForgotPasswordComponent,
        ResetPasswordComponent,
        EmailActivationComponent,
        ConfirmEmailComponent,
        SendTwoFactorCodeComponent,
        ValidateTwoFactorCodeComponent,
        LanguageSwitchComponent,
        BuyEditionComponent,
        UpgradeEditionComponent,
        ExtendEditionComponent,
        PayPalPurchaseComponent,
        StripePurchaseComponent,
        StripeSubscribeComponent,
        StripeUpdateSubscriptionComponent,
        ProductListComponent,
        ProductDetailComponent,
        UserRegisterComponent,
        AboutUsComponent,
        ViewBlogModalComponent
    ],
    providers: [
        LoginService,
        TenantRegistrationHelperService,
        PaymentHelperService,
        AccountRouteGuard,
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
    ]
})
export class AccountModule {

}
