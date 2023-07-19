

import { Component, OnInit } from '@angular/core';
import { DoctorService } from '../Service/doctor.service';
import { LoginService } from '../../auth/Services/login.service';

import { Router } from '@angular/router';
import { error } from 'jquery';
import { NotesService } from '../../user/services/notes.service';

@Component({
  selector: 'app-subscrebtion',
  templateUrl: './subscrebtion.component.html',
  styleUrls: ['./subscrebtion.component.scss']
})
export class SubscrebtionComponent implements OnInit {
  waitingPatient: any

  waitingPatients!: any[];
  DoctorId!: string;
  AlertMsg: any;
  showAlert: boolean = false;
  showalertsub: boolean = false;
  behaviorSubjectforSubscribtionsnumber = 0;
  lengthofwaitinglist: any
  constructor(private _DoctorService: DoctorService, private _LoginService: LoginService, private _NoteService: NotesService) { }
  ngOnInit(): void {
    this.getDoctorId();

    this.GetAllWaitingPatient();

  }
 displayAlert(msg: string) {
    this.AlertMsg = msg;
    this.showAlert = true;
    this.showalertsub = true;
    setTimeout(() => {
      this.showAlert = false;

    }, 5000); // Adjust the duration (in milliseconds) as needed
  }
  getDoctorId() {
    this.DoctorId = this._LoginService.getUserId()
  }
  GetAllWaitingPatient() {
    this._DoctorService.getAllWaitingPatients(this.DoctorId).subscribe((response) => {
      this.waitingPatients = response;
      this.lengthofwaitinglist = response.length;
      this._NoteService.setValueforSubscibtionNum(this.lengthofwaitinglist);
    })

  }
  assignPaitent(paitentId: string) {
    this.waitingPatient = {
      "patientId": paitentId,
      "doctorID": this.DoctorId
    }
  }
  AcceptPatient() {

    this.showSpinner = true;
    this.makeApiRequest();


    this._NoteService.getValueofSubscibtionNum().subscribe({
      next: data => {
        this.behaviorSubjectforSubscribtionsnumber = data
        console.log(data)
      },
      error: err => console.log(err)
    })


    console.log(this.waitingPatient);
    this._DoctorService.acceptPatient(this.waitingPatient).subscribe((res) => {
      clearTimeout(this.timeoutId);
      this.showSpinner = false;
      console.log(res)
      if (res.msg == "NotFound") {
        this.displayAlert("ليس لديك خطه مناسبه للمريض ")

      }
      else if (res.msg == 'Confirmed') {
        this.GetAllWaitingPatient()

      }
    }, error => {

      console.log("err", error)
    }
    )


  }

  RejectPatient() {


    this._NoteService.getValueofSubscibtionNum().subscribe({
      next: data => {
        this.behaviorSubjectforSubscribtionsnumber = data
        console.log(data)
      },
      error: err => console.log(err)
    })

    this._NoteService.setValueforSubscibtionNum(--this.behaviorSubjectforSubscribtionsnumber);
    console.log(this.waitingPatient);
    this._DoctorService.rejectPatient(this.waitingPatient).subscribe((response) => {

      this.GetAllWaitingPatient();
    }),
      (error: any) => {

        this.GetAllWaitingPatient();
      }


  }

  showSpinner = false;
  timeoutId: any;
  makeApiRequest() {
    // Start the API request

    this.timeoutId = setTimeout(() => {
      this.showSpinner = false; // Set showSpinner to false after 2 seconds
    }, 17000);
  }
}