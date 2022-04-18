import { Injectable } from '@angular/core';
import {Observable, Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class StateService {
  private subject: Subject<boolean>;

  constructor() {
    this.subject = new Subject<boolean>();
  }

  SendState(logged: boolean) {
    this.subject.next(logged);
  }

  GetState(): Observable<boolean> {
    return this.subject.asObservable();
  }
}
