import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Book } from '../Models/book';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class BookService implements OnInit {
  private apiUrl = 'https://localhost:44330/api/Books/';

  editId: number;

  private editIdSubject: Subject<number> = new Subject<number>();
  editId$: Observable<number> = this.editIdSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit(): void {}

  url: string;

  getAllBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.apiUrl);
  }

  getBookById(Id): Observable<Book> {
    return this.http.get<Book>(this.apiUrl + Id);
  }

  takeBookById(Id) {
    this.editId = Id;
    this.editIdSubject.next(Id);
  }

  changeBookStatus(Id) {
    console.log('this.url');
    this.url = `${this.apiUrl}/${Id}/toggle-status`;
    return this.http.put(`${this.apiUrl}${Id}/toggle-status`, Id);
    console.log(this.url);
  }
  
  getEditId(): number {
    return this.editId;
  }

  updateBook(book: any): Observable<any> {
    return this.http.put('https://localhost:44330/api/Books/edit', book);
  }

  deleteBook(Id) {
    return this.http.delete(this.apiUrl + Id, Id);
  }
}
