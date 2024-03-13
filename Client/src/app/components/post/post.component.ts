import { Component, EventEmitter, Output } from '@angular/core';
import Post from 'src/app/interfaces/post';

import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent {

  constructor(private formBuilder: FormBuilder) { }
  directions = ['asc', 'desc'];
  sortType = ['id', 'reads', 'likes', 'popularity'];
  @Output() submitForm = new EventEmitter<Post>();

  postForm = this.formBuilder.group({
    tags: '',
    sortBy: '',
    direction: ''
  });

  public onSubmit() {
    this.submitForm.emit(this.postForm.value);
  }
}
