import {
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpEvent
} from "@angular/common/http";

import { Observable } from "rxjs/Observable";

export class EnsureAcceptHeaderinterceptor implements HttpInterceptor {
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    console.log("request = ", request);

    if (!request.headers.has("Accept")) {
      console.log("addimg my accept header");
      request = request.clone({
        headers: request.headers.set("Accept", "application/json")
      });
    }

    // pass the request onto the next handler
    return next.handle(request);
  }
}
