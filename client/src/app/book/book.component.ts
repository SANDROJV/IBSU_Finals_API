import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {Book} from "../Models/book";
import {BookService} from "../services/book.service";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";
import {Router} from "@angular/router";

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css']
})
export class BookComponent implements OnInit{
  books: Book[];
  authors: any[];
  book: any;

  constructor(private bookService: BookService , private sanitizer: DomSanitizer , private cdRef: ChangeDetectorRef, private router : Router) { }

  ngOnInit(): void {
    this.getAllBooks();
  }


  getAllBooks(): void {
    this.bookService.getAllBooks().subscribe({
      next: (response: any[]) => {
        console.log('log books: ', response);
        this.books = response;
        this.processImages();
        this.cdRef.detectChanges();
      },
      error: (error) => {
        console.error('Error loading books: ', error);
      },
    });
  }



  private processImages(): void {
    for (const book of this.books) {
      if (book.image && book.image.length > 0) {
        const byteString = atob(book.image);
        const bytes = new Uint8Array(byteString.length);
        for (let i = 0; i < byteString.length; i++) {
          bytes[i] = byteString.charCodeAt(i);
        }

        const blob = new Blob([bytes], { type: 'image/jpeg' });
        book.imageUrl = URL.createObjectURL(blob);
      } else {
        book.imageUrl = '/assets/Book-Cover-Template.jpg';
      }
    }
  }

  goToAdd(){
    this.router.navigate(['books/add']);
  }

}
