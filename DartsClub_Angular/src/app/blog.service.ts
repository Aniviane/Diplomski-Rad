import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Blog } from './models/Blog';
import { ApproveDTO } from './models/ApproveDTO';

@Injectable({
  providedIn: 'root'
})
export class BlogService {

  BaseUrl =  "https://localhost:44314/"


  constructor(private http:HttpClient) { }


  deleteBlog(blogId : string) : Observable<boolean> {
    return this.http.delete<boolean>(this.BaseUrl + 'api/Blog/' + blogId)
  }

  approveBlog(request : ApproveDTO) : Observable<boolean> {
    return this.http.put<boolean>(this.BaseUrl + 'api/Blog/Approve/', request)
  }

  getMyBlogs(id : string) :  Observable<Blog[]> {
    return this.http.get<Blog[]>(this.BaseUrl + 'api/Blog/MyBlogs/' + id)
  }

  getBlogs() : Observable<Blog[]> {
    return this.http.get<Blog[]>(this.BaseUrl + 'api/Blog/Approved')
  }

  getNotApprovedBlogs() : Observable<Blog[]> {
    return this.http.get<Blog[]>(this.BaseUrl + 'api/Blog/NotApproved')
  }

  postBlog(blog : Blog) : Observable<string> {
    return this.http.post<string>(this.BaseUrl + 'api/Blog/', blog)
  }
}
