import { Component, OnInit, ErrorHandler } from "@angular/core";

import { Tour } from "./shared/models/tour.model";
import { TourService } from "./shared/tour.service";

@Component({
  selector: "app-tours",
  templateUrl: "./tours.component.html",
  styleUrls: ["./tours.component.css"]
})
export class ToursComponent implements OnInit {
  title: string = "Tour overview";
  tours: Tour[] = [];

  constructor(private tourService: TourService) {}

  ngOnInit() {
    console.log("getting tours...");
    this.tourService.getTours().subscribe(tours => {
      this.tours = tours;
    });
  }
}
