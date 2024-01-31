import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookComponent } from './book/book.component';
import { AuthorComponent } from './author/author.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { BookDetailsComponent } from './book-details/book-details.component';
import { EditBookComponent } from './edit-book/edit-book.component';
import { AddBookComponent } from './add-book/add-book.component';
import { AuthorDetailComponent } from './author-detail/author-detail.component';
import { AddAuthorComponent } from './add-author/add-author.component';
import { AuthGuard } from './guards/auth.guard';
import { CanDeactivateGuard } from './guards/can-deactivate.guard';


const routes: Routes = [
  { path: 'books/edit', component: EditBookComponent, canActivate: [AuthGuard] },
  { path: 'books/add', component: AddBookComponent, canActivate: [AuthGuard] },
  { path: 'authors/add', component: AddAuthorComponent, canActivate: [AuthGuard] },
  { path: 'authors/:id', component: AuthorDetailComponent, canActivate: [AuthGuard]},
  { path: 'books/:id', component: BookDetailsComponent, canActivate: [AuthGuard] },
  { path: 'books', component: BookComponent, canActivate: [AuthGuard] },
  { path: 'authors', component: AuthorComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  {
    path: 'register',
    component: RegisterComponent,
    canDeactivate: [CanDeactivateGuard],
  },

  { path: '', redirectTo: '/books', pathMatch: 'full' },
  { path: '**', redirectTo: '/books', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
