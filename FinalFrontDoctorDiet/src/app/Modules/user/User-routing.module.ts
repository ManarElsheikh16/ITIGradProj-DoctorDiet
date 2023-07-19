import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PlanComponent } from './plan/plan.component';
import { ProfileComponent } from './profile/profile.component';



const routes: Routes = [
 
  { path: 'Plan',component:PlanComponent},
  { path: 'Profile',component:ProfileComponent },
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }