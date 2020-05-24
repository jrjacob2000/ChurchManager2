import {Component, OnDestroy, OnInit} from '@angular/core';
import { User } from '../data-structure/user-model';
import { AuthenticationService } from '../services/authentication.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';


declare var $;

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {

  public registerForm: FormGroup;
  submitted = false;
  public user :  User = new User();

  constructor(private autService:AuthenticationService, private formBuilder: FormBuilder) {
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

    this.registerForm = this.formBuilder.group({
          FullName : ['', Validators.required],
          Username: ['', Validators.required],
          Email: ['', Validators.required],
          PasswordHash: ['', Validators.required],
          ReTypePasswordHash : ['', Validators.required]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.registerForm.controls; }

  ngOnDestroy(): void {
    $('body').removeClass('hold-transition login-page');
  }

  onSubmit(){
    //console.log(this.user);
    this.submitted = true;
    if (this.registerForm.invalid) {
      return;
  }

    this.user = this.registerForm as unknown as User;
    // this.user.FullName = this.f.FullName.value;
    // this.user.Email= this.f.Email.value;
    // this.user.PasswordHash= this.f.PasswordHash.value;
    
    this.autService.register(this.user).subscribe((data) =>
    {
      console.log(data);
    },
    (error) =>{
      console.log(error);
    });
  }
}
