import { Component } from "@angular/core";

import { AuthenticationService } from "src/app/services/authentication.service";
import { LoginCredentials } from "src/app/models/login-credentials.model";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"]
})
export class LoginComponent {
  authToken: string;

  model = new LoginCredentials();

  constructor(private authenticationService: AuthenticationService) { }

  onSubmit() {
    this.authenticationService
        .login(this.model)
        .subscribe(response => this.authToken = JSON.stringify(response));
  }
}