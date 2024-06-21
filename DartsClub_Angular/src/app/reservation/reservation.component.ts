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

@Component({
  selector: 'app-reservation',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [MatChipsModule, CommonModule,MatDatepickerModule,MatInputModule,MatFormFieldModule,MatButtonModule],
  templateUrl: './reservation.component.html',
  styleUrl: './reservation.component.css'
})
export class ReservationComponent {


  constructor(private userService:UserService, private reservationService:ReservationService) {}

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
    let crDTO = new createReservationsDTO
    crDTO.day = this.Day
    crDTO.userId = this.User!.id
    crDTO.Hours = this.selectedHours
    
    console.log(crDTO)

    this.reservationService.postReservation(crDTO)

  }

}
