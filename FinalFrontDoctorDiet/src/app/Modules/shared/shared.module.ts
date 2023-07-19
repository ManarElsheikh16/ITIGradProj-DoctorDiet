import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavComponent } from './nav/nav.component';
import { FooterComponent } from './footer/footer.component';
import { RouterLink } from '@angular/router';



@NgModule({
  declarations: [
    NavComponent,
    FooterComponent,
  ], 
  
  imports: [
    CommonModule,RouterLink
  ],
  exports:[
    NavComponent,
    FooterComponent
  ]
})
export class SharedModule { }
