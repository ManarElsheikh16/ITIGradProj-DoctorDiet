export interface IPatientNoteData {
  "id":number
  "patientId": string,
  "doctorId": string,
  "dayCustomPlanId":number,
  "date":Date,
  "text":string|null|undefined
  "seen" : boolean
}
