import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError } from "rxjs/operators";

import { AuthenticationService } from "./authentication.service";

@Injectable({
  providedIn: "root"
})
export class RefreshTokenInterceptorService implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService) { }

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
               .pipe(catchError(error => {
                 return Observable.throw(error);
               }));
  }
}
