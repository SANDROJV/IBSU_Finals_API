import {BookAuthor} from "./book-author";

export interface Book {

  id: number;
  title: string;
  description: string;
  image: any;
  rating: number;
  dateAdded: Date;
  available: boolean;
  authors: any[];
  imageUrl?: string;
  imageBase64?: string;
}
