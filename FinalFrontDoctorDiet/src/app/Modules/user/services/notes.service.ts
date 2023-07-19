import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { INoteGetPatientData } from '../interface/INoteGetPatientData';
import { INoteGetDoctorData } from '../interface/INoteGetDoctorData';
import { IDoctorNoteDto } from '../interface/IDoctorNoteDto';
import { IPatientNoteData } from '../interface/IPatientNoteData';
import { IDoctorNoteData } from '../interface/IDoctorNoteData';

@Injectable({
  providedIn: 'root'
})
export class NotesService {

  constructor(private httpClient: HttpClient, private router: Router) {

  }
  private behaviorSubjectforNotificationsnumber = new BehaviorSubject<any>(0);
  private behaviorSubjectforCountNotifications = new BehaviorSubject<any>(0);
  private behaviorSubjectforSubscibtionsnumber = new BehaviorSubject<any>(0);

  private behaviorSubjectforHtmlDivs = new BehaviorSubject<HTMLElement[]>([]);

  // Methods to set and get the value of the behavior subject

  setValueforNotificationNum(value: any) {
    this.behaviorSubjectforNotificationsnumber.next(value);
  }

  getValueofNotificationNum(): Observable<any> {
    return this.behaviorSubjectforNotificationsnumber.asObservable();
  }
  setValueforNotificationCounter(value: any) {
    this.behaviorSubjectforCountNotifications.next(value);
  }

  getValueofNotificationCounter(): Observable<any> {
    return this.behaviorSubjectforCountNotifications.asObservable();
  }


  setValueforSubscibtionNum(value: any) {
    this.behaviorSubjectforSubscibtionsnumber.next(value);
  }

  getValueofSubscibtionNum(): Observable<any> {
    return this.behaviorSubjectforSubscibtionsnumber.asObservable();
  }


  setValueforHtmlDivs(value: HTMLElement[]) {
    const currentValue = this.behaviorSubjectforHtmlDivs.getValue();
    const updatedValue = [...currentValue, ...value];
    this.behaviorSubjectforHtmlDivs.next(updatedValue);
  }

  getValueofHtmlDivs(): Observable<HTMLElement[]> {
    return this.behaviorSubjectforHtmlDivs.asObservable();
  }

  url: string = "http://localhost:5268"


  AddPatientNote(PatientNoteData: IPatientNoteData): Observable<any> {

    return this.httpClient.post<IPatientNoteData>(`${this.url}/api/Patient/AddPatientNote`, PatientNoteData);
  }
  UpdatePatientNoteStatus(PatientNoteId: number): Observable<any> {

    return this.httpClient.get<any>(`${this.url}/api/Patient/UpdatePatientNoteStatus/${PatientNoteId}`);
  }
  AddDoctorNote(DoctorNoteData: IDoctorNoteData): Observable<any> {
    return this.httpClient.post<IDoctorNoteData>(`${this.url}/api/Doctor/AddDoctorNote`, DoctorNoteData);
  }

  GetPatientsNotesByDoctorId(doctorId: string): Observable<INoteGetPatientData[]> {

    return this.httpClient.get<INoteGetPatientData[]>(`${this.url}/api/Patient/GetPatientsNotesByDodId/${doctorId}`);
  }

  GetDoctorNotes(doctorNoteDto: IDoctorNoteDto): Observable<INoteGetDoctorData[]> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'

    });

    return this.httpClient.post<INoteGetDoctorData[]>(`${this.url}/api/Doctor/GetDoctorNotes`, doctorNoteDto, { headers });
  }
}
