import {Component, OnDestroy, OnInit} from '@angular/core';
import { UserLogin } from '../data-structure/user-model';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

declare var $;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {

  public userLogin: UserLogin = new UserLogin();
  
  constructor(private autService : AuthenticationService, private router: Router) {
  }

  ngOnInit() {
    $('body').addClass('hold-transition login-page');
    $(() => {
      $('input').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue',
        increaseArea: '20%' /* optional */
      });
    });
  }

  ngOnDestroy(): void {
    $('body').removeClass('hold-transition login-page');
  }

  onLogin(){
    console.log(this.userLogin);
    this.autService.login(this.userLogin).subscribe((data) =>
    {
      console.log(data);
      this.router.navigateByUrl("/members");
    },
    (error) =>{
      console.log(error);
    });
  }
}
