import {Component, Inject, OnDestroy} from '@angular/core';
import {Router} from "@angular/router";
import {Subscription} from "rxjs";
import {State} from "@popperjs/core";
import {StateService} from "../services/state/state.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnDestroy {
  public auth: boolean;
  private status: Subscription;

  constructor(private state: StateService) {
    this.auth = false;
    let token = localStorage.getItem("accessToken");
    if(token) this.auth = true;
    this.status = state.GetState().subscribe(res => {
      this.auth = res;
    });
  }

  ngOnDestroy(): void {
    this.status.unsubscribe();
  }
}
