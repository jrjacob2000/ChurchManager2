import { Component, OnInit, OnDestroy, Renderer2 } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from '../utils/services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  public submitted = false;
  public loginForm: FormGroup;
  public loading = false;
  public errorMessage = null;

  constructor(
    private renderer: Renderer2,
    private toastr: ToastrService,
    private authService: AuthenticationService,
    private router: Router
  ) {}

  ngOnInit() {
    this.renderer.addClass(document.body, 'login-page');
    this.loginForm = new FormGroup({
      email: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.required)
    });
  }

   // convenience getter for easy access to form fields
   get f() { return this.loginForm.controls; }

  logIn() {

    this.errorMessage = null;
    this.submitted = true;
    this.loading = true;

    if (this.loginForm.valid) {
      this.authService.login({Email: this.f.email.value, PasswordHash: this.f.password.value})
      .subscribe((data) => {
        this.loading = false;
        this.router.navigateByUrl("/");
      },
      (error) =>{
        this.loading = false;
        let message = "unable to login";
        if(error.status == 401)
        {
          message = "Invalid user name or password"
          this.errorMessage = message;
        }
        else
          this.toastr.error(message, 'Error!');
      });
    } 

  }

  ngOnDestroy() {
    this.renderer.removeClass(document.body, 'login-page');
  }
}
