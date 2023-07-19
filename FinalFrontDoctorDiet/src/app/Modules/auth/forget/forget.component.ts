import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ForgetService } from '../Services/forget.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forget',
  templateUrl: './forget.component.html',
  styleUrls: ['./forget.component.scss']
})
export class ForgetComponent {

  constructor(private formBuilder: FormBuilder, private forgetPass: ForgetService, private router:Router) {


  }
  UserForm = this.formBuilder.group({

    userName: ['', [Validators.required,]],


  });

  GoToUser(UserForm: any) {

    if(UserForm.valid){
    this.forgetPass.setUserName(UserForm.get("userName").value);

    this.router.navigate(['/auth/ChangePass'])
    }
  }
}

