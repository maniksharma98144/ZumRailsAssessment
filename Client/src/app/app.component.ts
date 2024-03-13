import { Component } from '@angular/core';
import { PostService } from '../app/services/post.service';
import Post from './interfaces/post';
import PostData from './interfaces/postData';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Client';

  public postData?: PostData[];
  constructor(private postService: PostService) { }

  public async getPosts(data: Post) {
    this.postService.getPosts(data).subscribe(posts => this.postData = posts);
  }
}
