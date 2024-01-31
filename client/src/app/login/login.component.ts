import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit() {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  goToRegister() {
    this.router.navigate(['/register']);
  }

  async login() {
    if (this.loginForm.invalid) {
      alert('Please fill in the required fields.');
      return;
    }

    const formData = this.loginForm.value;

    try {
      const response = await this.http
        .post('https://localhost:44330/api/User/login', formData, {
          responseType: 'text',
        })
        .toPromise();

      const token = response.trim();

      localStorage.setItem('token', token);
      this.router.navigate(['/home']);
    } catch (error) {
      console.error(error);
      alert('Login failed. Please check your credentials.');
    }
  }
}
