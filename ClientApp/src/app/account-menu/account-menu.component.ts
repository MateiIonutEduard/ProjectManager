import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {AccountService} from "../services/account/account.service";
import {StateService} from "../services/state/state.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-account-menu',
  templateUrl: './account-menu.component.html',
  styleUrls: ['./account-menu.component.css']
})
export class AccountMenuComponent implements OnInit, OnDestroy {
  private status: Subscription;
  public account: AccountMenu;
  private baseUrl: string;

  public auth: boolean;
  private filter: any;

  constructor(private state: StateService, private router: Router, private accountService: AccountService, @Inject('BASE_URL') baseUrl: string) {
    this.auth = false;
    this.baseUrl = baseUrl;

    this.account = {
      id: 0,
      accessToken: "",
      username: ""
    };

    this.status = state.GetState().subscribe(res => {
      this.auth = res;
      console.log(res);
    });

    this.loadAccount();
  }

  ngOnInit(): void {
    this.filter = setInterval(() => {
      this.accountService.Refresh().subscribe(res => {
        localStorage.removeItem("accessToken");
        localStorage.setItem("accessToken", res.accessToken);
        this.state.SendState(true);
        this.loadAccount();
      }, error => {
        this.Destroy();
        this.state.SendState(false);
      });
    }, 119000);
    }

  loadAccount(): void {
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

  Destroy(): void {
    localStorage.removeItem("id");
    localStorage.removeItem("accessToken");
    localStorage.removeItem("username");
  }

  SignOut(): void {
    if(this.auth)
    {
      this.accountService.SignOut().subscribe(res => {
        this.router.navigate(['/'])
          .then(nav => {
            this.Destroy();
            this.state.SendState(false);
            this.auth = false;
          }, err => {
            this.Destroy();
            this.state.SendState(false);
            this.auth = false;
          });
      });
    }
  }

  ngOnDestroy(): void {
    clearInterval(this.filter);
  }
}

export interface AccountMenu
{
  id: number;
  accessToken: string;
  username: string;
}
