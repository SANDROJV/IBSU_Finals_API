import {Component, OnInit} from '@angular/core';
import {Author} from "../Models/author";
import {AuthorService} from "../services/author.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-author',
  templateUrl: './author.component.html',
  styleUrls: ['./author.component.css']
})
export class AuthorComponent implements OnInit{
  constructor(private authorService: AuthorService, private router: Router) {
  }

  authors: any[];
  books: any[];
  ngOnInit(): void {
    this.authorService.getAllAuthors().subscribe({
      next: (response) =>{
        this.authors=response;
        console.log(response);
      },
      error: (error) =>{
        console.log('Error loading authors',error);
      }
    })
  }

  details(id){
    this.router.navigate(['authors',id])
  }
  getAuthorById(id:number){
    this.authorService.getAuthorById(id).subscribe({
      next: (response) =>{
        this.books = response.books;
        console.log('books of current author', this.books);
      },
      error: (error) =>{
        console.log(error)
      }
    })
  }

  goToAddAuthor(){
    this.router.navigate(['/authors/add'])
  }

}
