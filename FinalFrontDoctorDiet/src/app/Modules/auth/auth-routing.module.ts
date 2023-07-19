import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';

import { ForgetComponent } from './forget/forget.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { DoctorRegisterComponent } from './doctor-register/doctor-register.component';




const routes: Routes = [

  { path: 'Login',component:LoginComponent},
  { path: 'Register',component:RegisterComponent},
  {path:'DoctorRegister',component:DoctorRegisterComponent},
  { path: 'ForgetPass',component:ForgetComponent},
  { path: 'ChangePass',component:ChangePasswordComponent}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],

  exports: [RouterModule]
})
export class AuthRoutingModule { }
