import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { OpenIdConnectService } from "../open-id-connect.service";

@Component({
  selector: "app-signin-oidc",
  templateUrl: "./signin-oidc.component.html",
  styleUrls: ["./signin-oidc.component.css"]
})
export class SigninOidcComponent implements OnInit {
  constructor(
    private router: Router,
    private openIdConnectService: OpenIdConnectService
  ) {}

  ngOnInit() {
    this.openIdConnectService.handleCallBack();
    this.router.navigate(["./"]);
  }
}
