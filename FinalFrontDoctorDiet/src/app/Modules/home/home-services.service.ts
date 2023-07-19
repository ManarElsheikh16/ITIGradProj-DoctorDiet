import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ShowDoctorsComponent } from './show-doctors/show-doctors.component';
import { HttpClient } from '@angular/common/http';
import { IDoctor } from '../shared/Interface/IDoctor';
import { Observable } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class HomeServicesService {
  dialogReferance: any = new BehaviorSubject("")
  dialogRef: any;
  constructor(public dialog: MatDialog, private http: HttpClient) { }
  ShowDoctors() {
    this.dialogRef = this.dialog.open(ShowDoctorsComponent, {
      width: ' 45rem',
      height: '31.25rem',
      panelClass: 'custom-dialog-container',

    });
    this.dialogReferance.next(this.dialogRef)
  }
  url: string = "http://localhost:5268"

  CloseDoctors() {
    this.dialogRef = this.dialog.closeAll()
  }
  getAllDoctors(): Observable<IDoctor[]> {
    return this.http.get<IDoctor[]>(`${this.url}/api/Doctor/GetAllDoctors`);
  }

}
