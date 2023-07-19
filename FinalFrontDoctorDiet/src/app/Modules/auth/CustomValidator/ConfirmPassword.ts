import { AbstractControl } from "@angular/forms";
export function ConfirmPassVali(control: AbstractControl) {
   const pass = control.get('password')
   const Confpass = control.get('confirmPassword')


   if (pass?.pristine || Confpass?.pristine) {
      return null
   }
   else {
      
      return pass && Confpass && pass.value != Confpass.value
         ? { 'MisMatch': true }
         : null;
   }

     
}
export function ConfirmPassVaildators(control:AbstractControl)
{
  const password=control.get('password');
  const ConfirmPass=control.get('confirmPassword');

  return password && ConfirmPass && password.value!=ConfirmPass.value
  ? {'misMatch':{value:true}}
  :null
}

