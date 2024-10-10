import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass'] 
})
export class LoginComponent {
  public username: string = '';
  public password: string = '';
  public remember: boolean = false;

  constructor(private readonly router: Router) {}

  redirectToRegister() {
    this.router.navigate(['/register']);
  }

  Log() {
    console.log(this.username);
    console.log(this.password);
    console.log(this.remember);
  }

  onSubmit() {
    this.Log();
  }
}
