import { Component, inject, model, signal } from '@angular/core';
import { Blog } from '../models/Blog';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ChanegPasswordDTO } from '../models/ChangePasswordDTO';
import { UserService } from '../user.service';
import { UserDTO } from '../models/UserDTO';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {
  MatDialog,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [MatFormFieldModule,
    MatInputModule,
    FormsModule,
    CommonModule,
    MatButtonModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    ReactiveFormsModule,
    MatDialogClose],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent {
  readonly dialogRef = inject(MatDialogRef<ChangePasswordComponent>);
  

  constructor(private userService:UserService) {}

  User : UserDTO | null = null
  changed : boolean = false

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.User = user;
    })
  }

 PasswordForm = new FormGroup({
    OldPassword : new FormControl("",Validators.required),
    NewPassword : new FormControl("",Validators.required)
  })

  
  submit() {
    let data = new ChanegPasswordDTO
    if(!this.User) return
    data.id = this.User?.id
    data.oldPassword = this.PasswordForm.get('OldPassword')?.value || ""
    data.newPassword = this.PasswordForm.get('NewPassword')?.value || ""

    this.userService.changePassword(data).subscribe(ret => {
      if(ret)
      this.changed = ret
    })
  }

  
  onNoClick(): void {
    this.dialogRef.close();
  }
}
