import { Component } from '@angular/core';
import {MatCardModule} from '@angular/material/card';

import {MatButtonModule} from '@angular/material/button';
import { UserService } from '../user.service';
import { UserDTO } from '../models/UserDTO';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [MatCardModule,MatButtonModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})



export class UserComponent {

  
  User : UserDTO | null = null

  constructor(private userService:UserService, private router:Router) {}
  
  ngOnInit() : void {
    this.userService.getCurrentUser().subscribe(user => {
      this.User = user;
      if(!this.User) {
        console.log("didnt find user")
        this.router.navigate([''])
      }
     })
  }


}
