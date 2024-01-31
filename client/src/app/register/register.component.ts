import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CanComponentDeactivate } from '../guards/can-deactivate.guard';
import { TranslateService } from '@ngx-translate/core';
import { TranslationService } from '../services/translation.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit, CanComponentDeactivate {
  registrationForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private translate: TranslateService,
    private translationService: TranslationService
  ) {
    this.translate.setDefaultLang('en');
  }

  changeLanguage(lang: string): void {
    this.translationService.setLanguage(lang);
  }

  ngOnInit() {
    this.registrationForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confPassword: [''],
    });
  }

  register() {
    const formData = this.registrationForm.value;

    if (formData.password !== formData.confPassword) {
      alert('Passwords do not match.');
      return;
    }

    this.http
      .post('https://localhost:44330/api/User/register', formData)
      .subscribe(
        (response) => {
          alert('Registration successful.');
          this.router.navigate(['/login']);
        },
        (error) => {
          alert('Registration failed: ' + error.error.message);
        }
      );
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }

  canDeactivate(): boolean {
    if (this.registrationForm.dirty) {
      return window.confirm(
        'You have unsaved changes. Do you really want to leave?'
      );
    }
    return true;
  }
}
