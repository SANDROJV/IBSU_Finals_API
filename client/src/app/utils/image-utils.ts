import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

export function getSafeImageUrl(sanitizer: DomSanitizer, imageByteArray: any): SafeUrl {
  if (imageByteArray && imageByteArray.length > 0) {
    const blob = new Blob([imageByteArray], { type: 'image/jpeg' });
    const imageUrl: string = URL.createObjectURL(blob);
    return sanitizer.bypassSecurityTrustUrl(imageUrl);
  }
  return '';
}

export function dataURItoBlob(dataURI: string): Uint8Array {
  const byteString = atob(dataURI);
  const arrayBuffer = new ArrayBuffer(byteString.length);
  const uint8Array = new Uint8Array(arrayBuffer);

  for (let i = 0; i < byteString.length; i++) {
    uint8Array[i] = byteString.charCodeAt(i);
  }

  return uint8Array;
}
