import { Component, OnInit } from '@angular/core';
import { IPatient } from '../../doctor/Interface/IPatient';
import { UserService } from '../services/user.service';
import { LoginService } from '../../auth/Services/login.service';
import { INoteGetDoctorData } from '../interface/INoteGetDoctorData';
import { IDoctorNoteDto } from '../interface/IDoctorNoteDto';
import { NotesService } from '../services/notes.service';
import { IDocIdResponse } from '../interface/IDocIdResponse';
import { IPatientNoteData } from '../interface/IPatientNoteData';
import { FormBuilder, Validators } from '@angular/forms';
import { error } from 'jquery';
import { HomeServicesService } from '../../home/home-services.service';
@Component({
  selector: 'app-plan',
  templateUrl: './plan.component.html',
  styleUrls: ['./plan.component.scss']
})
export class PlanComponent implements OnInit {
  constructor(private formBuilder: FormBuilder,
    private _NoteService: NotesService, private _UserService: UserService, private homeService: HomeServicesService,
    private Auth: LoginService) {
    this.userID = this.Auth.getUserId()


    this._UserService.GetIFPatientIsComfirmed(this.userID).subscribe((res) => {
      if (res.msg == "Confirmed") {
        this.ISsubscribe = "Confirmed"
        this.docId = res.doctorid
        this.DoctorNote.doctorId = res.doctorid
        this.DoctorNote.patientId = this.userID
        this.getPatientCurrentDay();
        this.GetAllDocNotesForSpecificPatient(this.DoctorNote)


      }
      else if (res.msg == "Canceled") {
        this.ISsubscribe = "Canceled"
      }
      else if (res.msg == "Waiting") {
        this.ISsubscribe = "Waiting"
      }
      else if (res.msg == "Done") {
        this.ISsubscribe = "Done"
      }
      else {
        this.ISsubscribe = "NotConfirmed"
      }
    })
  }


  userID: string = ''
  data: INoteGetDoctorData[] = []
  errorMsg: string = ""
  docId!: string
  DoctorNote: IDoctorNoteDto = {
    doctorId: this.docId,
    patientId: this.userID
  }


  patientNoteDataForm = this.formBuilder.group({
    text: ['', Validators.required],
  });
  get text() {
    return this.patientNoteDataForm.get('text');
  }
  currentDate = new Date();
  currentDateTime = this.currentDate;
  patientNoteData: IPatientNoteData = {
    patientId: '',
    doctorId: '',
    date: this.currentDateTime,
    id: 0,
    dayCustomPlanId: 0,
    text: '',
    seen: false
  }
  CurrentDayCustomPlanId!: number
  showDone: boolean = false
  CustomPlanId: any

  ISsubscribe !: string
  CurrentPatient!: IPatient;
  breakfast: any[] = [];
  lunch: any[] = [];
  dinner: any[] = [];
  snaks: any[] = [];
  sohor: any[] = [];

  ISsohor: boolean = false
  ISbreakfast: boolean = false
  ISlunch: boolean = false
  ISsnaks: boolean = false
  ISdinner: boolean = false
  behaviorSubjectforNotificationsnumber = 0;

  ngOnInit() {


  }

  ShowDoctors() {

    this.homeService.ShowDoctors();
  }

  getPatientCurrentDay() {
    console.log(this.DoctorNote)
    this._UserService.CurrentCustomPlan(this.DoctorNote).subscribe((response) => {
      this.CustomPlanId = response.id;
      if (this.CustomPlanId == 0) {
        this.ISsubscribe = "Done"
      } else {
        this._UserService.getCurrentDay(this.CustomPlanId).subscribe((response) => {
          console.log(response)
          if (response.Id == 0) {
            console.log("tyuioiuytre")
          }
          let CurrentDay = response;
          this.CurrentDayCustomPlanId = CurrentDay.id;
          for (let i = 0; i < CurrentDay['customMeals'].length; i++) {

            switch (CurrentDay['customMeals'][i].category) {
              case 0:
                this.breakfast.push(CurrentDay['customMeals'][i]);
                this.ISbreakfast = true
                break;
              case 1:
                this.lunch.push(CurrentDay['customMeals'][i]);
                this.ISlunch = true
                break;
              case 2:
                this.dinner.push(CurrentDay['customMeals'][i]);
                this.ISdinner = true
                break;
              case 3:
                this.sohor.push(CurrentDay['customMeals'][i]);
                this.ISsohor = true
                break;
              case 4:
                this.snaks.push(CurrentDay['customMeals'][i]);
                this.ISsnaks = true
                break;
              default:
                break;
            }
          }

        })
      }
    })
  }



  GetAllDocNotesForSpecificPatient(DoctorNote: IDoctorNoteDto) {

    this._NoteService.GetDoctorNotes(DoctorNote).subscribe((resp) => {
      this.data = resp;

    }, error => {
      this.errorMsg = "error"
      console.log(error)
    })


  }



  AddPatientNote() {
    this.patientNoteData = {
      patientId: this.userID,
      doctorId: this.docId,
      date: this.currentDateTime,
      id: 0,
      dayCustomPlanId: this.CurrentDayCustomPlanId,
      text: this.text?.value,
      seen: false

    }
    const newElement = document.createElement('div');
    newElement.innerHTML = ``;

    this._NoteService.setValueforHtmlDivs([newElement]);

    this._NoteService.getValueofNotificationNum().subscribe({
      next: data => this.behaviorSubjectforNotificationsnumber = data,
      error: err => console.log(err)
    })

    this._NoteService.setValueforNotificationNum(++this.behaviorSubjectforNotificationsnumber);
    console.log(this.behaviorSubjectforNotificationsnumber)



    this._NoteService.AddPatientNote(this.patientNoteData).subscribe((resp) => {
      console.log(resp);

      this.text?.reset();
      if (resp.status == "Success") {

        this.showDone = !this.showDone;
      }


    }, (error) => {
      console.log('Error:', error);
      this.errorMsg = 'Failed Successfully Ha Ha Ha';


    });
  }






}