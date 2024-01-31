import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root',
})
export class TranslationService {
  constructor(private translateService: TranslateService) {}

  setLanguage(lang: string): void {
    this.translateService.use(lang);
  }
}
