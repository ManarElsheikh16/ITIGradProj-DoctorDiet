import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserModule } from './Modules/user/user.module';
import { DoctorModule } from './Modules/doctor/doctor.module';
import { SharedModule } from './Modules/shared/shared.module';
import { AuthModule } from './Modules/auth/auth.module';
import { RouterLink, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthRoutingModule } from './Modules/auth/auth-routing.module';
import { HomeModule } from './Modules/home/home.module';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient,HttpClientModule } from '@angular/common/http';
import { ButtonModule } from 'primeng/button';


@NgModule({
  declarations: [
    AppComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule,
    HomeModule,
    UserModule,
    DoctorModule,
    SharedModule,
    ReactiveFormsModule,
    AuthRoutingModule,
    AuthModule,
    RouterLink,
    HttpClientModule,
    BrowserAnimationsModule,
    ButtonModule,
    TranslateModule.forRoot(
      {
        defaultLanguage:'en',
        loader:{
          provide:TranslateLoader,
          useFactory:CreateTranslateLoader,
          deps:[HttpClient]
        }
      }
    )
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],

  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
export function CreateTranslateLoader(http:HttpClient){
  return new TranslateHttpLoader(http,'./assets/i18n/','.json')
}
