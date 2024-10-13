import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from './login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass'] 
})
export class LoginComponent implements OnInit {
  

  public _email: string = "";
  public _password: string = "";
  public _trust: boolean = false;
  public _conflict: string = "";
  public _blank: string = "";


  constructor(private readonly router: Router, private readonly loginService: LoginService) { }


  trustSwitch() {
    this._trust = !this._trust;
  } 


  ngOnInit(): void {

    document.cookie.split("email=")[1] === undefined ? null : this._email = document.cookie.split("email=")[1].split(";")[0];
    document.cookie.split("password=")[1] === undefined ? null : this._password = document.cookie.split("password=")[1].split(";")[0];
  }


  redirectToRegister = () => this.router.navigate(["/register"]);


  async onSubmit() {

    if(this._email === "" || this._password === "")
      return;

    const response = await this.loginService.loginAsync(this._email, this._password, this._trust);

    console.log(response);

    if(response.status !== 200) {
      this._conflict = response.conflict;
    } else {
      this._trust && document.cookie.split("DeviceID=")[1] === undefined ? document.cookie = `DeviceID=${response.deviceId}` : null;
      document.cookie = "token=" + response.token
      this.router.navigate(["/"]);
    } 
      

  }
}
