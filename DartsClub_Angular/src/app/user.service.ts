import { HttpClient } from '@angular/common/http';
import { Injectable, afterNextRender } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoginDTO } from './models/LoginDTO';
import { UserCreateDTO } from './models/UserCreateDTO';
import { UserDTO } from './models/UserDTO';
import { UsernamesDTO } from './models/UsernamesDTO';
import { PictureDTO } from './models/PictureDTO';
import { UpdatePictureDTO } from './models/UpdatePictureDTO';
import { ChanegPasswordDTO } from './models/ChangePasswordDTO';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  BaseUrl =  "https://localhost:44314/"

  private UserSubject = new BehaviorSubject<UserDTO | null>(null);

  constructor(private http:HttpClient) {
    afterNextRender(() => {

      const storedData = sessionStorage.getItem('userId');
      console.log(storedData)
      if (storedData) {
        try {

          if(storedData) {
            console.log('found id in storage' + storedData)
            this.getUserById(storedData).subscribe(ret => {
              this.setUser(ret)
            })
          }
        }
        catch (err) {
        }
      }
    });

  
   }

   logOut() {
    sessionStorage.removeItem('userId')
    this.setUser(null)
   }

   getUserById(id : string) : Observable<UserDTO>{
    return this.http.get<UserDTO>(this.BaseUrl + "api/Users/" + id)
  }

  changePassword(data : ChanegPasswordDTO) : Observable<boolean> {
    return this.http.put<boolean>(this.BaseUrl + "api/Users/Password", data)
  }


  getUsernames() : Observable<UsernamesDTO[]> {
    
    return this.http.get<UsernamesDTO[]>(this.BaseUrl + "api/Users/Usernames")
  }


  updatePicture(picture : UpdatePictureDTO) : Observable<PictureDTO> {
      let formData = new FormData
      
      formData.append("picture", picture.picture!)
      formData.append("userId", picture.userId)

      return this.http.put<PictureDTO>(this.BaseUrl + "api/Users/Picture", formData)
      
    
  }

  setUser(user : any): void {
    this.UserSubject.next(user)
    sessionStorage.setItem('userId', user.id)
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
