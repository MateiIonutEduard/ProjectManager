import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {Project, ProjectService} from "../services/project/project.service";

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})
export class ProjectComponent implements OnInit, OnDestroy {
  public fetch: number;
  public projects: Project[] = [];
  private handler: any;

  constructor(private projectService: ProjectService, private router: Router) {
      this.fetch = 1;
      projectService.GetProject().subscribe(result => {
        this.projects = result;
      }, error => this.fetch = -1);

      if(!this.projects.length)
        this.fetch = 0;
  }

  ngOnInit() {
    this.handler = setInterval(() => {
      this.fetch = 1;

      this.projectService.GetProject().subscribe(result => {
        this.projects = result;
      }, error => this.fetch = -1);

      if(!this.projects.length)
        this.fetch = 0;
    }, 1000);
  }

  trackByFn(index: number, item: Project) {
    return item.Id;
  }

  ngOnDestroy() {
    if(this.handler)
      clearInterval(this.handler);
  }
}
