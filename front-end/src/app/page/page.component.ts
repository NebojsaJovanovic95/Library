import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface IData {
  name: string
  content: string
  user: string
}

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrl: './page.component.css'
})
export class PageComponent implements OnInit {
  data: IData = {
    name: '',
    content: '',
    user: '',
  }
  constructor(private http: HttpClient) {

  }

  ngOnInit(): void {
      this.http.get<IData>('http://localhost:7071/api/users/banana')
      .subscribe(
        (data) => {
          this.data = data
        }
      )
  }
}
