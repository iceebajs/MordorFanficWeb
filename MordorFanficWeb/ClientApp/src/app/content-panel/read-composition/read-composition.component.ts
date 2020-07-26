import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Composition } from '../../shared/interfaces/composition/composition.interface';
import { CompositionService } from '../../shared/services/composition.service';
import { take } from 'rxjs/operators';
import { Chapter } from '../../shared/interfaces/chapter/chapter.interface';

@Component({
  selector: 'app-read-composition',
  templateUrl: './read-composition.component.html',
  styleUrls: ['./read-composition.component.css'],
  providers: [CompositionService]
})
export class ReadCompositionComponent implements OnInit {

  constructor(private route: ActivatedRoute, private compositionService: CompositionService) { }

  ngOnInit(): void {
    this.compositionId = Number(this.route.snapshot.paramMap.get('id'));
    this.compositionService.getCompositionById(this.compositionId)
      .pipe(take(1))
      .subscribe((response: Composition) => {
        this.currentComposition = response;
        this.dataLoaded = Promise.resolve(true);
        this.currentComposition.chapters.length < 1 ? this.setChapterError() : this.setChapter();
      },
        () => { this.hasError = true; this.errorMessage = 'Composition not found.' });
  }

  hasError: boolean = false;
  errorMessage: string = '';
  dataLoaded: Promise<boolean>;

  compositionId: number;
  currentComposition: Composition;
  currentChapter: Chapter;
  currentChapterIndex: number = 0;
  chapterCount: number;

  nextButtonDisabled: boolean = true;
  nextChapter() {
    this.currentChapterIndex += 1;
    this.currentChapter = this.currentComposition.chapters[this.currentChapterIndex];
    this.setButtonStatus();
  }

  prevButtonDisabled: boolean = true;
  prevChapter() {
    this.currentChapterIndex -= 1;
    this.currentChapter = this.currentComposition.chapters[this.currentChapterIndex];
    this.setButtonStatus();
  }

  private setButtonStatus() {
    this.currentChapterIndex === 0 ? this.prevButtonDisabled = true : this.prevButtonDisabled = false;
    this.currentChapterIndex === this.chapterCount - 1 ? this.nextButtonDisabled = true : this.nextButtonDisabled = false;
  }

  private setChapter() {
    this.currentChapter = this.currentComposition.chapters[0];
    this.chapterCount = this.currentComposition.chapters.length;
    if (this.chapterCount > 1)
      this.nextButtonDisabled = false;
  }

  private setChapterError() {
    this.hasError = true;
    this.errorMessage = 'Author of this composition still not created any chapters.';
  }
}
