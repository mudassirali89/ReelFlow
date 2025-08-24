import { Routes } from '@angular/router';
import { Dashboard } from '../dashboard/dashboard';
import { Login } from '../auth/login/login';
import { Register } from '../auth/register/register';
import { Movies } from '../movies/movies';
import { Category } from '../category/category';
import { authGuard } from '../../guards/authGuard';

export const routes: Routes = [
    {
        path: '',
        component: Login
    },
    {
        path: 'dashboard',
        component: Movies, canActivate: [authGuard]
    },
    {
        path: 'login',
        component: Login
    },
    {
        path: 'register',
        component: Register
    },
    {
        path: 'movies',
        component: Movies, canActivate: [authGuard]
    },
    {
        path: 'categories',
        component: Category, canActivate: [authGuard]
    },

];
