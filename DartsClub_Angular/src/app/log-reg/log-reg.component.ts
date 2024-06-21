import { Component } from '@angular/core';
import {MatSelectModule} from '@angular/material/select';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {MatButtonModule} from '@angular/material/button';
import { UserService } from '../user.service';
import {LoginDTO} from '../models/LoginDTO'
import { UserCreateDTO } from '../models/UserCreateDTO';
import { UserDTO } from '../models/UserDTO';

@Component({
  selector: 'app-log-reg',
  standalone: true,
  imports: [MatFormFieldModule,MatInputModule,MatSelectModule,MatButtonModule,FormsModule,ReactiveFormsModule],
  templateUrl: './log-reg.component.html',
  styleUrl: './log-reg.component.css'
})
export class LogRegComponent {

  constructor(private userService:UserService) {}

  ngOnInit() : void {
   this.userService.getCurrentUser().subscribe(user => {
    console.log("logreg got user")
    this.User = user;
   })
   
  }

  ngOnDestroy() : void {
    
  }
  User : UserDTO | null = null

  LoginForm = new FormGroup({
    Username : new FormControl("",Validators.required),
    Password : new FormControl("",Validators.required)
  })

  RegisterForm = new FormGroup({
    Username : new FormControl("",Validators.required),
    Password : new FormControl("",Validators.required),
    Email : new FormControl("", Validators.required)
  })

 
  submitLogin() :void {
    console.log(this.LoginForm.get("Username")?.value,this.LoginForm.get("Password")?.value)

    let loginData = new LoginDTO()
    loginData.password = this.LoginForm.get("Password")?.value!
    loginData.username = this.LoginForm.get("Username")?.value!

    console.log("login data:  " + loginData)

    this.userService.loginUser(loginData)

  }

  printUser() {
    console.log(this.User)
  }

  submitRegister() : void {
    console.log(this.RegisterForm.get("Username")?.value,this.RegisterForm.get("Password")?.value, this.RegisterForm.get("Email")?.value)

    let registerData = new UserCreateDTO()
    registerData.password = this.RegisterForm.get("Password")?.value!
    registerData.name = this.RegisterForm.get("Username")?.value!
    registerData.email = this.RegisterForm.get("Email")?.value!

    console.log("register data:  " + registerData)

    this.userService.registerUser(registerData)

  }
}