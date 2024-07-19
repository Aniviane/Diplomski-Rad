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

@Component({
  selector: 'app-reservation',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [MatChipsModule,FormsModule,ReactiveFormsModule, CommonModule,MatDatepickerModule,MatInputModule,MatFormFieldModule,MatButtonModule],
  templateUrl: './reservation.component.html',
  styleUrl: './reservation.component.css'
})
export class ReservationComponent {


  constructor(private userService:UserService,private _snackBar:MatSnackBar, private reservationService:ReservationService) {}

  addEvent( event: MatDatepickerInputEvent<Date>) {
    if(!event.value) return
    console.log(event.value)
    this.reservationService.getHours(event.value).subscribe(ret => {
        console.log(ret)
        this.Hours = [11,12,13,14,15,16,17,18,19,20].filter(num => !ret.includes(num))
        this.Day = event.value || new Date
        console.log(this.Hours)
      }
    )
  }
  
  Day : Date = new Date

  User : UserDTO | null = null

  ngOnInit() : void {
    this.userService.getCurrentUser().subscribe(user => {
      this.User = user;
     })

     this.reservationService.getHours(new Date).subscribe(ret => {
      console.log(ret)
      this.Hours = [11,12,13,14,15,16,17,18,19,20].filter(num => !ret.includes(num))
      this.Day =  new Date
      console.log(this.Hours)
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
    })

  }

}
