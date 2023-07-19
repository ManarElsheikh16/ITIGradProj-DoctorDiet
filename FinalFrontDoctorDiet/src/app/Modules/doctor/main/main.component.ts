import { Component, OnInit } from '@angular/core';
import { LoginService } from '../../auth/Services/login.service';
import { DoctorService } from '../Service/doctor.service';
import { NotesService } from '../../user/services/notes.service';
import { INoteGetPatientData } from '../../user/interface/INoteGetPatientData';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {

  userName: String | undefined;
  decodedImage: any;
  behaviorSubjectforNotificationsnumber: number = 0;
  behaviorSubjectforSubscribtionsnumber: number = 0;
  behaviorSubjectforCountNotifications: number = 0;
  data: INoteGetPatientData[] = [];
  doctorId: string = this.loginservice.getUserId();
  errorMsg: any;
  counter: number = 0;
  CounterSubscription: number = 0

  constructor(private loginservice: LoginService, private _doctorService: DoctorService, private _NoteService: NotesService) {

  }
  ngOnInit(): void {
    this._NoteService.GetPatientsNotesByDoctorId(this.doctorId).subscribe((resp) => {
      this.data = resp;
      this.behaviorSubjectforNotificationsnumber = 0;

      this._doctorService.getAllWaitingPatients(this.doctorId).subscribe((response) => {
        this.behaviorSubjectforSubscribtionsnumber = response.length;
        this._NoteService.setValueforSubscibtionNum(this.behaviorSubjectforSubscribtionsnumber);
      })

      this.data.forEach(element => {
        if (element.seen == false) {
          this._NoteService.setValueforNotificationCounter(++this.behaviorSubjectforCountNotifications);

        }
      });


    }, error => {
      this.errorMsg = "Errrrrror"
    })

    

    

    

    this._NoteService.getValueofNotificationNum().subscribe({
      next: data => {
        this.behaviorSubjectforNotificationsnumber = data

      },
      error: err => console.log(err)
    })
    this._NoteService.getValueofNotificationCounter().subscribe({
      next: data => {
        this.behaviorSubjectforCountNotifications = data

      },
      error: err => console.log(err)
    })
    
    this._NoteService.getValueofSubscibtionNum().subscribe({
      next: data => this.behaviorSubjectforSubscribtionsnumber = data,
      error: err => console.log(err)
    })


    this.userName = this.loginservice.getUserName();
    this._doctorService.GetDoctorImg(this.loginservice.getUserId()).subscribe((resp) => {
      this.decodedImage = resp.img
    })

  }

  resetBehaviourNotification() {

  }

  resetBehavioursubs() {
  }

}
