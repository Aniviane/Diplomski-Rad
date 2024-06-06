import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  BaseUrl =  "https://localhost:44314/"
  constructor(private http:HttpClient) { }

  getUser() : Observable<any>{
    return this.http.get<any>(this.BaseUrl + "api/Users")
  }
}
