import { Component, OnInit } from '@angular/core';
import { LoginService } from '../../auth/Services/login.service';
import { DoctorService } from '../Service/doctor.service';
import { data } from 'jquery';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-plans',
  templateUrl: './plans.component.html',
  styleUrls: ['./plans.component.scss']
})
export class PlansComponent implements OnInit {
  doctorID !: string
  data!: any[]
  LossWeight :any[]=[]
  GainWeight :any[]=[]
  Muscles :any[] =[]
  plansId: number = 0

  constructor(private _loginService: LoginService, private _doctorService: DoctorService, private route: Router) {

  }
  ngOnInit(): void {
    this.doctorID = this._loginService.getUserId();
    this.GetDoctorPlans(this.doctorID)

  }
  GetDoctorPlans(doctorID: string) {
    this._doctorService.GetDoctorPlans(this.doctorID).subscribe((resp) => {
      if (this.GainWeight.length > 0 || this.LossWeight.length > 0 || this.Muscles.length > 0) {
        this.GainWeight=[]
        this.Muscles=[]
        this.LossWeight=[]
          }

        this.data = resp
      for (let i = 0; i < this.data.length; i++) {

        switch (this.data[i].goal) {
          case 'weightLoss':
            this.LossWeight.push(this.data[i]);
            break;
          case 'weightGain':
            this.GainWeight.push(this.data[i]);
            break;
          case 'muscleBuilding':
            this.Muscles.push(this.data[i]);
            break;
         
          default:
            break;
        }
      }
this.Muscles.reverse()
this.LossWeight.reverse()
this.GainWeight.reverse()

    })
  }
  GOToplan(planID: number) {
    this.route.navigate([`/doctor/dash/Plan/${planID}`])
  }
  Delete() {
    this._doctorService.Delete(this.plansId).subscribe((resp) => {
      if (resp.msg == 'deleted') {
        this.GetDoctorPlans(this.doctorID)
      }
    })
  }
  Update(PlanID: number) {
    this.route.navigate([`/doctor/dash/UpdatePlan/${PlanID}`])
  }

  assignPlan(planID: number) {
    this.plansId = planID
  }

}
