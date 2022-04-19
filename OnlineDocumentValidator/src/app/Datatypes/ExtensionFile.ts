import {SafeResourceUrl, SafeUrl} from '@angular/platform-browser';

export class ExtensionFile implements File{
  readonly lastModified: number;
  readonly name: string;
  readonly size: number;
  readonly type: string;
  readonly webkitRelativePath: string;
  path?: SafeResourceUrl;
  arrayBuffer(): Promise<ArrayBuffer> {
    return Promise.resolve(undefined);
  }

  slice(start?: number, end?: number, contentType?: string): Blob {
    return undefined;
  }

  stream(): ReadableStream {
    return undefined;
  }

  text(): Promise<string> {
    return Promise.resolve('');
  }
}
