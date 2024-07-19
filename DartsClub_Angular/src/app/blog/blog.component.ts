import { Component, inject } from '@angular/core';
import { UserService } from '../user.service';
import { UserDTO } from '../models/UserDTO';
import { Blog } from '../models/Blog';
import { BlogService } from '../blog.service';
import {MatButtonModule} from '@angular/material/button';
import { CommonModule } from '@angular/common';
import {MatInput, MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatCardModule} from '@angular/material/card';
import {MatDividerModule} from '@angular/material/divider';
import {MatChipsModule} from '@angular/material/chips';
import { ApproveDTO } from '../models/ApproveDTO';
import {MatTabsModule} from '@angular/material/tabs';

import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';
import { UpdateBlogDialogComponent } from '../update-blog-dialog/update-blog-dialog.component';
import { SearchQueryDTO } from '../models/SearchQueryDTO';
import { log } from 'console';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-blog',
  standalone: true,
  imports: [MatButtonModule,MatTabsModule,CommonModule,FormsModule, ReactiveFormsModule, MatInputModule,MatFormFieldModule, MatCardModule,MatDividerModule,MatChipsModule,CommonModule],
  templateUrl: './blog.component.html',
  styleUrl: './blog.component.css'
})
export class BlogComponent {
  constructor(private userService:UserService, private blogService:BlogService) {}

  
  User : UserDTO | null = null
  UserApprovedBlogs : Blog[] = []
  UserNotApprovedBlogs : Blog[] = []
  NotApprovedBlogs : Blog[] = []
  Blogs : Blog[] = []
  SearchedBlogs : Blog[] = []

  ngOnInit() : void {
    this.userService.getCurrentUser().subscribe(user => {
      this.User = user;
      if(this.User) {

        if(this.User.isModerator) {
          this.blogService.getNotApprovedBlogs().subscribe(ret => {
            if(ret)
              this.NotApprovedBlogs = ret
          })
        }

        this.blogService.getMyBlogs(this.User.id).subscribe(ret => {
        this.UserApprovedBlogs = ret.filter(blog => blog.userId == this.User?.id && blog.isApproved)
        
      })
     }
    })

    this.blogService.getBlogs().subscribe(ret => {
      this.Blogs = ret
      this.SearchedBlogs = this.Blogs
      console.log(this.Blogs)
    })
  }


  approveBlog(blogId : string) {
    if(!this.User) return

    let request = new ApproveDTO
    request.blogId = blogId
    request.userId = this.User.id

    this.blogService.approveBlog(request).subscribe(ret => {
      if(ret)
        {
          const index = this.NotApprovedBlogs.findIndex(elem => elem.id == blogId)
          console.log(index, this.NotApprovedBlogs[index])
          this.NotApprovedBlogs.splice(index,1)
        }
    })
  }

  deleteBlog(blogId : string) {
    this.blogService.deleteBlog(blogId).subscribe(ret => {
      if(ret)
        {
          const index = this.NotApprovedBlogs.findIndex(elem => elem.id == blogId)
          console.log(index, this.NotApprovedBlogs[index])
          this.NotApprovedBlogs.splice(index,1)
        }
    })
  }

 
  contentField = ""
  titleField = ""

  onChange() {
    let data = new SearchQueryDTO
    data.content = this.contentField
    data.title = this.titleField

    if(data.content == "" && data.title == "") { 
      this.SearchedBlogs = this.Blogs  
      return
    }

    this.blogService.searchBlogs(data).subscribe(ret => {
      console.log(ret)
      this.SearchedBlogs = ret
    })


  }


  readonly dialog = inject(MatDialog);

  changeBlog(id : string) {
    let blog = this.UserApprovedBlogs.find(elem => elem.id == id)
    if(!blog) return;
    const dialogRef = this.dialog.open(UpdateBlogDialogComponent, {
      width : '500px',
      data :  blog
    })

  }
}
