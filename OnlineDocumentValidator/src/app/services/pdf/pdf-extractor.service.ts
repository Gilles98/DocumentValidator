import { Injectable } from '@angular/core';
import * as pdfjsLib from 'pdfjs-dist';
@Injectable({
  providedIn: 'root'
})
export class PdfExtractorService {

  constructor() {if (!pdfjsLib.GlobalWorkerOptions.workerSrc) {
    const WORKER_URL = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjsLib.version}/pdf.worker.min.js`;
    pdfjsLib.GlobalWorkerOptions.workerSrc = WORKER_URL;
  }
  else{
    pdfjsLib.GlobalWorkerOptions.workerSrc = '//mozilla.github.io/pdf.js/build/pdf.worker.js';
  }
  }
  public async readPdf(pdfUrl: string): Promise<string> {
    const pdf = await pdfjsLib.getDocument(pdfUrl).promise;
    const page = await pdf.getPage(1);
    console.log(await page.getTextContent());
    return'';
  }
}
