import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { UserLogin, User } from '../data-structure/user-model';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

    private url = "https://localhost:44325";

    constructor(private httpClient: HttpClient) { }
  
    login(userLogin: UserLogin) {
      return this.httpClient.post<{token:  string}>(this.url + '/api/ApplicationUser/Login', userLogin)
      .pipe(tap(res => 
      {
        localStorage.setItem('access_token', res.token);
      }))
    }
  
    register(user : User) {
      return this.httpClient.post<{token: string}>(this.url + '/api/ApplicationUser/register', user)
      .pipe(tap(res => 
        {
            this.login({Email: user.Email, PasswordHash: user.PasswordHash})
        }))
    }
  
    logout() {
      localStorage.removeItem('access_token');
    } 
  
    public get loggedIn(): boolean{
      return localStorage.getItem('access_token') !==  null;
    }

    public get token(): string{
        return localStorage.getItem('access_token');
      }
  }
  