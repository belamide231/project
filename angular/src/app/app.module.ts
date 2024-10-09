import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { RoutesModule } from './routes.module';
import { FormsModule } from '@angular/forms';
import { HomeModule } from './home/home.module';
import { LoginModule } from './login/login.module';


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    RoutesModule,
    FormsModule,
    LoginModule,
    HomeModule
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule { }
