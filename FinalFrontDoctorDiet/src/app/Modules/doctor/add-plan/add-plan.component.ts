import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { IMeal } from '../Interface/IMeal';
import { IPlan } from '../Interface/IPlan';
import bsCustomFileInput from 'bs-custom-file-input';
import { IDay } from '../Interface/IDay';
import { DoctorService } from '../Service/doctor.service';
import { LoginService } from '../../auth/Services/login.service';

@Component({
  selector: 'app-add-plan',
  templateUrl: './add-plan.component.html',
  styleUrls: ['./add-plan.component.scss']
})
export class AddPlanComponent implements OnInit {
  Meals: any[] = [];
  Days: IDay[] = [];
  Day !: IDay
  NotAdded :boolean=false;
  NoDayAdded :boolean=false;
  NotMealAdded :boolean=false;
  AddDayDone :boolean=false;
  myMealImage: any ;
  myPlan: IPlan = {
    Duration: 0,
    CaloriesTo: 0,
    CaloriesFrom: 0,
    gender:0,
    goal:0,
    Days: [],
    Allergics: [],
    doctorId: this._loginSefvice.getUserId(),
  };
  ngOnInit(): void {
    bsCustomFileInput.init();

  }
  constructor(private router: Router, private formBuilder: FormBuilder
    , private _doctorService: DoctorService, private _loginSefvice: LoginService) { }

  PlanForm = this.formBuilder.group({
    duration: ['', Validators.required],
    CaloriesTo: ['', Validators.required],
    CaloriesFrom: ['', [Validators.required]],
    gender: ['', [Validators.required]],
    goal: ['', [Validators.required]],

  });

  MealForm = this.formBuilder.group({
    categoryID: ['categoryID', [Validators.required]],
    description: ['', [Validators.required]],
    TypeMeal: ['TypeMeal', [Validators.required]],
    imgMeal: ['', [Validators.required]]
  });

  get imgMeal() {
    return this.PlanForm.get('imgMeal');
  }
  get duration() {
    return this.PlanForm.get('duration');
  }
  get categoryID() {
    return this.PlanForm.get('categoryID');
  }
  get CaloriesTO() {
    return this.PlanForm.get('CaloriesTo');
  }
  get CaloriesFrom() {
    return this.PlanForm.get('CaloriesFrom');
  }
  get description() {
    return this.PlanForm.get('description');
  }
  get TypeMeal() {
    return this.PlanForm.get('TypeMeal');
  }

  ChangeCatigory() {

    var catogry = document.getElementById("catogry") as HTMLSelectElement;
    const Selectoption = catogry.options[catogry.selectedIndex].text;

    var lable1 = document.getElementById("lable1") as HTMLLabelElement;
    lable1.textContent = Selectoption;


  }
  AddMealToText() {
    const catogry = document.getElementById("Meals") as HTMLSelectElement;
    const selectOption = catogry.options[catogry.selectedIndex].text;
    const meals = document.getElementById("AllMeals") as HTMLSelectElement;
    const selectMeal = meals.options[meals.selectedIndex].text;

    const input1 = this.MealForm.get('description');

    if (selectOption == "الاساسية") {
      input1?.setValue(input1.value + selectMeal + "+");
    }
    else {
      input1?.setValue(input1.value + selectMeal + '+');
    }

  }

  addPlan(PlanForm: any) {

    if(this.Days.length>=1){
      this.showOverlay()
      this.NotAdded=false
    this.myPlan.doctorId =   this._loginSefvice.getUserId()
    this.myPlan.Duration = Number(this.PlanForm.get('duration')?.value);
    this.myPlan.CaloriesFrom = Number(this.PlanForm.get('CaloriesFrom')?.value);
    this.myPlan.CaloriesTo = Number(this.PlanForm.get('CaloriesTo')?.value);
    this.myPlan.gender = Number(this.PlanForm.get('gender')?.value);
    this.myPlan.Days = this.Days;
    this._doctorService.addPlan(this.myPlan).subscribe({
      next: data => {
        this.showOverly = false;
      this.router.navigate(['doctor/dash/Plans'])
    }
      ,
      error: err => console.log(err)
    })

  }
  else{
    console.log("nNotenetered")

      this.NotAdded=true
  }

  }

