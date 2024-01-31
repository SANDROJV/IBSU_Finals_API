import {Component, OnInit} from '@angular/core';
import {AuthorService} from "../services/author.service";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-author-detail',
  templateUrl: './author-detail.component.html',
  styleUrls: ['./author-detail.component.css']
})
export class AuthorDetailComponent implements OnInit{
  constructor(private route: ActivatedRoute,private authorService: AuthorService, private router: Router) {
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.getAuthorById(id);
  }

  isEditing: boolean = false;
  editedFirstName: string;
  editedLastName: string;
  editedBirthDate: Date;


  books: any[];
  author: any;
  getAuthorById(id){
    this.authorService.getAuthorById(id).subscribe({
      next: (response) =>{
        this.author = response;
        this.books = response.books;
        console.log('books of current author', this.books);
      },
      error: (error) =>{
        console.log(error)
      }
    })
  }
  editAuthor() {
    this.isEditing = true;
  }

  saveChanges() {
    const formattedBirthDate = new Date(this.editedBirthDate);
    console.log(formattedBirthDate);
    const editedAuthor = {
      authorId: this.author.id,
      firstName: this.editedFirstName,
      lastName: this.editedLastName,
      birthDate: formattedBirthDate.toISOString()
    };

    this.authorService.updateAuthor(editedAuthor).subscribe({
      next: (response) => {
        console.log('Author updated successfully!', response);
        this.getAuthorById(this.author.id);
      },
      error: (error) => {
        console.error('Error updating author:', error);
      }
    });
    this.isEditing = false;
  }




}
