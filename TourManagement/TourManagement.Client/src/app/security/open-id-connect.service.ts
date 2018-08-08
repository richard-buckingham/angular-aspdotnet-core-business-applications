import { Injectable } from "@angular/core";
import { UserManager, User } from "oidc-client";
import { environment } from "../../environments/environment";

@Injectable()
export class OpenIdConnectService {
  private userManager: UserManager = new UserManager(
    environment.openIdConnectSettings
  );
  private currentUser: User;

  get userAvailable(): boolean {
    return this.currentUser != null;
  }

  get user(): User {
    return this.currentUser;
  }

  constructor() {
    this.userManager.clearStaleState();

    this.userManager.events.addUserLoaded(user => {
      if (!environment.production) {
        console.log("user was loaded", user);
      }
      this.currentUser = user;
    });
  }

  // create an authentication request to our identity provider
  triggerSignIn() {
    this.userManager.signinRedirect().then(function() {
      if (!environment.production) {
        console.log("re-direction to signin triggered");
      }
    });
  }

  // handle the called back after the user has signed in
  handleCallBack() {
    this.userManager.signinRedirectCallback().then(function(user) {
      if (!environment.production) {
        console.log("callback after signin handled. user = ", user);
      }
    });
  }
}