  hideConfirmation() {
    this.NotAdded = false;
  }
  hideConfirmationMeal() {
    this.NotMealAdded = false;
  }
  hideConfirmationDay() {
    this.NoDayAdded = false;
  }
  hideAddDayDone() {
    this.AddDayDone = false;
  }
  AddMeal(MealForm: any) {


    let myMeal: IMeal = {
      Description: '',
      image: undefined,
      category: -1,
      type: -1
    };
    const prevew = document.getElementById('preview');
    if (prevew) {
      prevew.style.display = 'none';
    }
    myMeal.category = Number(this.MealForm.get('categoryID')?.value);
    myMeal.Description = this.MealForm.get('description')?.value;
    myMeal.type = Number(this.MealForm.get('TypeMeal')?.value);
    let img = this.myMealImage
    myMeal.image = img;
    if(myMeal.Description=='' || myMeal.image==undefined || myMeal.category==-1 || myMeal.type == -1){
      this.NotMealAdded=true
    }
    else{

      this.NotMealAdded=false
    this.Meals.push(myMeal)
    MealForm.get('categoryID').reset({ value: 'category', disabled: false });
    MealForm.get('TypeMeal').reset({ value: 'JLC', disabled: false });
    MealForm.get('description').reset('');
    MealForm.get('imgMeal').reset();
    }
  }

  previewImage(event: any) {
    const input = event.target;
    if (input.files && input.files[0]) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const preview = document.getElementById('preview');
        if (preview) {
          preview.setAttribute('src', e.target.result);
          preview.style.display = 'inline';
        }
      };
      reader.readAsDataURL(input.files[0]);
    }
    this.imageuplud(event);

  }
  addDayList() {
   if(this.Meals.length>=1){
    this.NoDayAdded=false

      const newDay: IDay = {
      Meals: this.Meals,
    };

    this.Days.push(newDay)
    this.AddDayDone=true
    this.Meals=[]
  }
    else{
      this.NoDayAdded=true
    }

  }
  imageuplud(event: any) {
    const reader = new FileReader();
    reader.onloadend = () => {
      const base64Data = reader.result?.toString().split(',')[1]; 
     this.myMealImage = base64Data;
    };
    reader.readAsDataURL(event.target.files[0]);
  }

  MealLists = ["الاساسية", " البديل"];
  SubMeals = [ "لحم", "لبن", "فاكهة", "بيض", "أرز", 
  "مكرونه", "زبادي", "سمك", "سلطه", "رايب", "كرواسون", "فراخ مشويه", 
  "محشي", "لسان عصفور", "عصير طبيعي",
 , "فينو", "بيض", "ارز", "زبادى", "بسكوت", "لحوم مشوية", "فراخ مسلوقة", 
 "لحوم مشوية", "سلطه خضروات", "أسماك", "جبن", "بقسماط", "رغيف عيش", "شوكولاته", 
 "فراولة بالقشطه", "فاصوليا خضراء", "جبن رومى",

" طعمية", "فول", "بطاطس مهروسة", "بانية مشوى", "بتنجان مقلى", "كوسة", "ثمار فاكهه", "عصير مانجو"
]


  //Style\\ التصنيف
  options: string[] = ['تكوين العضلات', 'زيادة الوزن', 'التخسيس'];
  selectedOption: string | undefined;

  selectOption(option: string): void {
    this.selectedOption = option;
    if(option=='التخسيس'){
    this.myPlan.goal = 0;}
    if(option=='زيادة الوزن'){
    this.myPlan.goal = 1;}
    if(option=='تكوين العضلات'){
    this.myPlan.goal = 2;}

  }

  showOverly: boolean = false;

  showOverlay() {
    this.showOverly = true;
 
  }
}
