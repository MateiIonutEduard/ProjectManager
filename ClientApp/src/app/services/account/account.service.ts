import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private readonly baseUrl: string;

  constructor(private client: HttpClient)
  {
    this.baseUrl = 'api/account/';
  }

  SignIn(formData: FormData): Observable<Token> {
    return this.client.post<Token>(this.baseUrl, formData);
  }

  GetAccount(): Observable<any> {
    const token = localStorage.getItem("accessToken");
    return this.client.get<any>(this.baseUrl, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  SignUp(formData: FormData): Observable<Token> {
    return this.client.post<Token>(this.baseUrl, formData);
  }

  Refresh(): Observable<Token> {
    const token = localStorage.getItem("accessToken");
    return this.client.put<Token>(this.baseUrl, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  SignOut() {
    const token = localStorage.getItem("accessToken");
    return this.client.delete<any>(this.baseUrl, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  }
}

export interface Token
{
  accessToken: string;
}
