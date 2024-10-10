import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.sass'
})
export class RegisterComponent {
  
  public email: string = "";
  public remember: boolean = false;

  constructor(private readonly router: Router) {}

  redirectToLogin() {
    this.router.navigate(["/login"])
  }
}
