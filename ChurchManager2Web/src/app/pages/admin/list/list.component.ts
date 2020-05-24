import { Component, OnInit, ViewChild } from '@angular/core';
//import { User } from 'src/app/utils/models/user';
import { UserService } from 'src/app/utils/services/user/user.service';
import { User } from 'src/app/utils/models/user';
import { ToastrService } from 'ngx-toastr';
import { MatTable } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import {DomSanitizer} from '@angular/platform-browser';
import {MatIconRegistry} from '@angular/material/icon';
import { UserModalComponent } from '../user-modal/user-modal.component';


@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

  public users : User[];
  displayedColumns: string[] = ['userName', 'email', 'action'];
  dataSource : User[];
  
  @ViewChild(MatTable,{static:true}) table: MatTable<any>;
  
  constructor(
       private userService : UserService
      , private toastr: ToastrService
      , public dialog: MatDialog) 
    { 
    }

  ngOnInit(): void {
    this.loadData();
  }


  loadData(){
    this.userService.getUsers().subscribe((data:any[]) =>
      {
        this.dataSource =  data.map(x =>  {
          return {
            Id : x.id,
            UserName : x.userName,
            Email : x.email
          }
        } )as unknown as User[];
      },
      (error) =>{
        console.log(error);
        if (error.status !== 401) {
          this.toastr.error(error.message, 'Error!');
        }
      });
  }

  openDialog(action,obj) {

    obj.action = action;
    const dialogRef = this.dialog.open(UserModalComponent, {
      width: '350px',
      data:obj
    });
 
    dialogRef.afterClosed().subscribe(result => {
        if(result.event == 'Delete'){
            this.loadData();
        }
    });
  }
 
  // addRowData(){
  //   var d = new User();
  //   d.UserName = "dfdsfdsf";
  //   d.Email = "dsfsdgsg";
  //   this.dataSource.push(d);
  //   this.table.renderRows();
    
  // }
  // updateRowData(row_obj){
  //   this.dataSource = this.dataSource.filter((value,key)=>{
  //     if(value.id == row_obj.id){
  //       value.name = row_obj.name;
  //     }
  //     return true;
  //   });
  // }
  // deleteRowData(row_obj){
  //   this.dataSource = this.dataSource.filter((value,key)=>{
  //     return value.id != row_obj.id;
  //   });
  // }

  

}
