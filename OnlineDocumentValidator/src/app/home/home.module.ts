import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { FormsModule } from '@angular/forms';
import { HomePage } from './home.page';

import { HomePageRoutingModule } from './home-routing.module';
import {NgxFileDropModule} from 'ngx-file-drop';
import {PdfViewerModule} from 'ng2-pdf-viewer';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        IonicModule,
        HomePageRoutingModule,
        NgxFileDropModule,
        PdfViewerModule
    ],
  declarations: [HomePage]
})
export class HomePageModule {}
