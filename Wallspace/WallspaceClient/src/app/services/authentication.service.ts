import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Observable } from "rxjs";

import { LoginCredentials } from "../models/login-credentials.model";

@Injectable({
  providedIn: "root"
})
export class AuthenticationService {
  constructor(private httpClient: HttpClient) { }

  login(credentials: LoginCredentials): Observable<string> {
    return this.httpClient.post<string>("/api/account/login", credentials);
  }
}