import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable, of} from 'rxjs';
import ValidatorUser from '../Datatypes/ValidatorUser';
import {OneApiResult} from '../Datatypes/OneApiResult';
import {catchError, map, retry} from 'rxjs/operators';
import {Companies} from '../Datatypes/Companies';
import {error} from 'protractor';
import {posix} from 'path';

// noinspection TypeScriptValidateTypes
@Injectable({
  providedIn: 'root'
})
export class ApiService {

  tokenState: boolean;
   basicApiUrl = 'https://localhost:5001/api';
  constructor(private http: HttpClient) { }

  async getTokenState() {
    console.log('called state');
    await this.getTokenValid().then((o) => {
      o.subscribe((t) => {this.tokenState = t;});
    });
  }
 async login(email: string, password: string): Promise<Observable<any>>{
    return this.http.post(this.basicApiUrl + '/authentication/authenticate',
      {username: email, password, token:'', roles:[]});
  }

  async getMail(id: string){
    const headers = new HttpHeaders().set('Content-Type', 'text/plain; charset=utf-8');
    return this.http.get(this.basicApiUrl +'/user_emailByCompany/', {
     observe: 'body',
     headers,
     responseType: 'text',
     params:{
     accId: id
     },
   });
  }

//proberen op te lossen later
 async getTokenValid(): Promise<Observable<boolean>>{
    return this.http.post<boolean>(this.basicApiUrl+'/authentication/checkToken', {
      token:localStorage.getItem('token')
    });
  }
  async getLocalFile() {
    const res = await fetch('../assets/sample2.json');
    return await res.json();
  }
  getCompanies(): Observable<Companies[]>{
    return this.http.get(
      `${this.basicApiUrl+'/companies'}`,
      {
        observe: 'body',
        headers: new HttpHeaders({
          // eslint-disable-next-line @typescript-eslint/naming-convention
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }),
        responseType: 'json'
      }
    )     .pipe(
      // eslint-disable-next-line @typescript-eslint/no-shadow
      catchError(error => {
        console.error(error);
        return of(undefined);
      }),
      // Retry the request 3 times before throwing an error.
      // Useful for intermittent network connections.
      retry(3)
    );
  }
    getUsers(): Observable<ValidatorUser[]> {
    return this.http
      .get<ValidatorUser[]>(
        `${this.basicApiUrl}`,
        {
          observe: 'body',
          headers: new HttpHeaders({
            // eslint-disable-next-line @typescript-eslint/naming-convention
            Authorization: `Bearer ${localStorage.getItem('token')}`
          }),
          responseType: 'json'
        }
      )     .pipe(
        // eslint-disable-next-line @typescript-eslint/no-shadow
        catchError(error => {
          console.error(error);
          return of(undefined);
        }),
        // Retry the request 3 times before throwing an error.
        // Useful for intermittent network connections.
        retry(3)
      );
  }
}
