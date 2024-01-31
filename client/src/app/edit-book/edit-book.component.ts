import { Component, OnInit } from '@angular/core';
import { Book } from '../Models/book';
import { BookService } from '../services/book.service';
import {ActivatedRoute, Router} from "@angular/router";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {BookDataService} from "../services/book-data.service";
import {DatePipe} from "@angular/common";

@Component({
  selector: 'app-edit-book',
  templateUrl: './edit-book.component.html',
  styleUrls: ['./edit-book.component.css']
})
export class EditBookComponent implements OnInit {

  book: any;
  bookId: number;
  newBookForm: FormGroup;

  newBook : any = {
    BookId: null,
    Title: '',
    Description: '',
    ImageFile: null,
    Rating: '',
    AuthorId: null,
    FirstName:'',
    Lastname: '',
    Birthdate: new Date(),
  }



  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private bookService: BookService,
    private bookDataService: BookDataService,
    private datePipe: DatePipe
  ) {}





  ngOnInit(): void {

    this.book = this.bookDataService.getSelectedBook();
    if (this.book) {
      this.initForm();
    } else {
      this.router.navigate(['books']);
    }

    console.log('logging populeted this.book:', this.book);
  }

  initForm() {

    this.newBookForm = this.formBuilder.group({
      title: [this.book.title, Validators.required],
      description: [this.book.description, Validators.required],
      rating: [this.book.rating, [Validators.required, Validators.min(1), Validators.max(10)]],
      available: [this.book.available],

      firstName: [this.book.authors[0].firstName, Validators.required],
      lastName: [this.book.authors[0].lastName, Validators.required],
      birthDate: [this.formatDate(this.book.authors[0].birthDate), Validators.required],
      image: [null], 
    });
  }


  private formatDate(date: string): string {
    const dateObj = new Date(date);
    return this.datePipe.transform(dateObj, 'yyyy/MM/dd') || '';
  }


  onSubmit() {

    console.log('Form Valid:', this.newBookForm.valid);
    console.log('Book ID:', this.book.id);
    console.log('Author ID:', this.book.authors[0].id);

    console.log('Title Valid:', this.newBookForm.get('title').valid, 'Value:', this.newBookForm.get('title').value);
    console.log('Description Valid:', this.newBookForm.get('description').valid, 'Value:', this.newBookForm.get('description').value);
    console.log('Rating Valid:', this.newBookForm.get('rating').valid, 'Value:', this.newBookForm.get('rating').value);
    console.log('Available Valid:', this.newBookForm.get('available').valid, 'Value:', this.newBookForm.get('available').value);
    console.log('FirstName Valid:', this.newBookForm.get('firstName').valid, 'Value:', this.newBookForm.get('firstName').value);
    console.log('LastName Valid:', this.newBookForm.get('lastName').valid, 'Value:', this.newBookForm.get('lastName').value);
    console.log('BirthDate Valid:', this.newBookForm.get('birthDate').valid, 'Value:', this.newBookForm.get('birthDate').value);
    console.log('Image Valid:', this.newBookForm.get('image').valid, 'Value:', this.newBookForm.get('image').value);


    if (!this.newBookForm.valid) {
      console.error('Form data is invalid.');
      return;
    }

    const birthDate = this.newBookForm.get('birthDate').value;
    const formattedBirthDate = this.formatDateForSubmit(birthDate);

    const bookAuthorDTO = {
      BookId: this.book.id,
      Title: this.newBookForm.get('title').value,
      Description: this.newBookForm.get('description').value,
      Rating: this.newBookForm.get('rating').value,
      Available: this.newBookForm.get('available').value,

      AuthorId: this.book.authors[0].id,
      FirstName: this.book.authors[0].firstName,
      LastName: this.book.authors[0].lastName,
      BirthDate: this.formatDateForSubmit(this.book.authors[0].birthDate),
    };


    const formData = new FormData();
    formData.append('Title', bookAuthorDTO.Title);
    formData.append('Description', bookAuthorDTO.Description);
    formData.append('Rating', bookAuthorDTO.Rating);
    formData.append('Available', bookAuthorDTO.Available);
    formData.append('AuthorId', bookAuthorDTO.AuthorId);
    formData.append('FirstName', bookAuthorDTO.FirstName);
    formData.append('LastName', bookAuthorDTO.LastName);
    formData.append('BirthDate', bookAuthorDTO.BirthDate);
    formData.append('imageFile', this.newBookForm.get('image').value);

    console.log('logging bookauthordto: ',bookAuthorDTO);

    console.log('Logging formData before request: ',formData)

    this.bookService.updateBook(formData).subscribe({
      next: (response: any) => {
        console.log('Book updated successfully:', response);
        this.router.navigate(['books', this.book.id]);
      },
      error: (error) => {
        console.error('Error updating book:', error);
      }
    });


  }

  private formatDateForSubmit(date: string): string {
    const dateObj = new Date(date);
    const year = dateObj.getFullYear();
    const month = this.padZero(dateObj.getMonth() + 1);
    const day = this.padZero(dateObj.getDate());
    return `${year}/${month}/${day}`;
  }

  private padZero(value: number): string {
    return value < 10 ? `0${value}` : `${value}`;
  }

  onImageSelected(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      const reader = new FileReader();

      reader.onload = (e: any) => {

        const blob = new Blob([new Uint8Array(e.target.result)], { type: file.type });

        this.newBookForm.get('image').setValue(blob);
      };


      reader.readAsArrayBuffer(file);
    }
  }

  onCancel(){
    this.router.navigate(['books', this.book.id]);
  }

}
