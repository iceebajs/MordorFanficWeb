import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from './../../shared/services/authorization.service';
import { take } from 'rxjs/operators';
import { CompositionService } from './../../shared/services/composition.service';
import { Tag } from '../../shared/interfaces/tags/tag.interface';
import { Composition } from '../../shared/interfaces/composition/composition.interface';
import { Rating } from '../../shared/interfaces/composition/rating.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  providers: [CompositionService]
})
export class HomeComponent implements OnInit {

  constructor(private authService: AuthorizationService,
    private compositionService: CompositionService,
    private router: Router) { }

  ngOnInit(): void {
    if (this.authService.isSignedIn())
      this.authService.tokenExpirationCheck().pipe(take(1)).subscribe();

    this.compositionService.getAllTags()
      .pipe(take(1))
      .subscribe((response: Tag[]) => {
        this.tagsList = response;
        this.tagsLoaded = Promise.resolve(true);
      });

    this.compositionService.getLastCompositions()
      .pipe(take(1))
      .subscribe((response: Composition[]) => {
        this.compositionsList = response;
        this.compositionsLoaded = Promise.resolve(true);
        this.calculateRatings();
      });
  }

  tagsList: Tag[] = [];
  tagsLoaded: Promise<boolean>;

  searchByTag(index) {
    index = index + 1;
    console.log(index);
  }

  compositionsList: Composition[] = [];
  compositionsLoaded: Promise<boolean>;

  compositionRating: number[] = [];

  calculateRatings() {
    for (let composition of this.compositionsList) {
      this.compositionRating.push(calcRating(composition.compositionRatings));
    }


    function calcRating(ratings: Rating[]) {
      let summary = 0;
      if (ratings.length > 0) {
        ratings.forEach(rating => {
          summary = summary + rating.rating;
        });
        return (summary / ratings.length);
      }
      else
        return summary;
    }
  }

  readComposition(id: number) {
    this.router.navigate(['read', id]);
  }
}
