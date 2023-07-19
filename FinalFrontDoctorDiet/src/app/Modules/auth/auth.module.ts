import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthRoutingModule } from './auth-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { FileUploadModule } from 'primeng/fileupload';
import { ForgetComponent } from './forget/forget.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { DoctorRegisterComponent } from './doctor-register/doctor-register.component';



@NgModule({
  declarations: [
    RegisterComponent,
    LoginComponent,
    ForgetComponent,
    ChangePasswordComponent,
    DoctorRegisterComponent,


  ],
  providers: [


  ],
  imports: [
    AuthRoutingModule,
    CommonModule,
    RouterLink,
    ReactiveFormsModule,
    HttpClientModule,
    FileUploadModule


  ]

})
export class AuthModule { }
