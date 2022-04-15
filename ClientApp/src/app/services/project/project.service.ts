import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private readonly token: string;
  private readonly baseUrl: string;

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.token = "";
    this.baseUrl = baseUrl;
    let accessToken = localStorage.getItem("accessToken");
    if(accessToken) this.token = accessToken;
  }

  CreateProject(formData: FormData): Observable<Object> {
    let url = `${this.baseUrl}/project/`;
    let res = this.httpClient.post(url, formData, {
      headers: {
        Authorization: `Bearer ${this.token}`
      }
    });

    return res;
  }

  GetProject(id?: number): Observable<any[]> {
    if(id) {
      let url = `${this.baseUrl}/project/${id}`;
      return this.httpClient.get<Sprint[]>(url, {
        headers: {
          Authorization: `Bearer ${this.token}`
        }
      });
    } else
    {
      let url = `${this.baseUrl}/project/`;
      return this.httpClient.get<Project[]>(url, {
        headers: {
          Authorization: `Bearer ${this.token}`
        }
      });
    }
  }

  UpdateProject(formData: FormData): Observable<Object> {
    let url = `${this.baseUrl}/project/`;
    let res = this.httpClient.put(url, formData, {
      headers: {
        Authorization: `Bearer ${this.token}`
      }
    });

    return res;
  }

  RemoveProject(id?: number): Observable<Object> {
    let url = `${this.baseUrl}/project/${id}`;
    let res = this.httpClient.delete(url, {
      headers: {
        Authorization: `Bearer ${this.token}`
      }
    });

    return res;
  }
}

export interface Project
{
  Id: number;
  Title: string;
  Description: string;
  Username?: string;
  CreatorId: number;
  Budget: number;
  BeginDate: Date;
  EndDate: Date;
  Status: number;
}

export interface Sprint
{
  Id: number;
  Title: string;
  Description: string;
  TotalAmount: number;
  BeginDate: Date;
  EndDate: Date;
  Priority: number;
  Status: number;
}
