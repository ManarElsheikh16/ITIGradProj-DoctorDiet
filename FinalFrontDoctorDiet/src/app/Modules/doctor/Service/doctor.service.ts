import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IDoctor } from '../../shared/Interface/IDoctor';
import { IConnect } from '../Interface/IConnect';
import { IPlan } from '../Interface/IPlan';
import { FormBuilder } from '@angular/forms';
import { choose } from '../Interface/IChooseplan';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  constructor(private _http: HttpClient, private _formBuilder: FormBuilder) { }


  url: string = "http://localhost:5268"

  getSingleDoctor(id: string): Observable<IDoctor> {
    return this._http.get<IDoctor>(`${this.url}/api/Doctor/doctorid?doctorid=${id}`);
  }

  Subscribe(Connect: IConnect): Observable<IConnect> {
    return this._http.post<IConnect>(`${this.url}/api/Patient/Subscribtion`, Connect);
  }

  addPlan(data: IPlan): Observable<IPlan> {
    return this._http.post<IPlan>(`${this.url}/api/Plan`, data);
  }


  ChangeDoctorPass(userData: object): Observable<any> {

    return this._http.post(`${this.url}/api/Doctor/ChangePassowrd`, userData)
  }
  getAllWaitingPatients(Doctorid: any): Observable<any> {
    return this._http.get(`${this.url}/api/Patient/GetPatientsByDoctorIdWithStatusWaiting?Doctorid=${Doctorid}`)
  }
  rejectPatient(waitingPatient: any): Observable<any> {
    return this._http.put(`${this.url}/api/Patient/RejectAccount`, waitingPatient);
  }
  acceptPatient(waitingPatient: any): Observable<any> {
    return this._http.put(`${this.url}/api/Patient/ConfirmAccount`, waitingPatient);
  }

  GetDoctorPlans(doctorid: string): Observable<any> {

    return this._http.get(`${this.url}/api/Plan/GetAllPlansByDoctotId?doctorID=${doctorid}`)
  }

  GetDaysByplanID(planID: number) {


    return this._http.get(`${this.url}/api/Plan/GetDaysByPlanId?planId=${planID}`)
  }
  GetMealByDayID(dayId: number) {


    return this._http.get(`${this.url}/api/Plan/GetMealsByDayId?dayId=${dayId}`)
  }
  EditCustomMeal(editMealForm: any) {

    return this._http.put(`${this.url}/api/CustomPlan/UpdateCustomMeal`, editMealForm)
  }
  GetMealById(id: number) {
    return this._http.get(`${this.url}/api/Plan/GetMealById${id}`)
  }
  GetCustomMealById(id: number) {
    return this._http.get(`${this.url}/api/CustomPlan/GetMealCustomPlanByCusDayId/${id}`)
  }

  EditMeal(editMealForm: any) {


    return this._http.put(`${this.url}/api/Plan/UpdateMeal`, editMealForm)
  }

  EditProfile(editUserForm: any): Observable<any> {

    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    const options = { headers: headers };
    return this._http.put(`${this.url}/api/Doctor/EditDoctorData`, editUserForm, options)
  }

  GetDoctorImg(userId: any): Observable<any> {

    return this._http.get(`${this.url}/api/Doctor/GetDoctorImg?doctorid=${userId}`)
  }
  Delete(PlanId: any): Observable<any> {

    return this._http.delete(`${this.url}/api/Plan/DeletePlan${PlanId}`)
  }
  Chooseplan(IChoose: choose): Observable<any> {

    return this._http.post(`${this.url}/api/Doctor/AddCustomPlanToSpecificPatient`, IChoose)
  }

  EditPlan(editPlan: any) {


    return this._http.put(`${this.url}/api/Plan/UpdatePlan`, editPlan)
  }

  GetPlanID(planID: any): Observable<any> {
    return this._http.get(`${this.url}/api/Plan/GetPlanById?id=${planID}`)
  }



}
