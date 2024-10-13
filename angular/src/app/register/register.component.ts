import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RegisterService } from './register.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.sass']
})
export class RegisterComponent implements OnInit {
  
  public _email: string = "";
  public _password: string = "";
  public _message : string = "";
  public _code = "";
  public _phase = 1;
  public _shown: boolean = false;
  public _remember: boolean = false;
  public _loading: boolean = false;

  constructor(private readonly _router: Router, private readonly _service: RegisterService) {}

  ngOnInit() {
    this._service._updateLoading.subscribe(value => this._loading = value);
  }
  redirectToLogin() {
    this._router.navigate(["/login"]);
  }
  rememberSwitch() {
    this._remember = !this._remember;
  }
  shownSwitch() {
    this._shown = !this._shown; 
  }

  clear() {
    this._phase = 1
    this._email = "";
    this._password = "";
    this._message = "";
    this._code = "";
    this._shown = false;
    this._remember = false;
    this._loading = false;
  }

  async ngSubmit() {

    this._message = "";

    if(this._phase === 1) {

      const response = await this._service.verifyAsync(this._email);
      this._message = response.message;
      console.log(response);
      response.status === 200 ? this._phase = 2 : this._message = response.conflict;

    } else if(this._phase === 2) {

      const nonAlphanumericChars = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
      const smallCase = "abcdefghijklmnopqrstuvwxyz";
      const capitalCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      const numeric = "0123456789";

      if(![...nonAlphanumericChars].some(char => this._password.includes(char))) 
        this._message = "Password requires non-alphanumeric.";
      else if(![...smallCase].some(char => this._password.includes(char)))
        this._message = "Password requires small letter case.";
      else if(![...capitalCase].some(char => this._password.includes(char)))
        this._message = "Password requires capital letter case.";
      else if(![...numeric].some(char => this._password.includes(char)))
        this._message = "Password requires numeric letter case.";
      else
        this._phase = 3;

    } else if(this._phase === 3) {

      const response = await this._service.registerAsync(this._email, this._password, this._code);
      if(response.status !== 200) {

        this._message = response.conflict
      } else {

        document.cookie = `DeviceID=${response.deviceId}; path=/login`;

        if(this._remember) {
          document.cookie = `email=${this._email}; path=/login`;
          document.cookie = `password=${this._password}; path=/login`;
          document.cookie = "remember=true; path=/login";
        }

        this._phase = 4;
      }
    }
  }
}
