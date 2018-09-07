import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RequestQuery } from '../../models/request-query-model';

@Injectable({
  providedIn: 'root'
})

export class ApiServiceService {

  private _httpHeaders: HttpHeaders;
  constructor(private _httpClient: HttpClient) {
    this._httpHeaders = new HttpHeaders();
    this._httpHeaders.append("Content-Type", "application/json; charset=utf-8");
  }

  getAll<T>(apiEndPoint: string, requestQuery?: RequestQuery, queryString?: object): Observable<T> {
    let params: URLSearchParams = this.getParams(queryString);
    let requestQueryParams: URLSearchParams = this.getParams(requestQuery);
    if (queryString || requestQuery)
      return this._httpClient.get<T>(`${apiEndPoint}?${requestQueryParams}&${params}`, { headers: this._httpHeaders });
    else
      return this._httpClient.get<T>(`${apiEndPoint}`, { headers: this._httpHeaders });
  }

  get<T>(apiEndPoint: string, queryString?: object): Observable<T> {
    let params: URLSearchParams = this.getParams(queryString);
    if (queryString)
      return this._httpClient.get<T>(`${apiEndPoint}?${params}`, { headers: this._httpHeaders });
    else
      return this._httpClient.get<T>(`${apiEndPoint}`, { headers: this._httpHeaders });
  }

  post<T>(apiEndPoint: string, data: any, queryString?: object): Observable<T> {
    let params: URLSearchParams = this.getParams(queryString);
    if (queryString)
      return this._httpClient.post<T>(`${apiEndPoint}?${params}`, data, { headers: this._httpHeaders });
    else
      return this._httpClient.post<T>(`${apiEndPoint}`, data, { headers: this._httpHeaders });
  }

  put<T>(apiEndPoint: string, data: any, queryString?: object): Observable<T> {
    let params: URLSearchParams = this.getParams(queryString);
    if (queryString)
      return this._httpClient.put<T>(`${apiEndPoint}/?${params}`, data, { headers: this._httpHeaders });
    else
      return this._httpClient.put<T>(`${apiEndPoint}`, data, { headers: this._httpHeaders });
  }

  delete<T>(apiEndPoint: string, queryString: object): Observable<T> {
    let params: URLSearchParams = this.getParams(queryString);
    if (queryString)
      return this._httpClient.delete<T>(`${apiEndPoint}?${params}`, { headers: this._httpHeaders });
    else
      return this._httpClient.delete<T>(`${apiEndPoint}`, { headers: this._httpHeaders });
  }

  private getParams(queryString: object): URLSearchParams {
    let params = new URLSearchParams();
    if (typeof queryString === "object") {
      for (let prop in queryString) {
        if (prop)
          params.set(prop, queryString[prop]);
      }
    }
    return params;
  }

  private requestQueryParams(queryString: object) {
    let params = new URLSearchParams();
    if (typeof queryString === "object") {
      for (let prop in queryString) {
        if (prop)
          params.set(`requestQuery.${prop}`, queryString[prop]);
      }
    }
    return params;
  }
}
