import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { M3Config } from '../models/export.model';

@Injectable()
export class HttpService {

  private apiUrl = '';

  constructor(private http: HttpClient, @Inject('config')  config: M3Config) {
    if (config) {
        this.apiUrl = config.apiUrl;
      }
  }

  get headers() {
    return new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: 'application/json'
    });
  }


  private handleError(error: HttpErrorResponse) {

    if (error.error instanceof ProgressEvent) {
        console.error(error.error);
    } else {
      console.error(error);
    }
    return throwError(
      'Could not connect to remote server.'
    );

  }


  get(path: string): Observable<any> {
    return this.http
      .get<any>(`${this.apiUrl}${path}`, { headers: this.headers })
      .pipe(
        catchError(err => this.handleError(err))
      );
  }

  post(path: string, body: any): Observable<any> {
    return this.http
      .post<any>(`${this.apiUrl}${path}`, JSON.stringify(body), { headers: this.headers }
    )
    .pipe(
      catchError(err => this.handleError(err))
    );
  }
}
