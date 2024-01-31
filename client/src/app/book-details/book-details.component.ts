import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {Book} from "../Models/book";
import {ActivatedRoute, Router} from "@angular/router";
import {BookService} from "../services/book.service";
import {BookDataService} from "../services/book-data.service";
import {DatePipe} from "@angular/common";

@Component({
  selector: 'app-book-details',
  templateUrl: './book-details.component.html',
  styleUrls: ['./book-details.component.css']
})
export class BookDetailsComponent implements OnInit{

  book: Book;


  isEditing: boolean = false;
  updatedTitle: string;
  updatedDescription: string;
  updatedImage:any;
  updatedRating:any;
  updatedAuthorId:any;
  updatedFirstName:string;
  updatedLastName:string;
  updatedBirthdate: any;

  constructor(private route: ActivatedRoute,
              private bookService: BookService,
              private cdRef: ChangeDetectorRef,
              private bookDataService: BookDataService,
              private router: Router,
              private datePipe: DatePipe) { }


  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    this.bookService.getBookById(id).subscribe({
      next: (book: Book) => {
        console.log('logging book: ',book);
        this.book = book;

        this.processImages();
        this.cdRef.detectChanges();

        this.bookDataService.setSelectedBook(book);
      },
      error: (error) => {
        console.error('Error loading book details: ', error);
      },
    });
    this.bookService.takeBookById(id);
  }

  saveChanges(){
    const formattedBirthDate = new Date(this.book.authors[0].BirthDate);
    console.log(formattedBirthDate);
    const editedBook = {
      BookId: this.book.id,
      Title: this.updatedTitle,
      Description: this.updatedDescription,
      ImageFile: this.updatedImage,
      Rating: this.updatedRating,
      AuthorId: this.book.authors[0].id,
      FirstName: this.book.authors[0].FirstName,
      LastName: this.book.authors[0].LastName,
      BirthDate: this.book.authors[0].birthDate
    }

    const byteCharacters = atob(this.updatedImage);
    const byteArrays = [];
    for (let offset = 0; offset < byteCharacters.length; offset += 512) {
      const slice = byteCharacters.slice(offset, offset + 512);
      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }
    const blob = new Blob(byteArrays, { type: 'image/jpeg' });

    const formData = new FormData();
    formData.append('BookId', editedBook.BookId.toString()); 
    formData.append('Title', editedBook.Title);
    formData.append('Description', editedBook.Description);
    formData.append('ImageFile', blob, 'image.jpg');
    formData.append('Rating', editedBook.Rating.toString());
    formData.append('AuthorId', editedBook.AuthorId.toString());
    formData.append('FirstName', editedBook.FirstName);
    formData.append('LastName', editedBook.LastName);
    formData.append('BirthDate', this.datePipe.transform(editedBook.BirthDate, 'yyyy/MM/dd'));

    this.bookService.updateBook(formData).subscribe({
      next: (response) => {
        console.log('Book updated successfully', response)
      },
      error: (error) => {
        console.error('Error updating book:', error);
      }
    });

    this.isEditing = false;

  }

  editAuthor() {
    this.isEditing = true;
  }

  goToEdit() {
    this.bookService.takeBookById(this.book.id);
    this.router.navigate(['books', 'edit']);
  }

  takeReturn() {
    this.bookService.changeBookStatus(this.book.id).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => {
        console.error('Error changing book status: ', error);
      }});

    const currentUrl = this.router.url;
    this.router.navigateByUrl('/refresh', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    });

    }
    deleteBook(){
    this.bookService.deleteBook(this.book.id).subscribe({
      next: (response) => {
        console.log(response);

      },
      error: (error) => {
        console.error('Error deleting book: ', error);
      }});

      this.router.navigate(['books']);
    }

  onImageSelected(event: any) {


    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        if (typeof reader.result === 'string') {
          const base64String = reader.result.split(',')[1];
          this.updatedImage = base64String;
        } else {
          console.error('Invalid image format.');
        }
      };
      reader.onerror = (error) => {
        console.error('Error reading image:', error);
      };
      reader.readAsDataURL(file);
    }
  }

  private processImages(): void {

      if (this.book.image && this.book.image.length > 0) {
        const byteString = atob(this.book.image);
        const bytes = new Uint8Array(byteString.length);
        for (let i = 0; i < byteString.length; i++) {
          bytes[i] = byteString.charCodeAt(i);
        }

        const blob = new Blob([bytes], { type: 'image/jpeg' });
        this.book.imageUrl = URL.createObjectURL(blob);
      } else {
        this.book.imageUrl = '/assets/Book-Cover-Template.jpg';
      }
  }

}
