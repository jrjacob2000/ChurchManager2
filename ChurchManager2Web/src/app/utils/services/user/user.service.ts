import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../../models/user';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {
    private url = "https://localhost:44325";
    
    constructor(private httpClient: HttpClient) { }

    getUsers() {
      return this.httpClient.get(this.url + '/api/ApplicationUser/users');    
    }

    updateRole(user: User) {
      return this.httpClient.post(this.url + '/api/ApplicationUser/UpdateUserRole', user)

    }

    getUser(id: string) {
      return this.httpClient.get<{token:  string}>(this.url + '/api/ApplicationUser/GetUser/'+id)

    }

    deleteUser(id: string) {
      return this.httpClient.delete(this.url + '/api/ApplicationUser/DeleteUser/'+id)
  
    }
}
