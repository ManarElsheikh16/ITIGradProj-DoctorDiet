import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomepageComponent } from './homepage/homepage.component';
import { ShowDoctorsComponent } from './show-doctors/show-doctors.component';
import { KnowYourDocComponent } from '../doctor/know-your-doc/know-your-doc.component';




const routes: Routes = [
  { path: '',component:HomepageComponent},
  
  { path: 'Home',component:HomepageComponent},
  { path: 'showDoctor',component:ShowDoctorsComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }