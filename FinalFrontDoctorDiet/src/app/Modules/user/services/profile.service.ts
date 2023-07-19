import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IConnect } from '../../doctor/Interface/IConnect';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private _HttpClient: HttpClient) {
  }
  url: string = "http://localhost:5268"


  GetPatientInfo(PatientID: string): Observable<any> {

    return this._HttpClient.get(`${this.url}/api/Patient/patientDataDTO/${PatientID}`)
  }



  ChangePatientPass(userData: object): Observable<any> {

    return this._HttpClient.post(`${this.url}/api/Patient/ChangePassowrd`, userData)
  }

  ChangeAdminPass(userData: object): Observable<any> {

    return this._HttpClient.post(`${this.url}/api/Admin/ChangePassowrd`, userData)
  }
  getpatientSubscribtion(userId: string): Observable<any> {
    return this._HttpClient.get(`${this.url}/api/Patient/GetPatientHistory/${userId}`)
  }
  Cancel(userdata: IConnect): Observable<any> {
    return this._HttpClient.put(`${this.url}/api/Patient/CanceledSubscription`, userdata)
  }
  EditProfile(editUserForm: any): Observable<any> {

    return this._HttpClient.put(`${this.url}/api/Patient/EditPatientData`, editUserForm)
  }
}
