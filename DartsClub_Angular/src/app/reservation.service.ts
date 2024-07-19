import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { createReservationsDTO } from './models/createReservations';
import { ReservationDTO } from './models/Reservation';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {

  BaseUrl =  "https://localhost:44314/"

  constructor(private http:HttpClient) { }

  getMyReservations(id : string) : Observable<ReservationDTO[]> {
    return this.http.get<any[]>(this.BaseUrl + "api/Reservations/MyReservations/" + id)

  }

  getHours(date: Date) : Observable<number[]> {
    let params = new HttpParams().set("Date",date.toDateString())
    return this.http.get<number[]>(this.BaseUrl + "api/Reservations/Hours", {params: params})
  }

  postReservation(dto : createReservationsDTO) : Observable<any[]> {
    return this.http.post<any[]>(this.BaseUrl + "api/Reservations/", dto)
  }
}
