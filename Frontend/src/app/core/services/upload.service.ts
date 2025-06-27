import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { APIResponse } from 'src/app/sap/shared/interfaces/apiresponse';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class UploadService {

  constructor(private http: ApiService) { }

  uploadFile(url: string, file: File): Observable<APIResponse> {

    let formData = new FormData();
    formData.append('file', file);

    return this.http.post<APIResponse>(url, formData);

  }

  uploadMultiFile(url: string, files: File[]): Observable<APIResponse> {

    let formData = new FormData();
    files.forEach(file => formData.append('files', file));
    return this.http.post<APIResponse>(url, formData);
  }
}
