import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoginDTO } from './models/LoginDTO';
import { UserCreateDTO } from './models/UserCreateDTO';
import { UserDTO } from './models/UserDTO';
import { UsernamesDTO } from './models/UsernamesDTO';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  BaseUrl =  "https://localhost:44314/"

  private UserSubject = new BehaviorSubject<UserDTO | null>(null);

  constructor(private http:HttpClient) { }

  getUsernames() : Observable<UsernamesDTO[]> {
    
    return this.http.get<UsernamesDTO[]>(this.BaseUrl + "api/Users/Usernames")
  }


  setUser(user : any): void {
    this.UserSubject.next(user)
  }

  getCurrentUser() {
    return this.UserSubject.asObservable()
  }

  registerUser(data : UserCreateDTO) : UserDTO {
    this.http.post<UserDTO>(this.BaseUrl + "api/Users/",data).subscribe(ret => {
      console.log("registered user -> ", ret)
      this.setUser(ret)
      return ret
    })
    console.log("returning bad user")
    return new UserDTO
  }


  getUser() : Observable<any>{
    return this.http.get<any>(this.BaseUrl + "api/Users")
  }

  loginUser(data : LoginDTO) : UserDTO {
    let help = this.http.post<UserDTO>(this.BaseUrl + "api/Users/Login",data).subscribe(ret => {
      console.log("user logged in -> ", ret )
      this.setUser(ret)
      return ret
  })
  console.log("returning bad user")
  return new UserDTO
    
  }
}
