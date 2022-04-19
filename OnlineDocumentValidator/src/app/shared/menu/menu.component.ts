import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';
import {MenuController} from '@ionic/angular';
import {ApiService} from '../../services/api.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit {

  constructor(private router: Router, private menuController: MenuController, private api: ApiService) { }
  checkRole(): (string | null){
    return localStorage.getItem('roles');
  }

 async logout(): Promise<void>{
    localStorage.removeItem('token');
    localStorage.removeItem('roles');

    console.log(localStorage.getItem('token') + ' failed');
    this.router.navigate(['/home']).then(() => {
      window.location.reload();
      this.menuController.close();
    });

  }
  ngOnInit() {}

}
