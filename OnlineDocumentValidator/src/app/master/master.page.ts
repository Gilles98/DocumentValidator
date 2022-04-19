import { Component, OnInit } from '@angular/core';
import ValidatorUser from '../Datatypes/ValidatorUser';
import {ApiService} from '../services/api.service';
import {AlertController} from '@ionic/angular';

@Component({
  selector: 'app-master',
  templateUrl: './master.page.html',
  styleUrls: ['./master.page.scss'],
})
export class MasterPage implements OnInit {

   allUsers = this.apiService.getCompanies();
   allEmployees = this.apiService.getUsers();
  constructor(public apiService: ApiService, private alertCtrl: AlertController) {
     apiService.getUsers().subscribe(x =>{
        console.log(x);
      });
  }

  async activateEdit(emp: ValidatorUser){
   await this.apiService.getMail(emp.accountId).then(o => {
     o.subscribe(async (r) => {
       console.log(r);
       const alert: HTMLIonAlertElement = await this.alertCtrl.create({
         inputs: [
           {
             name: 'name',
             placeholder: (emp.naam !== '') ? emp.naam:'Hele naam'
           },
           {
             name: 'password',
             placeholder: 'Wachtwoord',
             type: 'password'
           },
           {
             name: 'email',
             placeholder: (r !== '') ? r: 'Email'
           }
         ], buttons: [
           {
             text: 'Sluiten',
             role: 'cancel',
             handler: data => {
               this.alertCtrl.dismiss();
             }
           },
           {
             text: 'Aanpassen',
             handler: data => {
               if (data.email !== '' || data.password !== '' || data.name !== '') {
               }
               else {

                 return false;
               }
               this.alertCtrl.dismiss();
             }
           }]
       });
       await alert.present();
     });
   });
  }

  ngOnInit() {
  }

}
