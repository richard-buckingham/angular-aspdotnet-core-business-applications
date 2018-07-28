import { Component, OnInit, Input } from "@angular/core";
import { Show } from "./shared/show.model";

@Component({
  selector: "app-shows",
  templateUrl: "./shows.component.html",
  styleUrls: ["./shows.component.css"]
})
export class ShowsComponent {
  @Input() shows: Show[];
}
