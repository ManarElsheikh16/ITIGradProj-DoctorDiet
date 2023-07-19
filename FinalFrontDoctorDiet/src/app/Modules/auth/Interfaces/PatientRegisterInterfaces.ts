export interface PatientRegister {
    FullName: string;
    UserName: string;
    email: string;
    PhoneNumber:number;
    gender: string;
    age:Date;
    chronicDisease:string;
    confirmPassword:string;
    height:number;
    password:string;
    weight:number;
    Goal:string[];
    NoEat:string[];
    activityRate:string[];
    ProfileImg:File

  }