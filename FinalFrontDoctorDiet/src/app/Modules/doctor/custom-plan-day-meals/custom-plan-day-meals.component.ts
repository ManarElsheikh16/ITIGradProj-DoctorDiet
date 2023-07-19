import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IDoctorNoteData } from '../../user/interface/IDoctorNoteData';
import { NotesService } from '../../user/services/notes.service';
import { FormBuilder, Validators } from '@angular/forms';
import { LoginService } from '../../auth/Services/login.service';
import { DoctorService } from '../Service/doctor.service';

@Component({
  selector: 'app-custom-plan-day-meals',
  templateUrl: './custom-plan-day-meals.component.html',
  styleUrls: ['./custom-plan-day-meals.component.scss']
})
export class CustomPlanDayMealsComponent {
  data: any[] = []
  planId!: any;
  breakFast: any[] = [];
  Lunch: any[] = [];
  Dinner: any[] = [];
  snaks: any[] = [];
  sohor:any[]=[];
  state: any;
isNoted:boolean=false
  patientId: string='';
  currentDate = new Date();
  currentDateTime = this.currentDate;
  DoctorNoteData:IDoctorNoteData={
    id: 0,
    patientId: '',
    doctorId: '',
    date: this.currentDateTime,
    text:''
  }
  showDone:boolean=false

  constructor(private _NoteService:NotesService,private FormBuilder:FormBuilder,private router:Router,private DoctorService: DoctorService, private _ActivatedRoute: ActivatedRoute,private _LoginService:LoginService) {
    console.log(this.state)
    this.state=this.router.getCurrentNavigation()?.extras.state;
    if(this.state!=null){
      console.log(this.state)
      console.log("rrrrrrrrr")
      this.patientId=this.state['patientId']
      this.isNoted=true
       }else{
      console.log("ddddd")

      this.isNoted=false
        
       }
   
  }

  DoctorNoteForm=this.FormBuilder.group({
    text:['',Validators.required],
 })

 get text()
 {
   return this.DoctorNoteForm.get('text');
 }

  ngOnInit(): void {
    this._ActivatedRoute.paramMap.subscribe(params => {
      this.planId = params.get('id');
      this.DoctorService.GetCustomMealById(this.planId).subscribe((response: any) => {
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
              this.snaks.push(this.data[i]);
            }
            else if (this.data[i].category == 4) {
              this.sohor.push(this.data[i]);
            }
          }
        }
      });
    });

    }


    AddDoctorNote()
    {

      this.DoctorNoteData={
        id: 0,
        patientId: this.patientId,
        doctorId: this._LoginService.getUserId(),
        date: this.currentDateTime,
        text: this.text?.value
      }

       this._NoteService.AddDoctorNote(this.DoctorNoteData).subscribe(resp => {


     this.text?.reset();
     if(resp.status=="Success")
     {
      this.showDone=!this.showDone;
     }
       },(error) => {
           console.log("A7la Error:",error);
        }
       )
    }

    onSubmitDocNote()
    {
       if(this.DoctorNoteForm.valid)
       {
       }
    }
}
