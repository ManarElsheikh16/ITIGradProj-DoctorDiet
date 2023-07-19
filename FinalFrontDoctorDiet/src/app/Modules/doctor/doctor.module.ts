import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KnowYourDocComponent } from './know-your-doc/know-your-doc.component';
import { FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { DoctorRoutingModule } from './doctor-routing.module';
import { SubscrebtionComponent } from './subscrebtion/subscrebtion.component';
import { PatientFollowUpComponent } from './patient-follow-up/patient-follow-up.component';
import { AddMealComponent } from './add-meal/add-meal.component';
import { AddPlanComponent } from './add-plan/add-plan.component';
import { MainComponent } from './main/main.component';
import { MealsComponent } from './meals/meals.component';
import { PlansComponent } from './plans/plans.component';
import { PlanDashComponent } from './plan-dash/plan-dash.component';
import { TodayMealsComponent } from './today-meals/today-meals.component';
import { PatientDetailsComponent } from './patient-details/patient-details.component';
import { ProfileComponent } from './profile/profile.component';
import { EditCustomPlanComponent } from './edit-custom-plan/edit-custom-plan.component';
import { CustomPlanDayComponent } from './custom-plan-day/custom-plan-day.component';
import { CustomPlanDayMealsComponent } from './custom-plan-day-meals/custom-plan-day-meals.component';
import { ChooseplanComponent } from './chooseplan/chooseplan.component';
import { UpdatePlanComponent } from './update-plan/update-plan.component';
import { WelcomeComponent } from './welcome/welcome.component';

@NgModule({
  declarations: [

    KnowYourDocComponent,
    SubscrebtionComponent,
    PatientFollowUpComponent,
    AddMealComponent,
    AddPlanComponent,
    MainComponent,
    MealsComponent,
    PlansComponent,
    PlanDashComponent,
    TodayMealsComponent,
    PatientDetailsComponent,
    ProfileComponent,
    EditCustomPlanComponent,
    CustomPlanDayComponent,
    WelcomeComponent,
    CustomPlanDayMealsComponent,
    ChooseplanComponent,
    UpdatePlanComponent,
  ],
  imports: [
    DoctorRoutingModule,
    CommonModule,
    RouterLink,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class DoctorModule { }
