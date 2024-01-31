import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'imageBase64' })
export class ImageBase64Pipe implements PipeTransform {
  transform(value: any): string {
    if (value && value.length > 0) {
      const base64String = this.arrayBufferToBase64(value);
      return 'data:image/jpeg;base64,' + base64String;
    } else {
      return '/assets/Book-Cover-Template.jpg';
    }
  }

  private arrayBufferToBase64(buffer: any): string {
    let binary = '';
    const bytes = new Uint8Array(buffer);
    const len = bytes.byteLength;
    for (let i = 0; i < len; i++) {
      binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
  }
}
