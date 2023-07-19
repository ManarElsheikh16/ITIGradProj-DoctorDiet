import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CustomPlanService } from '../Service/custom-plan.service';
import { UserService } from '../../user/services/user.service';

@Component({
  selector: 'app-custom-plan-day',
  templateUrl: './custom-plan-day.component.html',
  styleUrls: ['./custom-plan-day.component.scss']
})
export class CustomPlanDayComponent {
  data:any
  customplanId !:any
  currentDay!:any
 constructor(private customPlanService:CustomPlanService,private _ActivatedRoute:ActivatedRoute,private  _UserService:UserService)  {


 }
  ngOnInit(): void {
    this._ActivatedRoute.paramMap.subscribe(params => {
      this.customplanId = params.get('customId');
      this.customPlanService.GetDaysCustomPlan(this.customplanId).subscribe(Response => {
        this.data = Response;
        console.log(this.data)
      });

      this._UserService.getCurrentDay(this.customplanId).subscribe({
        next : data => {this.currentDay = data}
      })
    });

  }
}
