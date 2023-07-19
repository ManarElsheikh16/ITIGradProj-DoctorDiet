import { Component, OnInit } from '@angular/core';
import { IPass } from '../../user/interface/IPass';
import { FormBuilder, Validators } from '@angular/forms';
import { ConfirmPassVaildators } from '../CustomValidator/ConfirmPassword';
import { PasswordValidator } from '../CustomValidator/PassValidator';
import { Iforgetpass } from '../Interfaces/Iforgetpass';
import { ForgetService } from '../Services/forget.service';
import { distinctUntilChanged } from 'rxjs';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  Username!: string
  question!: string
  constructor(private formBuilder: FormBuilder, private forgetPass: ForgetService, private router: Router) {

  }
  ngOnInit(): void {


    this.forgetPass.getUserName().subscribe((resp) => {
      this.Username = resp;

    })
    this.forgetPass.getquestion(this.Username).subscribe((resp) => {
      this.question = resp.msg
      if (resp.msg == "لا يوجد حساب بهذا الاسم") {
        this.showWrongname()
      }
    })
  }

  showConfirmation = false;

  ObjPass: Iforgetpass = {
    userName: '',
    newPass: '',
    answer: ''
  }
  PassForm = this.formBuilder.group({

    password: ['', [Validators.required, PasswordValidator, Validators.minLength(8)]],
    answer: ['', [Validators.required]]
  });

  get newpassword() {
    return this.PassForm.get('password');
  }
  onSubmitPass() {
    if (this.PassForm.valid) {

      const newPasswordControl = this.PassForm.get('password');
      if (newPasswordControl && newPasswordControl.value) {

        this.ObjPass.newPass = newPasswordControl.value;

      }
      this.ObjPass.answer = this.PassForm.get('answer')?.value as string;
      this.forgetPass.getUserName().subscribe((resp) => {
        this.ObjPass.userName = resp;

      })
      this.forgetPass.forgetpass(this.ObjPass).subscribe((resp) => {
        console.log(resp)
        console.log("hello")
        if (resp.msg == 'Password Changed Successfully') {
          this.router.navigate(['/auth/Login'])
        } else {
          this.showCustomAlert()
        }
      })

      console.log(this.ObjPass)
    }
  }

  hideConfirmation() {
    this.showConfirmation = false;
  }

  showCustomAlert() {
    var alert = document.getElementById("wronganswer");
    alert!.style.display = "flex";

    setTimeout(function () {
      alert!.style.display = "none";
    }, 3000); // 3000 milliseconds = 3 seconds
  }

  showWrongname() {
    var alert = document.getElementById("wrongname");
    alert!.style.display = "flex";

    setTimeout(function () {
      alert!.style.display = "none";
    }, 3000); // 3000 milliseconds = 3 seconds
  }



}
