import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import Post from '../interfaces/post';
import { environment } from 'src/environments/environment';
import PostData from '../interfaces/postData';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  constructor(private http: HttpClient) {
  }

  public getPosts(data: Post): Observable<PostData[]> {
    return this.http.get<PostData[]>(`${environment.serverUrl}?tags=${data.tags}&sortBy=${data.sortBy}&direction=${data.direction}`);
  }
}
