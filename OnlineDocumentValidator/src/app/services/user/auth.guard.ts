import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import {ApiService} from '../api.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private api: ApiService) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean{
    this.api.getTokenState().then(() => {
      console.log(this.api.tokenState);
    });
    if (localStorage.getItem('token') !== null)
    {
      return true;
    }

    else{
      this.router.navigateByUrl('/login');
      return false;
    }
  }

}
