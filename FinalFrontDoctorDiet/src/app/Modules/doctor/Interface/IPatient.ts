export interface IPatient{
    id:string,
    fullName:string,
    gender:string ,
    weight:number,
   height:number ,
   subscribed:string,
   birthDate :Date,
   diseases:string ,
   maxCalories:number ,
   minCalories:number, 
   profileImage :any,
   phoneNumber:string ,
   email:string ,

   noEat:any[],
   goal:any[] ,

   activityRates:any[]
}