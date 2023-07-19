import { FormControl } from "@angular/forms";

export function PasswordValidator(control: FormControl) {

      let hasDigit = /\d/.test(control.value);
     
      let hasLower = /[a-z]/.test(control.value);

      const valid = hasDigit  && hasLower  ;
      if (!valid)
      {
          return { strong: true };
      }
      return null;
  }

