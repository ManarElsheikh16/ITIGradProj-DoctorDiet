import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';


const routes: Routes = [
  { path: '', loadChildren: () => import('./Modules/home/home.module').then(m => m.HomeModule) },

  { path: 'auth', loadChildren: () => import('./Modules/auth/auth.module').then(m => m.AuthModule) },
  { path: 'doctor', loadChildren: () => import('./Modules/doctor/doctor.module').then(m => m.DoctorModule) ,canActivate: [AuthGuard] },
  { path: 'user', loadChildren: () => import('./Modules/user/user.module').then(m => m.UserModule),canActivate: [AuthGuard] },
  { path: '**', loadChildren: () => import('./Modules/home/home.module').then(m => m.HomeModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{useHash:true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
