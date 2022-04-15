import {Component, Inject, OnInit} from '@angular/core';
import {AccountService} from "../services/account/account.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login-window',
  templateUrl: './login-window.component.html',
  styleUrls: ['./login-window.component.css']
})
export class LoginWindowComponent {
  public auth: boolean;

  constructor(private accountService: AccountService, private router: Router, @Inject('BASE_URL') baseUrl: string) {
     this.auth = true;
  }

  SignIn() {
    const formData: any = new FormData();
    const address = (<HTMLInputElement>document.getElementById('address')).value;
    const password = (<HTMLInputElement>document.getElementById('password')).value;

    formData.append('address', address);
    formData.append('password', password);
    this.auth = true;

    this.accountService.SignIn(formData).subscribe(result => {
      let token = result.accessToken;
      localStorage.setItem("accessToken", token);

      this.accountService.GetAccount().subscribe(res => {
        localStorage.setItem("id", res.id);
        localStorage.setItem("username", res.username);
      });
    }, error => {
      this.auth = false;
    });
  }
}
