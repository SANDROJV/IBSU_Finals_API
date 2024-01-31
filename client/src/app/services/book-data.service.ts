import { Injectable } from '@angular/core';
import { Book } from '../Models/book';

@Injectable({
  providedIn: 'root',
})
export class BookDataService {
  private selectedBook: Book;

  setSelectedBook(book: Book) {
    this.selectedBook = book;
  }

  getSelectedBook(): any {
    return this.selectedBook;
  }
}
