import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatChipInputEvent, MatChipsModule} from '@angular/material/chips';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {LiveAnnouncer} from '@angular/cdk/a11y';
import {ChangeDetectionStrategy, Component, Signal, inject, signal} from '@angular/core';
import { UserService } from '../user.service';
import { UserDTO } from '../models/UserDTO';
import { Router } from '@angular/router';
import { Blog } from '../models/Blog';
import { BlogService } from '../blog.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-create-blog',
  standalone: true,
  imports: [MatInputModule,MatFormFieldModule,FormsModule,ReactiveFormsModule,MatChipsModule,MatIconModule,MatButtonModule],
  templateUrl: './create-blog.component.html',
  styleUrl: './create-blog.component.css'
})
export class CreateBlogComponent {

  readonly keywords  = signal<string[]>([]);
  readonly formControl = new FormControl(['']);

  constructor(private userService:UserService, private blogService:BlogService,private _snackBar: MatSnackBar, private router:Router) {}

  ngOnInit() : void {
   this.userService.getCurrentUser().subscribe(user => {
    console.log("Post got user")
    this.User = user;
    if(!this.User) {
      console.log("didnt find user")
      this.router.navigate(['Login'])
    }
   })
   
  }

  ngOnDestroy() : void {
    
  }
  User : UserDTO | null = null


  PostForm = new FormGroup({
    Title : new FormControl("",Validators.required),
    Content : new FormControl("",Validators.required)
  })

  removeKeyword(keyword: string) {
    this.keywords.update(keywords => {
      const index = keywords.indexOf(keyword);
      if (index < 0) {
        return keywords;
      }

      keywords.splice(index, 1);
      return [...keywords];
    });
  }

  submitPost() {
    let blog = new Blog()
    blog.blog_Content = this.PostForm.get("Content")?.value!
    blog.short_Description = this.PostForm.get("Title")?.value!
    blog.categories = this.keywords().toLocaleString()
    blog.isApproved = this.User?.isModerator || false
    blog.timestamp = new Date
    blog.userId = this.User?.id!
    blog.word_Count = blog.blog_Content.split(' ').length

    this.blogService.postBlog(blog).subscribe(ret => {
      if(!ret) {
        this._snackBar.open("There seems to have been an error. Please try again later.", 'Close', {
          duration: 3000
        })
        return;
      }

      this._snackBar.open("Your blog has been posted successfully!", 'Close', {
        duration: 3000
      })

    })
  }





  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    // Add our keyword
    if (value) {
      this.keywords.update(keywords => [...keywords, value]);
    }

    // Clear the input value
    event.chipInput!.clear();
  }

}
