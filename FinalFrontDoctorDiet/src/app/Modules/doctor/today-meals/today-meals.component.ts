import { Component } from '@angular/core';
import { DoctorService } from '../Service/doctor.service';
import { ActivatedRoute } from '@angular/router';
import { IMeal } from '../Interface/IMeal';
import { IMealList } from '../Interface/MealList';

@Component({
  selector: 'app-today-meals',
  templateUrl: './today-meals.component.html',
  styleUrls: ['./today-meals.component.scss']
})
export class TodayMealsComponent {
  data: any[] = []
  planId!: any;
  breakFast: any[] = [];
  Lunch: any[] = [];
  Dinner: any[] = [];
  snaks: any[] = [];
  sohor:any[]=[];

  constructor(private doctorService: DoctorService, private _ActivatedRoute: ActivatedRoute) {


  }
  ngOnInit(): void {
 
    this._ActivatedRoute.paramMap.subscribe(params => {
      this.planId = params.get('id');
      this.doctorService.GetMealByDayID(this.planId).subscribe((response: any) => {
      
        this.data = response;
    

        if (this.data && Array.isArray(this.data)) {
        
          for (let i = 0; i < this.data.length; i++) {
       
            if (this.data[i].category == 0) {

              this.breakFast.push(this.data[i]);
            } else if (this.data[i].category == 1) {
            
              this.Lunch.push(this.data[i]);
            } else if (this.data[i].category == 2) {
             
              this.Dinner.push(this.data[i]);
            } else if (this.data[i].category == 3) {
            
              this.sohor.push(this.data[i]);
            }  else if (this.data[i].category == 4) {
              
              this.snaks.push(this.data[i]);
            }
          }
        }
      });
    });
  
    }
}
