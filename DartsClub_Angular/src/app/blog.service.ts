import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Blog } from './models/Blog';
import { ApproveDTO } from './models/ApproveDTO';
import { SearchQueryDTO } from './models/SearchQueryDTO';

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

  updateBlog(blog : Blog) : Observable<boolean>  {
    return this.http.put<boolean>(this.BaseUrl + 'api/Blog/', blog)
  }

  searchBlogsByContent(text : string) : Observable<Blog[]> {
    return this.http.get<Blog[]>(this.BaseUrl + 'api/Blog/Content/' + text)
  }
  searchBlogsByTitle(text : string) : Observable<Blog[]> {
    return this.http.get<Blog[]>(this.BaseUrl + 'api/Blog/Title/' + text)
  }

  searchBlogs(query : SearchQueryDTO) : Observable<Blog[]> {
    let params = new HttpParams().set("content",query.content).append("title",query.title)
    return this.http.get<Blog[]>(this.BaseUrl + "api/Blog/Search", {params: params})
  }
}


