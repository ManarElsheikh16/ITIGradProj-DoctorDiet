import { Component, ElementRef, Renderer2 } from '@angular/core';
import { IPlan } from '../Interface/IPlan';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';
import { DoctorService } from '../Service/doctor.service';
import { LoginService } from '../../auth/Services/login.service';

@Component({
  selector: 'app-update-plan',
  templateUrl: './update-plan.component.html',
  styleUrls: ['./update-plan.component.scss']
})
export class UpdatePlanComponent {
  myPlan: IPlan = {
    Duration: 0,
    CaloriesTo: 0,
    CaloriesFrom: 0,
    gender: "",
    goal: "",
    doctorId: this._loginSefvice.getUserId(),
  };
  maleId = document.getElementById("male") as HTMLInputElement
  femaleId = document.getElementById("male") as HTMLInputElement

  ngOnInit(): void {
    this._activeRout.params.subscribe(params => {
      this.myPlan.id = params["id"];
      this._doctorService.GetPlanID(this.myPlan.id).subscribe((resp) => {
        this.myPlan.Duration = resp.duration;
        this.myPlan.CaloriesFrom = resp.caloriesFrom;
        this.myPlan.CaloriesTo = resp.caloriesTo;
        this.myPlan.gender = resp.gender;
        this.myPlan.goal = resp.goal;
        if (this.myPlan.goal == "weightLoss") {

          this.selectOption('التخسيس')
        } else if (this.myPlan.goal == "weightGain") {
          this.selectOption('زيادة الوزن')

        } else {

          this.selectOption('تكوين العضلات')
        }
        console.log("asdasd", this.myPlan)
        console.log("asdasdasdasdd", this.myPlan)
        if (this.myPlan.gender === 'Male') {
          this.elementRef.nativeElement.querySelector('#male').checked = true;

        }
        else {

          console.log("hellofemale")
          this.elementRef.nativeElement.querySelector('#female').checked = true;

        }
      })
    })
  }

  constructor(private router: Router, private _activeRout: ActivatedRoute
    , private _doctorService: DoctorService, private _loginSefvice: LoginService, private renderer: Renderer2, private elementRef: ElementRef) { }

  changedInputs: string[] = [];


  updatePlaneWithProperties = {
    properties: this.changedInputs,
    Meal: this.myPlan
  }
  EditPlan!: FormGroup
  UpdatePlan() {
    this.EditPlan = new FormGroup({
      Id: new FormControl(this.updatePlaneWithProperties.Meal.id),
      CaloriesFrom: new FormControl(this.updatePlaneWithProperties.Meal.CaloriesFrom),
      CaloriesTo: new FormControl(this.updatePlaneWithProperties.Meal.CaloriesTo),
      Duration: new FormControl(this.updatePlaneWithProperties.Meal.Duration),
      gender: new FormControl(this.updatePlaneWithProperties.Meal.gender),
      goal: new FormControl(this.updatePlaneWithProperties.Meal.goal),
      properties: new FormControl(this.updatePlaneWithProperties.properties),
    });

    console.log("asdfsafasdf", this.updatePlaneWithProperties)
    const formData = new FormData();
    formData.append('Id', this.EditPlan.get('Id')?.value);
    formData.append('CaloriesFrom', this.EditPlan.get('CaloriesFrom')?.value);
    formData.append('CaloriesTo', this.EditPlan.get('CaloriesTo')?.value);
    formData.append('Duration', this.EditPlan.get('Duration')?.value);
    formData.append('gender', this.EditPlan.get('gender')?.value);
    formData.append('goal', this.EditPlan.get('goal')?.value);
    formData.append('properties', this.EditPlan.get('properties')?.value);
    this._doctorService.EditPlan(formData).subscribe((resp)=>{
      console.log(resp)
      this.router.navigate(['/doctor/dash/Plans'])

    })
    console.log(this.myPlan)

  }
  onInputChange(fieldName: string, value: any) {
    console.log("asd", fieldName)
    if (!this.changedInputs.includes(fieldName)) {
      this.changedInputs.push(fieldName);
    }
    if (fieldName == "gender") {
      if (value.value == 1) {
        this.myPlan.gender = 1
      }
      else {
        this.myPlan.gender = 0
      }
    }
  }

  options: string[] = ['تكوين العضلات', 'زيادة الوزن', 'التخسيس'];
  selectedOption: string | undefined;

  selectOption(option: string): void {
    this.selectedOption = option;
    if (!this.changedInputs.includes("goal")) {
      this.changedInputs.push("goal");
    }
    if (option == 'التخسيس') {
      this.myPlan.goal = 0;
      console.log("تخسيس")

    }
    if (option == 'زيادة الوزن') {
      console.log("وزن")
      this.myPlan.goal = 1;
    }
    if (option == 'تكوين العضلات') {
      console.log("عضلات")
      this.myPlan.goal = 2;
    }

  }
}
