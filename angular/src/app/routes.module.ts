import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';


var route : Routes = [
  { path: "", component: HomeComponent, pathMatch: "full" },
  { path: "login", component: LoginComponent }
]


@NgModule({
  imports: [
    RouterModule.forRoot(route)
  ],
  exports: [
    RouterModule
  ]
})
export class RoutesModule { }
