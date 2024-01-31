import { Component } from '@angular/core';
import {Book} from "../Models/book";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Author} from "../Models/author";
import {DatePipe} from "@angular/common";
import {Router} from "@angular/router";


@Component({
  selector: 'app-add-book',
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.css']
})
export class AddBookComponent {

  constructor(private http:HttpClient, private datePipe: DatePipe, private router: Router) {
  }

  book: any = {
    Title: '',
    Description: '',
    Rating: 0,
    DateAdded: new Date(),
    Available: false,
    AuthorId: '',
    FirstName: '',
    LastName: '',
    BirthDate: new Date(),
    Image: null,
  };




  onImageSelected(event: any) {


    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        if (typeof reader.result === 'string') {
          const base64String = reader.result.split(',')[1];
          this.book.Image = base64String;
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

  onSubmit() {
    const formData = new FormData();
    formData.append('Title', this.book.Title);
    formData.append('Description', this.book.Description);
    formData.append('Rating', this.book.Rating.toString());
    formData.append('DateAdded', this.datePipe.transform(this.book.DateAdded, 'yyyy/MM/dd'));
    formData.append('Available', this.book.Available.toString());
    formData.append('AuthorId', this.book.AuthorId);
    formData.append('FirstName', this.book.FirstName);
    formData.append('LastName', this.book.LastName);
    formData.append('BirthDate', this.datePipe.transform(this.book.BirthDate, 'yyyy/MM/dd'));

    const byteCharacters = atob(this.book.Image);
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

    formData.append('imageFile', blob, 'image.jpg');

    this.http.post('https://localhost:44330/api/Books/add', formData).subscribe({
      next: (response) => {
        console.log('Book added successfully:', response);

        this.router.navigate(['/register']);

      },
      error: (error) => {
        console.error('Error adding book:', error);
      }
    });

  }


}
