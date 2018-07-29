import { Component, OnInit, OnDestroy } from "@angular/core";
import { Band } from "../../shared/band.model";
import { Manager } from "../../shared/manager.model";
import { FormGroup, FormBuilder, FormArray } from "@angular/forms";
import { Subscription } from "rxjs/Subscription";
import { MasterDataService } from "../../shared/master-data.service";
import { TourService } from "../shared/tour.service";
import { Router } from "@angular/router";
import { ShowSingleComponent } from "../shows/show-single/show-single.component";

@Component({
  selector: "app-tour-add",
  templateUrl: "./tour-add.component.html",
  styleUrls: ["./tour-add.component.css"]
})
export class TourAddComponent implements OnInit, OnDestroy {
  public tourForm: FormGroup;
  bands: Band[];
  managers: Manager[];
  public isAdmin: boolean = false;

  constructor(
    private masterDataService: MasterDataService,
    private tourService: TourService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit() {
    // define the tourForm (with empty default values)
    this.tourForm = this.formBuilder.group({
      band: [""],
      manager: [""],
      title: [""],
      description: [""],
      startDate: [],
      endDate: [],
      shows: this.formBuilder.array([])
    });

    // get bands from master data service
    this.masterDataService.getBands().subscribe(bands => {
      console.log("bands = ", bands);
      this.bands = bands;
    });

    if (this.isAdmin === true) {
      // get managers from master data service
      this.masterDataService.getManagers().subscribe(managers => {
        console.log("managers = ", managers);
        this.managers = managers;
      });
    }
  }

  addTour(): void {
    if (this.tourForm.dirty && this.tourForm.valid) {
      if (this.isAdmin === true) {
        // create TourWithManagerForCreation from the Form model
        let tour = automapper.map(
          "TourFormModel",
          "TourWithManagerForCreation",
          this.tourForm.value
        );

        console.log("TourFormModel = ", this.tourForm.value);
        console.log("TourWithManagerForCreation = ", tour);

        // post to service
        this.tourService.addTourWithManager(tour).subscribe(() => {
          this.router.navigateByUrl("/tours");
        });
      } else {
        // create TourForCreation from the Form model
        let tour = automapper.map(
          "TourFormModel",
          "TourForCreation",
          this.tourForm.value
        );

        console.log("TourFormModel = ", this.tourForm.value);
        console.log("TourCreation = ", tour);

        // post to service
        this.tourService.addTour(tour).subscribe(() => {
          this.router.navigateByUrl("/tours");
        });
      }
    }
  }

  addShow(): void {
    console.log("in addShow()");
    let showsFormsArray = this.tourForm.get("shows") as FormArray;
    // add the show
    showsFormsArray.push(ShowSingleComponent.createShow());
  }

  ngOnDestroy() {}
}
