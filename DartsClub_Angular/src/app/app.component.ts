import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { UserService } from './user.service';
import {MatCardModule} from '@angular/material/card';

import {MatButtonModule} from '@angular/material/button';


import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { UserDTO } from './models/UserDTO';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,MatCardModule,MatButtonModule,MatFormFieldModule,MatInputModule,RouterLink,CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'DartsClub_Angular';

  constructor(private userService:UserService) {}

  
  User : UserDTO | null = null

  ngOnInit() : void {
    this.userService.getCurrentUser().subscribe(user => {
      console.log("app got user")
      this.User = user;
     })
  }

  printUser() {
    console.log(this.User)
  }
}
