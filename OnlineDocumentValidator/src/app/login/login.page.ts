import { Component, OnInit } from '@angular/core';
import {ApiService} from '../services/api.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {

  email: string;
  password: string;
  constructor(private apiService: ApiService, private router: Router) { }

  ngOnInit() {
  }
  async login(){
       (await this.apiService.login(this.email, this.password)).subscribe(res => {
      localStorage.setItem('token', res.token);
      localStorage.setItem('roles', res.roles);
         this.router.navigateByUrl('/home');
      console.log(localStorage.getItem('token'), localStorage.getItem('roles'));
    }, error => {
      console.log(error);
    });


  }
}
