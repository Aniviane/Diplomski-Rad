import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, afterNextRender } from '@angular/core';
import { BehaviorSubject, catchError, Observable, throwError } from 'rxjs';
import { LoginDTO } from './models/LoginDTO';
import { UserCreateDTO } from './models/UserCreateDTO';
import { UserDTO } from './models/UserDTO';
import { UsernamesDTO } from './models/UsernamesDTO';
import { PictureDTO } from './models/PictureDTO';
import { UpdatePictureDTO } from './models/UpdatePictureDTO';
import { ChanegPasswordDTO } from './models/ChangePasswordDTO';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  BaseUrl =  "https://localhost:44314/"

  private UserSubject = new BehaviorSubject<UserDTO | null>(null);

  constructor(private http:HttpClient, private _snackBar: MatSnackBar) {
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
      if(ret) 
      this._snackBar.open("Welcome " + ret?.name, 'Close', {
        duration: 3000
      })
      else 
      this._snackBar.open("Your Username or Password might not be correct", 'Close', {
        duration: 3000
      })
      return ret
  }, err => {this.handleError(err)})
  return new UserDTO
    
  }



  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(
        `Backend returned code ${error.status}, body was: `, error.error);
    }
    this._snackBar.open("Your Username or Password might not be correct", 'Close', {
      duration: 3000
    })
    // Return an observable with a user-facing error message.
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
}
