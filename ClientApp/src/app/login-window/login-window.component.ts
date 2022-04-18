import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {AccountService} from "../services/account/account.service";
import {Router} from "@angular/router";
import {StateService} from "../services/state/state.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-login-window',
  templateUrl: './login-window.component.html',
  styleUrls: ['./login-window.component.css']
})
export class LoginWindowComponent {
  public auth: boolean;

  constructor(private state: StateService, private accountService: AccountService, private router: Router, @Inject('BASE_URL') baseUrl: string) {
     this.auth = false;
  }

  SignIn() {
    const formData: any = new FormData();
    const address = (<HTMLInputElement>document.getElementById('address')).value;
    const password = (<HTMLInputElement>document.getElementById('password')).value;

    (<HTMLInputElement>document.getElementById('address')).value = '';
    (<HTMLInputElement>document.getElementById('password')).value = '';
    
    formData.append('address', address);
    formData.append('password', password);
    this.auth = true;

    this.accountService.SignIn(formData).subscribe(result => {
      let token = result.accessToken;
      localStorage.removeItem('accessToken');
      localStorage.setItem("accessToken", token);

      this.accountService.GetAccount().subscribe(res => {
        localStorage.setItem("id", res.id);
        localStorage.setItem("username", res.username);
        this.state.SendState(true);
      });
    }, error => {
      this.auth = false;
      this.Destroy();
      this.state.SendState(false);
    });
  }

  Destroy(): void {
    localStorage.removeItem("id");
    localStorage.removeItem("accessToken");
    localStorage.removeItem("username");
  }
}
