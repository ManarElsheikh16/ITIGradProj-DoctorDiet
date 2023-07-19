export interface IDoctorNoteData {
  "id":number
  "patientId": string,
  "doctorId": string,
  "date":Date,
  "text":string|null|undefined
}
