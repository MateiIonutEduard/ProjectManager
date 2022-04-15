import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private readonly baseUrl: string;

  constructor(private client: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {
    this.baseUrl = baseUrl;
  }

  SignIn(formData: FormData): Observable<Token> {
    let url = 'api/account/';
    return this.client.post<Token>(url, formData);
  }

  GetAccount(): Observable<any> {
    let url = 'api/account/';
    const token = localStorage.getItem("accessToken");
    return this.client.get<any>(url, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  SignUp(formData: FormData): Observable<Token> {
    let url = 'api/account/';
    return this.client.post<Token>(url, formData);
  }
}

export interface Account
{
  Id?: number;
  Username?: string;
  password: string;
  address: string;
  profile?: File;
}

export interface Token
{
  accessToken: string;
}
