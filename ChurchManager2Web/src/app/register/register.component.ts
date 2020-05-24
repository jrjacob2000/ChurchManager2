import { Component, OnInit, Renderer2, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from '../utils/services/authentication.service';
import { User } from '../utils/models/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {
  public submitted = false;
  public invalidPassword = true;
  public registerForm: FormGroup;

  constructor(private renderer: Renderer2, private toastr: ToastrService, private authService : AuthenticationService) {}

  ngOnInit() {
    this.renderer.addClass(document.body, 'register-page');
    this.registerForm = new FormGroup({
      fullName : new FormControl(null),
      email: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.required),
      retypePassword: new FormControl(null)
    });
  }

     // convenience getter for easy access to form fields
  get f() { return this.registerForm.controls; }

  ngOnDestroy() {
    this.renderer.removeClass(document.body, 'login-page');
  }

  register()
  {
    this.submitted = true;

    this.invalidPassword = !(this.f.password.value == this.f.retypePassword.value)

    if(this.invalidPassword)
    {

      this.f.retypePassword.setErrors({ mismatch: true });
      this.toastr.error('Password not match', 'Validation Error');
    }

    if(this.registerForm.valid)
    {
        let user = new User();
        user.FullName = this.f.fullName.value;
        user.Email = this.f.email.value;
        user.PasswordHash = this.f.password.value;
        user.UserName = this.f.email.value;
        
        this.authService.register(user).subscribe((data) =>
        {
          this.toastr.success("", 'Successfully registered');
        },
        (error) =>{
          this.toastr.error("something went wrong", 'Failed to Register');
        });
    }
   
  }
}
