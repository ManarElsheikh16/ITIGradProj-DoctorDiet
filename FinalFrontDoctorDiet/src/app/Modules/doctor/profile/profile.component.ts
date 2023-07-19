import { Component } from '@angular/core';
import { LoginService } from '../../auth/Services/login.service';
import { DoctorService } from '../Service/doctor.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ConfirmPassVaildators } from '../../auth/CustomValidator/ConfirmPassword';
import { PasswordValidator } from '../../auth/CustomValidator/PassValidator';
import { IPass } from '../../user/interface/IPass';
import { IDoctor } from '../Interface/IDoctor';
import { IContacInfo } from '../Interface/IContactInfo';


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent {
  IContacInfos: IContacInfo[] = []
  ContacInfo1: IContacInfo = {
    doctorId: "",
    contactInfo: "",
    id: 0
  }
  ContacInfo2: IContacInfo = {
    doctorId: "",
    contactInfo: "",
    id: 0
  }
  ContacInfo3: IContacInfo = {
    doctorId: "",
    contactInfo: "",
    id: 0
  }
  PhoneNumber !: any[]

  contactInfo !: any[]
  userId!: string
  errorMsg!: string
  data: any = {}
  ObjPass: IPass = {
    DoctorId: "",
    Password: ''
  }
  changedInputs: string[] = [];
  EditUser: IDoctor = {
    FullName: "",
    Email: "",
    PhoneNumber0: "",
    PhoneNumber1: "",
    PhoneNumber2: "",
    Specializtion: "",
    Location: "",
    Id: "",
    ProfileImage: ''
  }
  showConfirmation = false;
  UserInfo !: FormGroup
  constructor(private _LoginService: LoginService, private _DocService: DoctorService, private formBuilder: FormBuilder) {

  }
  ngOnInit() {
    this.userId = this._LoginService.getUserId()
    this.GetProfile()

    this.EditUser.Id = this.userId
    this.UserInfo = this.formBuilder.group({
      fullName: ['', [Validators.minLength(5), Validators.required]],
      email: ['', [Validators.email, Validators.required]],
      profileImage: [this.data.profileImage, Validators.required],
      phoneNumber0: ['', [Validators.pattern(/^(010[0-9]{8}|011[0-9]{8}|012[0-9]{8}|015[0-9]{8})$/), Validators.required]],
      phoneNumber1: ['', [Validators.pattern(/^(010[0-9]{8}|011[0-9]{8}|012[0-9]{8}|015[0-9]{8})$/)]],
      phoneNumber2: ['', [Validators.pattern(/^(010[0-9]{8}|011[0-9]{8}|012[0-9]{8}|015[0-9]{8})$/)]],
      Specializtion: ['', [Validators.required]],
      location: ['', [Validators.required]],

    });

  }

  PassForm = this.formBuilder.group({

    password: ['', [Validators.required, PasswordValidator]],
    confirmPassword: ['', [Validators.required]],

  },
    { validators: [ConfirmPassVaildators] });

  get fullName() {
    return this.UserInfo.get('fullName');
  }
  get email() {
    return this.UserInfo.get('email');
  }
  get phoneNumber0() {
    return this.UserInfo.get('phoneNumber0');
  }
  get phoneNumber1() {
    return this.UserInfo.get('phoneNumber1');
  }
  get profileImage() {
    return this.UserInfo.get('profileImage');
  }
  get phoneNumber2() {
    return this.UserInfo.get('phoneNumber2');
  }
  get Specializtion() {
    return this.UserInfo.get('Specializtion');
  }
  get location() {
    return this.UserInfo.get('location');
  }
  get confirmpassword() {
    return this.PassForm.get('confirmPassword');
  }
  get newpassword() {
    return this.PassForm.get('password');
  }

  GetProfile() {
    this._DocService.getSingleDoctor(this.userId).subscribe((resp) => {
      this.data = resp;
      this.PhoneNumber = resp.contactInfo
   
      if (this.data) {
        this.UserInfo.patchValue({
          fullName: this.data.fullName,
          email: this.data.email,
          location: this.data.location,
          phoneNumber0: this.data.contactInfo[0]?.contactInfo || '',
          phoneNumber1: this.data.contactInfo.length > 1 ? this.data.contactInfo[1].contactInfo : '',
          phoneNumber2: this.data.contactInfo.length > 2 ? this.data.contactInfo[2].contactInfo : '',
          Specializtion: this.data.specialization,
        });
      }


    }, error => {
      this.errorMsg = "you dont have access"
    })

  }

  onInputChange(fieldName: string, value: any) {

    if (!this.changedInputs.includes(fieldName)) {

      if (this.data.fullName != value && fieldName == 'FullName') {

        this.changedInputs.push(fieldName);
      }
      if (this.data.location != value && fieldName == 'Location') {

        this.changedInputs.push(fieldName);
      }
      if (this.data.specialization != value && fieldName == 'Specialization') {

        this.changedInputs.push(fieldName);
      }
      if (this.data.email != value && fieldName == 'Email') {

        this.changedInputs.push(fieldName);
      }
      if (fieldName == '0') {
       
        this.ContacInfo1 = {
          doctorId: this.userId,
          id: this.data.contactInfo[0].id,
          contactInfo: value
        };
      }


      if (fieldName == '1') {
        this.ContacInfo2 = {
          doctorId: this.userId,
          id: this.data.contactInfo[1].id,
          contactInfo: value
        };
      }
      if (fieldName == '2') {
        this.ContacInfo3 = {
          doctorId: this.userId,
          id: this.data.contactInfo[2].id,
          contactInfo: value
        };
      }
      if (fieldName == 'ProfileImage') {
        this.changedInputs.push(fieldName);
      }
    }
   
  }
 
  changeImg(event: any) {
    const reader = new FileReader();
    reader.readAsDataURL(event.target.files[0]);
    reader.onloadend = () => {
      const base64Data = reader.result?.toString().split(',')[1];
      this.EditUser.ProfileImage = base64Data;
    };

   
  }
  EditUserWithProperties: any = {
    properties: this.changedInputs,

  };
  UpdateUSerForm!: FormGroup;
  UpdateUSer() {
console.log(this.IContacInfos)
    if (this.ContacInfo1.contactInfo != '') {
      this.IContacInfos.push(this.ContacInfo1)
    }
    if (this.ContacInfo2.contactInfo != '') {
      this.IContacInfos.push(this.ContacInfo2)
    }
    if (this.ContacInfo3.contactInfo != '') {
      this.IContacInfos.push(this.ContacInfo3)
    }
    console.log(this.IContacInfos)

    this.EditUserWithProperties.phone = this.IContacInfos

    this.UpdateUSerForm = new FormGroup({
      properties: new FormControl(this.EditUserWithProperties.properties),
      phone: new FormControl(this.EditUserWithProperties.phone),
    });

    const formData = new FormData();
    formData.append('id', this._LoginService.getUserId());
    formData.append('contactInfo', this.UpdateUSerForm.get('phone')?.value);
    formData.append('fullName', this.UserInfo.get('fullName')?.value);
    formData.append('location', this.UserInfo.get('location')?.value);
    formData.append('specialization', this.UserInfo.get('Specializtion')?.value);
    formData.append('profileImage', this.EditUser.ProfileImage);
    formData.append('email', this.UserInfo.get('email')?.value);
    formData.append('properties', this.UpdateUSerForm.get('properties')?.value);

    const formGroup = new FormGroup({
      id: new FormControl(this._LoginService.getUserId()),
      contactInfo: new FormControl(this.UpdateUSerForm.get('phone')?.value),
      fullName: new FormControl(this.UserInfo.get('fullName')?.value),
      location: new FormControl(this.UserInfo.get('location')?.value),
      specialization: new FormControl(this.UserInfo.get('Specializtion')?.value),
      profileImage: new FormControl(this.EditUser.ProfileImage),
      email: new FormControl(this.UserInfo.get('email')?.value),
      properties: new FormControl(this.UpdateUSerForm.get('properties')?.value)
    });

 
    const jsonString = JSON.stringify(formGroup.value);

    this._DocService.EditProfile(jsonString).subscribe((res) => {


      if (res.msg == "done") {
        this.showConfirmation = true;
      }
    })
    this.IContacInfos = []
  }






  onSubmitPass() {
    if (this.PassForm.valid) {
      console.log("PassForm", this.PassForm.value)

      const newPasswordControl = this.PassForm.get('password');
      console.log(newPasswordControl)
      if (newPasswordControl && newPasswordControl.value) {

        this.ObjPass.Password = newPasswordControl.value;
        console.log("5ra",newPasswordControl.value)
      }

      this.ObjPass.DoctorId = this._LoginService.getUserId()

      this._DocService.ChangeDoctorPass(this.ObjPass).subscribe((resp) => {
        console.log(resp);
        if (resp.message == 'Success') {
          this.showConfirmation = true;
          this.PassForm.reset();
        }

      }, error => {
        console.log(error)
      })

    }


  }


  hideConfirmation() {
    this.showConfirmation = false;
  }




}
