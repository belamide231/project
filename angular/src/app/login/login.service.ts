import { Injectable } from '@angular/core';
import { api } from '../api';


@Injectable({
  providedIn: 'root'
})
export class LoginService {
  
  public endpoint: string = "/api/user/login";

  async loginAsync (email: string, password: string, trust: boolean) {

    try {

      return (await api.post(this.endpoint, {
        "email": email,
        "password": password,
        "trust": trust,
        "deviceId": document.cookie.split("DeviceID=")[1] === undefined ? "" : document.cookie.split("DeviceID=")[1].split(";")[0]
      })).data;

    } catch (error: any) {

      return error.response.data;
    }    
  }
}
