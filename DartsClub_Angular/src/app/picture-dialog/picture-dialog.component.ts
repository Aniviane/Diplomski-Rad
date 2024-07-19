import { Component, inject, model } from '@angular/core';
import {FormsModule} from '@angular/forms';
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


@Component({
  selector: 'app-picture-dialog',
  standalone: true,
  imports: [MatFormFieldModule,
  MatInputModule,
  FormsModule,
  MatButtonModule,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose],
  templateUrl: './picture-dialog.component.html',
  styleUrl: './picture-dialog.component.css'
})
export class PictureDialogComponent {
  readonly dialogRef = inject(MatDialogRef<PictureDialogComponent>);
  readonly data = inject<File | null>(MAT_DIALOG_DATA);
  readonly picture = model(this.data);

  onChange(event : any) {
    this.picture.set(event.target.files[0]) 
    console.log(this.picture())
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
