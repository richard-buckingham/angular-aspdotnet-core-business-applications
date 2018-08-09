import { Component, OnInit } from "@angular/core";
import {} from "automapper-ts";
import { OpenIdConnectService } from "./security/open-id-connect.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent implements OnInit {
  title = "Pluralsight Demo";

  constructor(private openIdConnectService: OpenIdConnectService) {}

  ngOnInit(): void {
    var path = window.location.pathname;
    if (path != "/signin-oidc") {
      if (!this.openIdConnectService.userAvailable) {
        this.openIdConnectService.triggerSignIn();
      }
    }
  }
}
