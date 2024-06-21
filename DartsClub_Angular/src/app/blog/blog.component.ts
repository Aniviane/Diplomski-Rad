import { Component } from '@angular/core';
import { UserService } from '../user.service';
import { UserDTO } from '../models/UserDTO';
import { Blog } from '../models/Blog';
import { BlogService } from '../blog.service';
import {MatButtonModule} from '@angular/material/button';
import { CommonModule } from '@angular/common';

import {MatCardModule} from '@angular/material/card';
import {MatDividerModule} from '@angular/material/divider';
import {MatChipsModule} from '@angular/material/chips';

@Component({
  selector: 'app-blog',
  standalone: true,
  imports: [MatButtonModule,CommonModule, MatCardModule,MatDividerModule,MatChipsModule],
  templateUrl: './blog.component.html',
  styleUrl: './blog.component.css'
})
export class BlogComponent {
  constructor(private userService:UserService, private blogService:BlogService) {}

  
  User : UserDTO | null = null
  UserApprovedBlogs : Blog[] = []
  UserNotApprovedBlogs : Blog[] = []

  Blogs : Blog[] = []

  ngOnInit() : void {
    this.userService.getCurrentUser().subscribe(user => {
      this.User = user;
     })
    this.blogService.getBlogs().subscribe(ret => {
      this.Blogs = ret
      console.log(this.Blogs)
      if(this.User) {
        this.UserApprovedBlogs = ret.filter(blog => blog.userId == this.User?.id && blog.isApproved)
        this.UserNotApprovedBlogs = ret.filter(blog => blog.userId == this.User?.id && !blog.isApproved)
      }

    })
  }
}
