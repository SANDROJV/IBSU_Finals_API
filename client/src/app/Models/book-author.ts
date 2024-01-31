import {Book} from "./book";
import {Author} from "./author";

export interface BookAuthor {
  Title: string;
  Description: string;
  Rating: number;
  DateAdded: Date;
  Available: boolean;
  Authors: Author[];

}
