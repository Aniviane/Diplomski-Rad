import { Component, Inject, afterNextRender, inject, model, signal } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';
import { Blog } from '../models/Blog';
import { BlogService } from '../blog.service';
import {MatChipInputEvent, MatChipsModule} from '@angular/material/chips';
import {MatIconModule} from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-update-blog-dialog',
  standalone: true,
  imports: [MatFormFieldModule,
    MatChipsModule,
    MatIconModule,
    ReactiveFormsModule,
    MatInputModule,
    FormsModule,
    CommonModule,
    MatButtonModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose],
  templateUrl: './update-blog-dialog.component.html',
  styleUrl: './update-blog-dialog.component.css'
})
export class UpdateBlogDialogComponent {

  readonly dialogRef = inject(MatDialogRef<UpdateBlogDialogComponent>);
  readonly keywords  = signal<string[]>(['baby']);
  readonly formControl = new FormControl(['']);

  PostForm = new FormGroup({
    Title : new FormControl(this.data?.short_Description || "",Validators.required),
    Content : new FormControl(this.data?.blog_Content || "",Validators.required)
  })


  constructor(private blogService:BlogService, private _snackBar: MatSnackBar, @Inject(MAT_DIALOG_DATA) public data : Blog, private fb : FormBuilder) {
    
  }

  ngOnInit() {
    console.log(this.data)
    console.log(this.data?.blog_Content)
    this.PostForm.controls.Content.setValue(this.data?.blog_Content || "") 
    this.PostForm.controls.Title.setValue(this.data?.short_Description || "") 
   
    if(this.data?.categories)
    this.keywords.set(this.data?.categories.trim().split(','))

    console.log(this.keywords())
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

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

  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    // Add our keyword
    if (value) {
      this.keywords.update(keywords => [...keywords, value]);
    }

    // Clear the input value
    event.chipInput!.clear();
  }

  successFlag : boolean = false

  onSubmit() {
    let blog = this.data
    if(!blog) return
    blog.blog_Content = this.PostForm.controls.Content.value || ""
    blog.short_Description = this.PostForm.controls.Title.value || ""
    blog.word_Count = blog.blog_Content.trim().split(' ').length
    blog.categories = this.keywords().toLocaleString()

    this.blogService.updateBlog(blog).subscribe(ret => {
      if(ret)
        this.successFlag = ret
      this._snackBar.open("Post has been updated", 'Close', {
        duration: 3000
      })
    })
  }
}
