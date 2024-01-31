import {Component, OnInit} from '@angular/core';
import {AuthorService} from "../services/author.service";
import {DatePipe} from "@angular/common";
import {Router} from "@angular/router";

@Component({
  selector: 'app-add-author',
  templateUrl: './add-author.component.html',
  styleUrls: ['./add-author.component.css']
})
export class AddAuthorComponent implements OnInit{

  Id: number = 0;
  FirstName: string;
  LastName:string;
  BirthDate: Date;


  constructor(private authorService: AuthorService, private datePipe: DatePipe,private router: Router) {
  }
  ngOnInit(): void {
  }

  addAuthor() {
    const formattedBirthDate = new Date(this.BirthDate);
    console.log(formattedBirthDate);
    const newAuthor = {
      authorId: this.Id,
      firstName: this.FirstName,
      lastName: this.LastName,
      birthDate: formattedBirthDate.toISOString()
    };

    console.log(newAuthor);
    this.authorService.addAuthor(newAuthor).subscribe({
      next: (response) => {
        console.log('Author added successfully!', response);
        this.FirstName = '';
        this.LastName = '';
        this.BirthDate = null;
      },
      error: (error) => {
        console.error('Error adding author:', error);
      }
    });
    this.router.navigate(['/authors']);
  }



}
