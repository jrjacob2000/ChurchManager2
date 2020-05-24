import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { UserLogin, User } from '../models/user';
import * as jwt_decode from 'jwt-decode'

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private url = "https://localhost:44325";

  constructor(private httpClient: HttpClient) { }

  public login(userLogin: UserLogin) {
    return this.httpClient.post<{token:  string}>(this.url + '/api/ApplicationUser/Login', userLogin)
    .pipe(tap(res => 
    {
      localStorage.setItem('access_token', res.token);
    }))
  }

  public register(user : User) {
    return this.httpClient.post<{token: string}>(this.url + '/api/ApplicationUser/register', user)
    .pipe(tap(res => 
      {
          this.login({Email: user.Email, PasswordHash: user.PasswordHash})
      }))
  }

  public  logout() {
    localStorage.removeItem('access_token');
  } 

  public get loggedIn(): boolean{
    return localStorage.getItem('access_token') !==  null;
  }

  public get token(): string{
      return localStorage.getItem('access_token');
    }

  public get currentUser(){
    let decoded = jwt_decode(this.token); 
    return decoded['UserName'];
  }
}
