import { Component } from '@angular/core';
import { UserService } from '../user.service';
import { UserDTO } from '../models/UserDTO';

import {MatChipsModule} from '@angular/material/chips';

import { CommonModule } from '@angular/common';
import {MatDatepickerInputEvent, MatDatepickerModule} from '@angular/material/datepicker';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {provideNativeDateAdapter} from '@angular/material/core';
import { ReservationService } from '../reservation.service';
import { createReservationsDTO } from '../models/createReservations';

import {MatButtonModule} from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ReservationDTO } from '../models/Reservation';

import {MatTableModule} from '@angular/material/table';

@Component({
  selector: 'app-reservation',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [MatTableModule,MatChipsModule,FormsModule,ReactiveFormsModule, CommonModule,MatDatepickerModule,MatInputModule,MatFormFieldModule,MatButtonModule],
  templateUrl: './reservation.component.html',
  styleUrl: './reservation.component.css'
})
export class ReservationComponent {


  constructor(private userService:UserService,private _snackBar:MatSnackBar, private router:Router, private reservationService:ReservationService) {}

  addEvent( event: MatDatepickerInputEvent<Date>) {
    if(!event.value) return
    if(event.value < new Date) {
      this._snackBar.open("The date you have picked is invalid", 'Close', {
        duration: 3000
      })
      this.Day = new Date
      return;
    }
    console.log(event.value)
    this.reservationService.getHours(event.value).subscribe(ret => {
        console.log(ret)
        this.Hours = [11,12,13,14,15,16,17,18,19,20].filter(num => !ret.includes(num))
        this.Day = event.value || new Date
        console.log(this.Hours)
      }
    )
  }

  
  PendingReservations : ReservationDTO[] = []
  
  displayedColumns: string[] = ['Date', 'Time'];
  
  Day : Date = new Date

  User : UserDTO | null = null

  ngOnInit() : void {
    this.userService.getCurrentUser().subscribe(user => {
      this.User = user;
      if(!this.User)
        
        this.router.navigate(['Login'])
     })


     this.reservationService.getHours(new Date).subscribe(ret => {
      console.log(ret)
      this.Hours = [11,12,13,14,15,16,17,18,19,20].filter(num => !ret.includes(num))
      this.Day =  new Date
      console.log(this.Hours)

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
    }
  )
  }

  Hours : number[] = [11,12,13,14,15,16,17,18,19,20]


  selectedHours : number[] = []

  chipSelected(hour : number) {
   
    if(this.selectedHours.includes(hour))
      this.selectedHours = this.selectedHours.filter(num => num !== hour)
    else 
      this.selectedHours.push(hour)

  }


  makeReservation() {

    if(!this.User) {
      this._snackBar.open("You need to log in first.", 'Close', {
        duration: 3000
      })

      return
    }

    let crDTO = new createReservationsDTO
    crDTO.day = new Date(this.Day.toDateString())
    crDTO.userId = this.User!.id
    crDTO.Hours = this.selectedHours
    
    console.log(crDTO)

    this.reservationService.postReservation(crDTO).subscribe(ret => {
      console.log(ret)
      if(ret)
        this._snackBar.open("Reservation Created Successfully!", 'Close', {
          duration: 3000
        })

        ret.forEach(elem => {this.PendingReservations.push(elem)})

        console.log(this.PendingReservations)


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

}
