import { Component } from '@angular/core';
import { SidebarComponent } from "../sidebar/sidebar.component";
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [ SidebarComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  ngOnInit(): void {
    const themeColors = localStorage.getItem('themeColors');
    if (themeColors) {
      const colors = JSON.parse(themeColors);
      document.documentElement.style.setProperty('--primary-color', colors.primary);
      document.documentElement.style.setProperty('--secondary-color', colors.secondary);
      document.documentElement.style.setProperty('--accent-color', colors.accent);
      document.documentElement.style.setProperty('--whites-color', colors.whites);
    }
  }
  
}
