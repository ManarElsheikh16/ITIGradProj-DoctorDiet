import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PasswordValidator } from '../CustomValidator/PassValidator';
import { LoginService } from '../Services/login.service';
import * as $ from 'jquery';
import jwtDecode from 'jwt-decode';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  decodedToken: any;

  constructor(private formBuilder: FormBuilder, private _router: Router, private _LoginService: LoginService) { }
  invaliduser: string = '';
  userRole: string = '';
  loginUserRole :any
  LoginForm = this.formBuilder.group({
    username: ['', [Validators.required, Validators.minLength(5)]],

    password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(10), PasswordValidator]],

  });
  get UserName() {
    return this.LoginForm.get('username');
  }

  get password() {
    return this.LoginForm.get('password');
  }
  ngOnInit(): void {

  }

  onSubmit(LoginForm: FormGroup) {
    if (this.LoginForm.valid) {


      this._LoginService.Login(this.LoginForm.value).subscribe((resp) => {
        if (resp.messege == 'Success') {
          localStorage.setItem('userToken', resp.token);
          let encodedUserData = JSON.stringify(localStorage.getItem('userToken'));
          this.decodedToken = jwtDecode(encodedUserData);
              this.loginUserRole = this.decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
           this._LoginService.saveUserData();
          // this.userRole=this._LoginService.getUserRole();
          if( this.loginUserRole=='Patient'){
                  this._router.navigate(['home'])
          }
          else if(this.loginUserRole=='Doctor'){
            this._router.navigate(['doctor/dash/Welcome'])
          }
          else{
            this._router.navigate(['home'])
          }

        }

      }, error => {
        this.invaliduser = " اسم المستخدم او كلمه سر غير صحيحه";
      }
      )
      

    } else {

    }
  }
 show() {
    var x = document.getElementById("password")as HTMLInputElement;
    $("#eye").toggleClass('fa-eye fa-eye-slash');
   if (x.type == "password") {
      x.type = "text";
    } else {
      x.type = "password";
    }
  }

  

}


