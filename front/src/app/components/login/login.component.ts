import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  username: string = '';
  password: string = '';
  errorMessage: string = '';
  

  // Hardcoded credentials
  private credentials: { [key: string]: { username: string; password: string } } = {
    IBC: { username: 'IBCadmin', password: 'IBCadmin' },
    Jet: { username: 'Jetadmin', password: 'Jetadmin' },
    Gis: { username: 'Gisadmin', password: 'Gisadmin' },
  };

  private colorSchemes: { [key: string]: { primary: string; secondary: string; accent: string; whites: string} } = {
    IBC: { primary: '#000000', secondary: '#FFFFFF', accent: '#FF0305', whites: '#ffffff' },
    Jet: { primary: '#000000', secondary: '#FFFFFF', accent: '#FC9802', whites: '#ffffff' },
    Gis: { primary: '#1e1e1e', secondary: '#adb4b7', accent: '#ffc641', whites: '#ffffff' },
  };

  constructor(private router: Router) {}

  ngOnInit(): void {
    const loggedIn = localStorage.getItem('isLoggedIn');
    if (loggedIn === 'true') {
      this.router.navigate(['/dashboard']);
    }
  }

  login(): void {
    const enteredUsername = this.username.trim().toLowerCase();
    const enteredPassword = this.password.trim().toLowerCase();

    // Check if the entered credentials match any of the available companies
    for (const adminType in this.credentials) {
      const user = this.credentials[adminType];
      const storedUsername = user.username.toLowerCase();
      const storedPassword = user.password.toLowerCase();

      if (enteredUsername === storedUsername && enteredPassword === storedPassword) {
        // Successful login
        localStorage.setItem('isLoggedIn', 'true');
        localStorage.setItem('adminType', adminType);
        localStorage.setItem('themeColors', JSON.stringify(this.colorSchemes[adminType]));

        // Apply theme colors
        const themeColors = this.colorSchemes[adminType];
        document.documentElement.style.setProperty('--primary-color', themeColors.primary);
        document.documentElement.style.setProperty('--secondary-color', themeColors.secondary);
        document.documentElement.style.setProperty('--accent-color', themeColors.accent)
        document.documentElement.style.setProperty('--whites-color', themeColors.whites);

        this.router.navigate(['/dashboard']);
        return;
      }
    }

    // If no match is found, display an error
    this.errorMessage = 'Invalid username or password. Please try again.';
  }
}
