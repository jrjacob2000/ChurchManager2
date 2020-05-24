import {
  Component,
  OnInit,
  AfterViewInit,
  Renderer2,
  ElementRef,
  ViewChild,
  Output,
  EventEmitter
} from '@angular/core';
import { AuthenticationService } from 'src/app/utils/services/authentication.service';
import { Router } from '@angular/router';

declare var $;

@Component({
  selector: 'app-menu-sidebar',
  templateUrl: './menu-sidebar.component.html',
  styleUrls: ['./menu-sidebar.component.scss']
})
export class MenuSidebarComponent implements OnInit, AfterViewInit {
  @ViewChild('mainSidebar', { static: false }) mainSidebar;    
  @Output() mainSidebarHeight: EventEmitter<any> = new EventEmitter<any>();

  public currentUser : string;

  constructor(public authService: AuthenticationService
    , private renderer: Renderer2
    , public router:Router ) {}

  
  ngOnInit() {
    this.currentUser = this.authService.currentUser;
  }

  ngAfterViewInit() {
    this.mainSidebarHeight.emit(this.mainSidebar.nativeElement.offsetHeight);
  }

  toggleNavSidebar(elem,selected)
  {    
    if(elem.classList.contains('menu-open'))
    {
      this.renderer.removeClass(elem, 'menu-open');
    }
    else
    {
      this.renderer.addClass(elem, 'menu-open');
    }
  }

  public routeContains(route:string)
  {
    return this.router.url.includes(route);
  }
}
