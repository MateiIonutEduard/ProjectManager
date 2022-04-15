import {Component, Inject, OnInit} from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-account-menu',
  templateUrl: './account-menu.component.html',
  styleUrls: ['./account-menu.component.css']
})
export class AccountMenuComponent {
  public account: AccountMenu;
  public baseUrl: string;
  public auth: boolean;

  constructor(private router: Router, @Inject('BASE_URL') baseUrl: string) {
    this.auth = false;
    this.baseUrl = baseUrl;

    this.account = {
      id: 0,
      accessToken: "",
      username: ""
    };

    this.loadAccount();
  }

  loadAccount() {
    let id = localStorage.getItem("id");
    let token = localStorage.getItem("accessToken");
    let username = localStorage.getItem("username");

    if(id && token && username)
    {
      this.account.id = parseInt(id);
      this.account.accessToken = token;

      this.account.username = username;
      this.auth = true;
    }
    else
      this.auth = false;
  }

  SignOut() {
    if(this.auth)
    {
      localStorage.removeItem("id");
      localStorage.removeItem("accessToken");
      localStorage.removeItem("username");

      this.router.navigate(['/'])
        .then(nav => {
          console.log(nav);
        }, err => {
          console.log(err)
        });

      this.auth = false;
    }
  }
}

export interface AccountMenu
{
  id: number;
  accessToken: string;
  username: string;
}
