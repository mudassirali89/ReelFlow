import { HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth/authservice';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  
  // Skip authentication for auth endpoints (login/register)
  const isAuthEndpoint = req.url.includes('/api/auth');
  
  // Only add token for API calls that are not auth endpoints
  if (!isAuthEndpoint && req.url.includes('/api/')) {
    const token = authService.getToken();
    
    if (token) {
      // Clone the request and add the authorization header
      const authReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });
      
      return next(authReq);
    }
  }
  // For auth endpoints or requests without token, proceed with original request
  return next(req);
};
