import { Injectable, ErrorHandler } from "@angular/core";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/observable/throw";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/do";

import { Tour } from "./models/tour.model";
import { BaseService } from "../../shared/base.service";
import { TourWithEstimatedProfits } from "./models/tour-with-estimated-profits.model";

@Injectable()
export class TourService extends BaseService {
  constructor(private http: HttpClient) {
    super();
  }

  getTours(): Observable<Tour[]> {
    return this.http.get<Tour[]>(`${this.apiUrl}/tours`);
  }

  getTour(tourId: string): Observable<Tour> {
    return this.http.get<Tour>(`${this.apiUrl}/tours/${tourId}`, {
      headers: { Accept: "application/vnd.marvin.tour+json" }
    });
  }

  getTourWithEstimatedProfits(
    tourId: string
  ): Observable<TourWithEstimatedProfits> {
    return this.http.get<TourWithEstimatedProfits>(
      `${this.apiUrl}/tours/${tourId}`,
      {
        headers: {
          Accept: "application/vnd.marvin.tourwithestimatedprofits+json"
        }
      }
    );
  }
}
