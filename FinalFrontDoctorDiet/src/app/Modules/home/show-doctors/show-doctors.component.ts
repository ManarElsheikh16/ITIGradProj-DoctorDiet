import { Component, OnInit } from '@angular/core';
import { HomepageComponent } from '../homepage/homepage.component';
import { HomeServicesService } from '../home-services.service';
import { IDoctor } from '../../shared/Interface/IDoctor'
import { Router } from '@angular/router';
@Component({
  selector: 'app-show-doctors',
  templateUrl: './show-doctors.component.html',
  styleUrls: ['./show-doctors.component.scss']
})
export class ShowDoctorsComponent {

  doctors: IDoctor[] = []
  constructor(private homeService: HomeServicesService, private _router: Router) { }

  ngOnInit() {
    this.getDoctors()
  }
  close() {
    this.homeService.CloseDoctors();

  }
  getDoctors() {
    this.homeService.getAllDoctors().subscribe({
      next: data => {
        this.doctors = data;

      },
      error: err => {
      }
    })
  }

}