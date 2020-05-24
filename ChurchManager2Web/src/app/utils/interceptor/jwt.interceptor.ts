import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authenticationService: AuthenticationService
    , private router: Router
    , private toastr: ToastrService) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with jwt token if available
        let loggedIn = this.authenticationService.loggedIn;
        let token = this.authenticationService.token;
        if (loggedIn && token) {
            request = request.clone({
                setHeaders: { 
                    Authorization: `Bearer ${token}`
                }
            });
        }

        //return next.handle(request);
        return next.handle(request).pipe( tap(() => {},
            (err: any) => {
                if (err instanceof HttpErrorResponse) {
                    if (err.status == 401) {
                        this.router.navigate(['login']);
                    }
                    
                }
            }
        ));
    }
   
    
}
