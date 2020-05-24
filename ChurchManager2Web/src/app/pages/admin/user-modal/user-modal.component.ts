import { Component, OnInit, Optional, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { User } from 'src/app/utils/models/user';
import { UserService } from 'src/app/utils/services/user/user.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-user-modal',
  templateUrl: './user-modal.component.html',
  styleUrls: ['./user-modal.component.scss']
})
export class UserModalComponent implements OnInit {
  public action:string;
  public local_data:any;
  public loading = false;

  constructor(
    public dialogRef: MatDialogRef<UserModalComponent>
    //@Optional() is used to prevent error if no data is passed
    , @Optional() @Inject(MAT_DIALOG_DATA) public data: User
    , private userService :  UserService
    , private toastr: ToastrService) {
          this.local_data = {...data};
          this.action = this.local_data.action;
    }

  ngOnInit(): void {
    this.loadData();
  }

  loadData()
  {
    this.loading = true;
    this.userService.getUser(this.data.Id).subscribe((data:any) =>
    {
      this.local_data = data;
    },
    (error) =>{
      if(error.status !== 401)
      {
        this.toastr.error(error.status + "Unable to retrieve data", 'Error');
      }
    },
    () => {
      this.loading = false;
    });
  }

  doAction(){
    if(this.action == 'Update')
    {
        this.userService.updateRole(this.local_data).subscribe((data:any) =>
        {        
          this.toastr.success("", 'Saving success');
          this.dialogRef.close({event:this.action,data:this.local_data});
        },
        (error) =>{
          if(error.status !== 401)
          {
            console.log(error);
            this.toastr.error(error.status + " something went wrong", 'Saving failed');
          }
          else
            this.dialogRef.close();
        });
    }
    else if(this.action == 'Delete')
    {
      this.userService.deleteUser(this.data.Id).subscribe((data:any) =>
      {        
        this.toastr.success("", 'Delete success');
        this.dialogRef.close({event:this.action,data:this.local_data});
      },
      (error) =>{
        if(error.status !== 401)
        {
          console.log(error);
          this.toastr.error(error.status + " something went wrong", 'Delete failed');
        }
        else
          this.dialogRef.close();
      });
    }
    
  }

  closeDialog(){
    this.dialogRef.close({event:'Cancel'});
  }

}
