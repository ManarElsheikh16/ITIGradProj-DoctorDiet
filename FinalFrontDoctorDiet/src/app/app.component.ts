import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'DoctorDiet';
  currentLang!:string;
  constructor(public translate:TranslateService){
    this.currentLang=localStorage.getItem('currentLang')|| 'en'
    this.translate.use(this.currentLang)
  }
  changecurrentLang(lang:string){
  this.translate.use(lang);
  localStorage.setItem('currentLang',lang)
  }
}
