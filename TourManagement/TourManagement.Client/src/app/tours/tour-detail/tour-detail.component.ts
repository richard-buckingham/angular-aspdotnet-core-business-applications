import { Component, OnInit } from "@angular/core";
import { Tour } from "../shared/tour.model";
import { TourService } from "../shared/tour.service";
import { ActivatedRoute } from "@angular/router";
import { OnDestroy } from "@angular/core/src/metadata/lifecycle_hooks";
import { MasterDataService } from "../../shared/master-data.service";
import { Subscription } from "rxjs/Subscription";
import { Show } from "../shows/shared/show.model";

@Component({
  selector: "app-tour-detail",
  templateUrl: "./tour-detail.component.html",
  styleUrls: ["./tour-detail.component.css"]
})
export class TourDetailComponent implements OnInit, OnDestroy {
  private tour: any;
  private tourId: string;
  private sub: Subscription;
  // fake being an admin
  private isAdmin = true;

  constructor(
    private masterDataService: MasterDataService,
    private tourService: TourService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    // get route data (tourId)
    this.sub = this.route.params.subscribe(params => {
      this.tourId = params["tourId"];

      if (this.isAdmin) {
        console.log("getting tour WITH estimated profits");
        this.tourService
          .getTourWithEstimatedProfits(this.tourId)
          .subscribe(tour => {
            this.tour = tour;
          });
      } else {
        console.log("getting tour without estimated profits");
        this.tourService.getTour(this.tourId).subscribe(tour => {
          this.tour = tour;
        });
      }
    });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
