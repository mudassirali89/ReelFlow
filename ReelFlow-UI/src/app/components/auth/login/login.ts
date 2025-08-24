import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { AuthService } from '../../../services/auth/authservice';
import { Router, RouterModule } from '@angular/router';
import { LoginModel } from '../../../models/auth/loginModel';
@Component({
  selector: 'app-login',
  imports: [FormsModule,CommonModule,RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login 
{
  constructor(private authService: AuthService,private router: Router, private cdr: ChangeDetectorRef) { }
  loginModel:LoginModel = {
    UsernameOrEmail: '',
    Password: ''
  };
  isLoading: boolean = false;
  errorMessage: string = '';
  onLogin(loginForm: NgForm) {
    this.isLoading = true;      
    console.log(this.loginModel.UsernameOrEmail);
    console.log(JSON.stringify(this.loginModel));
    this.authService.login(this.loginModel)
    .subscribe({
         next:() =>{
         console.log('login successfull')
         this.isLoading = false;
         this.router.navigate(['movies'])
    },
    error:(err) =>{
      console.log(err)
      this.errorMessage = err.error ;
      this.isLoading = false;
      this.cdr.detectChanges();
    }
    });
  }
}
