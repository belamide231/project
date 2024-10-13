import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { api } from '../api';


@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  private _verify: string = "api/user/verify";
  private _register: string = "api/user/register";  

  private _setLoading = new BehaviorSubject<boolean>(false);
  _updateLoading = this._setLoading.asObservable();


  public async verifyAsync(email: string) {

    this._setLoading.next(true);

    try {
      return (await api.post(this._verify, {
        "email": email
      })).data;
    } catch (error: any) {
      return error.response.data;
    } finally {
      this._setLoading.next(false);
    } 
  }

  public async registerAsync(email: string, password: string, code: string) {
    this._setLoading.next(true);
    const deviceId: string = document.cookie.split('; ').find(row => row.startsWith('DeviceID='))?.split('=')[1] || "";
    try {
      return (await api.post(this._register, {
        "verificationCode": code,
        "email": email,
        "password": password,
        "deviceId": deviceId
      })).data;
    } catch (error: any) {
      return error.response.data;
    } finally {
      this._setLoading.next(false);
    }
  }
}
