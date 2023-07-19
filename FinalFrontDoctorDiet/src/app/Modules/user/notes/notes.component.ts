import { Component, OnInit } from '@angular/core';
import { NotesService } from '../services/notes.service';
import { LoginService } from '../../auth/Services/login.service';
import { INoteGetPatientData } from '../interface/INoteGetPatientData';
import { Router } from '@angular/router';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.scss']
})
export class NotesComponent implements OnInit {

  doctorId: string = this._LoginService.getUserId();
  data: INoteGetPatientData[] = []
  errorMsg: string = '';
  behaviorSubjectforNotificationsnumber = 0;
  behaviorSubjectforCountNotifications = 0;
  behaviorSubjectforHTMLDivs: HTMLElement[] = [];
  NewNotificationsclicked=0;
  constructor(private router: Router, private _NoteService: NotesService, private _LoginService: LoginService) {

  }
  ngOnInit(): void {
    this.GetAllPatientsNotes();
  }

  navigate(dayCustomPlanId: any, patientId: any, item: any): void {

    this._NoteService.UpdatePatientNoteStatus(item.id).subscribe({
      next: data => console.log(data),
      error: err => console.log(err)
    })


    if (item.seen == false) {
      this._NoteService.setValueforNotificationCounter(--this.behaviorSubjectforCountNotifications)

    }
    this.router.navigate([`/doctor/dash/CustomPlanDayMeal/${dayCustomPlanId}`], { state: { patientId: patientId } });

  }

  GetAllPatientsNotes() {



    this._NoteService.getValueofHtmlDivs().subscribe({
      next: data => {
        this.behaviorSubjectforHTMLDivs = data
      },
      error: err => console.log(err)
    })

    this._NoteService.getValueofNotificationCounter().subscribe({
      next: data => {
        this.behaviorSubjectforCountNotifications = data
      },
      error: err => console.log(err)
    })

    this._NoteService.getValueofNotificationNum().subscribe({
      next: data => {
        this.behaviorSubjectforNotificationsnumber = data
      },
      error: err => console.log(err)
    })



    this._NoteService.GetPatientsNotesByDoctorId(this.doctorId).subscribe((resp) => {
      this.data = resp.reverse();


    }, error => {
      this.errorMsg = "Errrrrror"
    })


  }

  NewNotificationsclickedFunc(){
    this.NewNotificationsclicked = 1;
    this._NoteService.setValueforNotificationNum(0);
  }



}
