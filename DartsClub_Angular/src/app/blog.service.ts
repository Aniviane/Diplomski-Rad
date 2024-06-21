import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Blog } from './models/Blog';

@Injectable({
  providedIn: 'root'
})
export class BlogService {

  BaseUrl =  "https://localhost:44314/"


  constructor(private http:HttpClient) { }



  getBlogs() : Observable<Blog[]> {
    return this.http.get<Blog[]>(this.BaseUrl + 'api/Blog/Approved')
  }
}
