import { ChangeDetectorRef, Component } from '@angular/core';
import { RouterLink, Router, RouterModule } from '@angular/router';
import { RegisterModel } from '../../../models/auth/registerModel.js';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth/authservice.js';

@Component({
  selector: 'app-register',
  imports: [FormsModule,CommonModule,RouterModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  registerModel: RegisterModel = {      
    UserName: '',
    Email: '',
    Password: '',
    ConfirmPassword: ''
  };

  errorMessage: string = '';
  isLoading: boolean = false;

  passwordsMatch(): boolean {
     return this.registerModel.Password === this.registerModel.ConfirmPassword;
  }

  constructor(private authService: AuthService, private router: Router,private cdr: ChangeDetectorRef) {
    
  }

  onSignup(form: NgForm) {
    if (form.invalid || !this.passwordsMatch()) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.register(this.registerModel).subscribe({
      next: (response: any) => {

        this.isLoading = false;
  console.log('Registration successful', response);
        this.router.navigate(['login']).then(
          () => {},
          (error) => console.error('Navigation failed:', error)
        );
      },
      error: (error: { status: number; }) => {
        console.error('Registration error:', error);
        this.isLoading = false;
        
        if (error.status === 400) {
          this.errorMessage = 'Invalid registration data. Please check your inputs.';
        } else if (error.status === 409) {
          this.errorMessage = 'User already exists with this email or username.';
        } else if (error.status === 500) {
          this.errorMessage = 'Server error. Please try again later.';
        } else {
          this.errorMessage = 'Registration failed. Please try again.';
        }
        this.cdr.detectChanges();
      }
    });
  }
}
