import {Component, OnInit} from '@angular/core';
import { AuthenticationService } from '../../auth/services/authentication.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  constructor(private authService: AuthenticationService) {
  }

  ngOnInit() {
  }

  logOut(){
    this.authService.logout();
  }
}
