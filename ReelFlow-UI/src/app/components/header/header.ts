import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth/authservice';
@Component({
  selector: 'app-header',
  imports: [CommonModule,RouterLink],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header 
{
  constructor(private authService: AuthService) {}
  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }
  logout()
  {
    this.authService.logout();
  }
  getCurrentUser()
  {
    return this.authService.getCurrentUser();
  }
}
