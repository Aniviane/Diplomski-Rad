import { Component, inject, signal } from '@angular/core';
import {MatCardModule} from '@angular/material/card';

import {MatButtonModule} from '@angular/material/button';
import { UserService } from '../user.service';
import { UserDTO } from '../models/UserDTO';
import { ActivatedRoute, Router } from '@angular/router';
import { GameService } from '../game.service';
import { AveragesDTO } from '../models/AveragesDTO';
import {MatTableModule} from '@angular/material/table';

import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';
import { PictureDialogComponent } from '../picture-dialog/picture-dialog.component';
import { UpdatePictureDTO } from '../models/UpdatePictureDTO';
import { ChangePasswordComponent } from '../change-password/change-password.component';
import { ReservationDTO } from '../models/Reservation';
import { ReservationService } from '../reservation.service';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [MatCardModule,MatButtonModule, MatTableModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})



export class UserComponent {

  
  guestId  = "a0a0aa0d-a00a-0000-0a0a-00aa0a000a00"

  User : UserDTO | null = null
  picturePath : string = this.User?.picture.imagePath || '../../assets/default.png'
  UserAverages : AveragesDTO | null = new AveragesDTO
  PendingReservations : ReservationDTO[] = []

  constructor(private userService:UserService,private reservationService: ReservationService, private gameService:GameService, private router:Router) {}
  
  
  readonly picture = signal<File | null>(null);
  pictureFile : File | null = null
  readonly dialog = inject(MatDialog);




  openDialog(): void {
    const dialogRef = this.dialog.open(PictureDialogComponent, {
      data: {picture: this.picture()},
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      if (result !== undefined) {
        this.picture.set(result);
        this.changePicture()
      }
    });
  }

  openPassworDialog(): void {
    const dialogRef = this.dialog.open(ChangePasswordComponent)
  }


  changePicture() {
    if(!this.User) return
    let updatePicture : UpdatePictureDTO = new UpdatePictureDTO

    updatePicture.picture = this.picture()
    updatePicture.userId = this.User?.id
    if(!updatePicture.picture) {
      return
    }

    this.userService.updatePicture(updatePicture).subscribe(ret => {
      console.log(ret)
      if(ret)
        this.picturePath = ret.imagePath
      console.log(this.picturePath)
    })

  }

  ngOnInit() : void {
    this.userService.getCurrentUser().subscribe(user => {
      this.User = user;
      

      if(!this.User) {
        console.log("didnt find user")
        this.router.navigate([''])
        return
      }

      if(!this.User) return;
      this.reservationService.getMyReservations(this.User.id).subscribe(ret => {
        console.log(ret)
        if(!this.User) return;
        this.User.reservations = ret;

        if(this.User.reservations) {
          this.PendingReservations = this.User.reservations.filter(elem => 
               new Date(elem.day) > new Date
          )
          console.log("pending reservations : ", this.PendingReservations)
        }

      })
      
        if(!this.User.picture) 
          this.picturePath ='../../assets/default.png'
        else
          this.picturePath = this.User.picture.imagePath ||  '../../assets/default.png'
  
        console.log(this.picturePath)

      this.gameService.getAverages(this.User?.id).subscribe(ret => {
        this.UserAverages = ret
      })

     })
  }

  getDate(help: Date) : string {
    if(help)
      {
        let date = new Date(help)
      

        return date.toDateString()
      }
      return ""
  }
  
  displayedColumns: string[] = ['Date', 'Time'];
 

}



