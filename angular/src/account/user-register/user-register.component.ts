import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrUpdateUserInput, PasswordComplexitySetting, UserEditDto, UserRoleDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css'],
  animations: [appModuleAnimation()]
})
export class UserRegisterComponent extends AppComponentBase implements OnInit {

  user: UserEditDto = new UserEditDto();
  sendActivationEmail = true;
  setRandomPassword = false;
  passwordComplexityInfo = '';
  profilePicture: string;
  roles: UserRoleDto[];
  saving = false;
  canChangeUserName = true;
  isTwoFactorEnabled: boolean = this.setting.getBoolean('Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled');
  isLockoutEnabled: boolean = this.setting.getBoolean('Abp.Zero.UserManagement.UserLockOut.IsEnabled');
  passwordComplexitySetting: PasswordComplexitySetting = new PasswordComplexitySetting();
  constructor(
    injector: Injector,
    private _userService: UserServiceProxy,
    private router: Router
  ) {
    super(injector);
  }

  ngOnInit() {
  }

  save(): void {
    let input = new CreateOrUpdateUserInput();

    input.user = this.user;
    input.setRandomPassword = this.setRandomPassword;
    input.sendActivationEmail = this.sendActivationEmail;
    input.user.isActive = true;
    input.assignedRoleNames.push("ceaac346ada04e04a316941881082dfc");

    this._userService.createOrUpdateUser(input)
      .pipe(finalize(() => { this.saving = false; }))
      .subscribe(() => {
        this.notify.info(this.l('RegisterSuccessfully'));
        this.router.navigate(['/account/login']);
      });
  }


}
