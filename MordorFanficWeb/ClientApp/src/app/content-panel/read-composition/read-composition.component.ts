import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Composition } from '../../shared/interfaces/composition/composition.interface';
import { CompositionService } from '../../shared/services/composition.service';
import { take } from 'rxjs/operators';
import { Chapter } from '../../shared/interfaces/chapter/chapter.interface';
import { Tag } from '../../shared/interfaces/tags/tag.interface';
import { Rating } from '../../shared/interfaces/composition/rating.interface';

@Component({
  selector: 'app-read-composition',
  templateUrl: './read-composition.component.html',
  styleUrls: ['./read-composition.component.css'],
  providers: [CompositionService]
})
export class ReadCompositionComponent implements OnInit {

  constructor(private route: ActivatedRoute, private compositionService: CompositionService) { }

  ngOnInit(): void {
    this.prevPage = this.page;
    this.compositionId = Number(this.route.snapshot.paramMap.get('id'));
    this.compositionService.getAllTags()
      .pipe(take(1))
      .subscribe((response: Tag[]) => { this.tagsList = response; this.getComposition(); });

  }
  getComposition() {
    this.compositionService.getCompositionById(this.compositionId)
      .pipe(take(1))
      .subscribe((response: Composition) => {
        this.currentComposition = response;
        this.dataLoaded = Promise.resolve(true);
        this.calculateRatings(response.compositionRatings);
        this.currentComposition.chapters.length < 1 ? this.setChapterError() : this.setChapter();
      },
        () => { this.hasError = true; this.errorMessage = 'Composition not found.' });
  }

  hasError: boolean = false;
  errorMessage: string = '';
  dataLoaded: Promise<boolean>;
  page: number = 1;
  prevPage: number;

  compositionId: number;
  currentComposition: Composition;
  currentChapter: Chapter;
  chapterCount: number;
  tagsList: Tag[];
  compositionRating: number = 0;
  sortedChapters: Chapter[] = [];

  private sotrChaptersByNumeration() {
    this.currentComposition.chapters.sort(function (a, b) {
      return a.chapterNumber - b.chapterNumber;
    });
  }

  private setChapter() {
    this.sotrChaptersByNumeration();
    this.currentChapter = this.currentComposition.chapters[0];
    this.chapterCount = this.currentComposition.chapters.length;
  }

  private setChapterError() {
    this.hasError = true;
    this.errorMessage = 'Author of this composition still not created any chapters.';
  }

  isRead: boolean = false;
  readComposition() {
    this.isRead = true;
  }

  searchByTag(index) {
    index = index + 1;
    console.log(index);
  }

  calculateRatings(ratings: Rating[]) {
    let summary = 0;
    if (ratings.length > 0) {
      ratings.forEach(rating => {
        summary = summary + rating.rating;
      });
      this.compositionRating = (summary / ratings.length);
    }
    else
      this.compositionRating = summary;
  }

  switchChapter(p: number) {
    this.currentChapter = this.currentComposition.chapters[p - 1];
  }
}
