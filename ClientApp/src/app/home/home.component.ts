import {Component, Inject} from '@angular/core';
import {Router} from "@angular/router";
import {Account, AccountService} from "../services/account/account.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public auth: boolean;

  constructor() {
    this.auth = false;
    let token = localStorage.getItem("accessToken");
    console.log(token);
    if(token) this.auth = true;
  }

  SignUp() {

  }
}
