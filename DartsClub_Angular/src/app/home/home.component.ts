import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSidenavModule } from '@angular/material/sidenav';
import { RouterLink, RouterOutlet } from '@angular/router';
import { UserService } from '../user.service';
import { UserDTO } from '../models/UserDTO';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MatSidenavModule,RouterOutlet,MatCardModule,MatButtonModule,MatFormFieldModule,MatInputModule,RouterLink,CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  constructor(private userService:UserService) {}

  User : UserDTO | null = null

  ngOnInit() : void {
    this.userService.getCurrentUser().subscribe(user => {
      console.log("app got user")
      this.User = user;
     })
  }

  logOut() {
    this.userService.logOut()
  }

}
