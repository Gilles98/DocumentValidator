import { Component } from '@angular/core';
import { NgxFileDropEntry, FileSystemFileEntry, FileSystemDirectoryEntry } from 'ngx-file-drop';
import {ApiService} from '../services/api.service';
import {ExtensionFile} from '../Datatypes/ExtensionFile';
import {DomSanitizer} from '@angular/platform-browser';
import {PdfExtractorService} from '../services/pdf/pdf-extractor.service';
import {KeyValue} from '@angular/common';



@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {

  files: File[] = [];
  filesDrop: NgxFileDropEntry[] = [];
  file: ExtensionFile = undefined;
  jsonKeys: string[] = [];
  jsonFile: any;
  constructor(private apiService: ApiService, private sanitizer: DomSanitizer, private pdfExtractorService: PdfExtractorService) {


  }
  public openDirectory(event) {
    event.preventDefault();
    const input: HTMLInputElement = document.createElement('input');
    input.type = 'file';
    //drag toevoegen
    input.accept = '.json,.pdf';
    input.multiple = true;
    input.onchange = _select => this.setFiles(_select, input.files);


    input.click();
}
  public dropped(files: NgxFileDropEntry[]) {
    this.filesDrop = files;
    for (const droppedFile of this.filesDrop) {

      // Is it a file?
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
        fileEntry.file((file: File) => {

          // Here you can access the real file
          console.log(droppedFile.relativePath, file);
        });
      } else {
        // It was a directory (empty directories are added, otherwise only files)
        const fileEntry = droppedFile.fileEntry as FileSystemDirectoryEntry;
        console.log(droppedFile.relativePath, fileEntry);
      }
    }
  }

  public fileOver(event): void{
    console.log(event);
  }

  public fileLeave(event): void{
    console.log(event);
  }
  public changeValue(key): void{
    console.log(this.jsonFile[key]);
  }

  public returnArray(key){
    return Object.keys(key);
  }

  public changeObjectValue(key, object: KeyValue<any, any>): void{
    this.jsonFile.address[object.key] = object.value;
    console.log(this.jsonFile);
  }

  public typeControle(key): string{

    const type = typeof(this.jsonFile[key]);

    if (type.toLowerCase() === 'object'){
      return 'object';
    }
    return 'not object';
  }

  public save(): void{
    console.log(this.jsonFile);
    const sJson = JSON.stringify(this.jsonFile);
    const element = document.createElement('a');
    element.setAttribute('href', 'data:text/json;charset=UTF-8,' + encodeURIComponent(sJson));
    element.setAttribute('download', 'verified.json');
    element.style.display = 'none';
    document.body.appendChild(element);
    element.click(); // simulate click
    document.body.removeChild(element);
  }
private async setFiles(event: any, files: FileList) {
  console.log(event.target.value + 'path');

  this.files = Array.from(files);
  console.log(this.files[0]);
  this.file = this.files[0];

  const test = this.sanitizer.bypassSecurityTrustResourceUrl(URL.createObjectURL(event.target.files[0]));
  this.file.path = test;
/*
  const reader = new FileReader();
  reader.readAsDataURL(this.file);
  reader.onload = () => {
    console.log(reader.result);

    console.log(this.file.path.toString());
    this.pdfExtractorService.readPdf(URL.createObjectURL(event.target.files[0]));
  };
*/
  this.jsonFile = await this.apiService.getLocalFile();
  this.jsonKeys = Object.keys(this.jsonFile);
}

}
