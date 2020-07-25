import { Component, OnInit, OnDestroy, ElementRef, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Composition } from './../../shared/interfaces/composition/composition.interface';
import { CompositionService } from './../../shared/services/composition.service';
import { FormControl, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { COMMA, ENTER, SPACE } from '@angular/cdk/keycodes';
import { MatAutocompleteSelectedEvent, MatAutocomplete } from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { map, startWith, take } from 'rxjs/operators';
import { CompositionTag } from '../../shared/interfaces/composition-tags/composition-tag.interface';
import { Tag } from '../../shared/interfaces/tags/tag.interface';
import { ChapterService } from './../../shared/services/chapter.service';
import { Chapter } from '../../shared/interfaces/chapter/chapter.interface';
import { ChapterNumeration } from '../../shared/interfaces/chapter/chapter-numeration.interface';

@Component({
  selector: 'app-manage-composition',
  templateUrl: './manage-composition.component.html',
  styleUrls: ['./manage-composition.component.css'],
  providers: [CompositionService, ChapterService]
})
export class ManageCompositionComponent implements OnInit, OnDestroy {

  subscription: Subscription;
  accountId: number;
  compositionId: number;
  submitButtonStatus = false;
  onInitSort: boolean = true;

  constructor(private activatedRoute: ActivatedRoute,
    private compositionService: CompositionService, private chapterService: ChapterService) {
    this.filteredTags = this.tagControl.valueChanges.pipe(
      startWith(null),
      map((tag: string | null) => tag ? this._filter(tag) : this.allTags.slice())
    );
  }

  
  ngOnInit(): void {
    this.subscription = this.activatedRoute.queryParams
      .pipe(take(1))
      .subscribe(params => {
        this.currentAccountId = Number(params['uId']);
        this.compositionId = Number(params['id']);
      });

    this.getCompositionForCurrentId();
  }

  getCompositionForCurrentId() {
    this.compositionService.getCompositionById(this.compositionId)
      .pipe(take(1))
      .subscribe((response: Composition) => {
        this.currentComposition = response;
        this.setCompositonsEditFields();
        this.setTagsAndTagsForComposition();
        this.onInitSort = true;
      });
  }

  currentCompositionTagsId: number[] = [];

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  oldTags: string[] = [];
  setTagsAndTagsForComposition() {
    this.compositionService.getAllTags()
      .pipe(take(1))
      .subscribe((response: Tag[]) => {
        for (let t of response) {
          this.allTags.push(t.tag);
          if (this.currentCompositionTagsId.includes(t.id)) {
            this.tags.push(t.tag);
            this.oldTags.push(t.tag);
          }
        }
      });
  }

  setCompositonsEditFields() {
    this.previewContext = this.currentComposition.previewContext;
    this.compositionTitle.setValue(this.currentComposition.title);
    for (let t of this.currentComposition.compositionTags)
      this.currentCompositionTagsId.push(t.tagId);
    this.selectedGenre.setValue(this.currentComposition.genre);
  }

  currentComposition: Composition;

  previewContext: string;
  currentAccountId: number;
  hasError: boolean = false;
  errorMessage: string = '';
  isSuccessfull: boolean = false;

  dataIsValid(): boolean {
    if (this.selectedGenre.valid
      && this.compositionTitle.valid
      && this.tags.length > 0
      && this.previewContext.length > 0)
      return true;
    return false;
  }

  selectedGenre = new FormControl('', [Validators.required]);
  errorSelectedGenre() {
    if (this.compositionTitle.hasError('required'))
      return 'You must enter a value';
  }

  compositionTitle = new FormControl('', [Validators.required]);
  errorCompositionTitle() {
    if (this.compositionTitle.hasError('required'))
      return 'You must enter a value';
  }

  visible = true;
  selectable = false;
  removable = false;
  readonly separatorKeysCodes: number[] = [ENTER, COMMA, SPACE];
  tagControl = new FormControl();
  filteredTags: Observable<string[]>;
  tags: string[] = [];
  allTags: string[] = [];

  @ViewChild('tagInput') tagInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;

  addTag(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    if (this.tags.indexOf(value) === -1)
      if ((value || '').trim()) {
        this.tags.push(value.trim());
      }

    if (input) {
      input.value = '';
    }
    this.tagControl.setValue(null);
  }

  removeTag(tag: string): void {
    const index = this.tags.indexOf(tag);

    if (index >= 0) {
      this.tags.splice(index, 1);
    }
  }

  selectedTags(event: MatAutocompleteSelectedEvent): void {
    if (this.tags.indexOf(event.option.viewValue) === -1)
      this.tags.push(event.option.viewValue);
    this.tagInput.nativeElement.value = '';
    this.tagControl.setValue(null);
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.allTags.filter(tag => tag.toLowerCase().indexOf(filterValue) === 0);
  }

  genres: Genre[] = [
    { genre: 'action', viewGenre: 'Action' },
    { genre: 'adventure', viewGenre: 'Adventure' },
    { genre: 'comedy', viewGenre: 'Comedy' },
    { genre: 'drama', viewGenre: 'Drama' },
    { genre: 'fantasy', viewGenre: 'Fantasy' },
    { genre: 'magic', viewGenre: 'Magic' },
    { genre: 'horror', viewGenre: 'Horror' },
    { genre: 'mystery', viewGenre: 'Mystery' },
    { genre: 'psychological', viewGenre: 'Psychological' },
    { genre: 'romance', viewGenre: 'Romance' }
  ];

  updateComposition() {
    if (this.dataIsValid()) {
      const composition = this.mapComposition();
      this.tagsArray === [] ? this.addCompositionTags(composition) : this.addTags(composition);
    }
    else {
      this.hasError = true;
      setTimeout(() => this.hasError = false, 3000);
    }
  }

  private addTags(composition: Composition) {
    this.mapTags();
    this.tagsArray.length > 0 ?
      this.compositionService.addTags(this.tagsArray)
        .pipe(take(1))
        .subscribe(() => this.addCompositionTags(composition)) : this.addCompositionTags(composition);
  }

  private addCompositionTags(composition: Composition) {
    this.allTags = [];
    let tagsId = [];
    this.compositionService.getAllTags()
      .pipe(take(1))
      .subscribe((response: Tag[]) => {
        for (let t of response) {
          tagsId.push(t.id);
          this.allTags.push(t.tag);
        }
        this.updateCompositionStep(composition, tagsId);
      });
  }

  updateCompositionStep(composition, tagsId) {
    this.compositionService.updateComposition(composition)
      .pipe(take(1))
      .subscribe(() => {
        this.mapCompositionTags(tagsId);
        this.isSuccessfull = true;
        setTimeout(() => this.isSuccessfull = false, 3000);
      }, error => {
        this.hasError = true;
        this.errorMessage = error;
        setTimeout(() => this.hasError = false, 3000);
      });
  }

  compositionTags: CompositionTag[] = [];
  private mapCompositionTags(tagsId) {
    this.compositionTags = [];
    let filteredTags = this.deleteExistingTags(this.tags);

    for (let t of filteredTags) {
      const compTag: CompositionTag = {
        tagId: tagsId[this.allTags.indexOf(t)],
        compositionId: this.compositionId
      } as CompositionTag;
      this.compositionTags.push(compTag);
    }
    if (this.compositionTags.length > 0)
      this.compositionService.addCompositionTags(this.compositionTags)
        .pipe(take(1))
        .subscribe();
  }

  deleteExistingTags(tags: string[]) {
    let tagsCopy = tags.slice();
    let indexes = [];
    tagsCopy.forEach((t, i) => {
      if (this.oldTags.indexOf(t) !== -1)
        indexes.push(i);
    });
    indexes.reverse().forEach(i => tagsCopy.splice(i, 1));
    return tagsCopy;
  }

  mapComposition() {
    const composition: Composition = {
      compositionId: this.currentComposition.compositionId,
      title: this.compositionTitle.value,
      previewContext: this.previewContext,
      genre: this.selectedGenre.value,
      userId: this.currentComposition.userId
    } as Composition;

    return composition;
  }

  tagsArray: Tag[] = [];
  mapTags() {
    this.tagsArray = [];
    for (let t of this.tags) {
      if (this.allTags.indexOf(t) === -1) {
        const tag: Tag = { tag: t } as Tag;
        this.tagsArray.push(tag);
      }
    }
  }

  editComposition: boolean = true;
  showEditComposition() {
    this.setEditCompositionState();
  }
  setEditCompositionState() {
    this.editComposition = true;
    this.createChapter = false;
    this.editChapter = false;
    this.changeChapterNumeration = false;
    this.editChapterButton = false;
  }

  createChapter: boolean = false;
  showCreateChapter() {
    this.setCreateChapterState();
  }
  setCreateChapterState() {
    this.createChapter = true;
    this.editComposition = false;
    this.editChapter = false;
    this.changeChapterNumeration = false;
    this.editChapterButton = false;
  }

  editChapter: boolean = false;
  editChapterButton: boolean = false;
  showEditChapter(chapter: Chapter) {
    this.setEditChapterState();
  }

  setEditChapterState() {
    this.editChapter = true;
    this.editChapterButton = true;
    this.editComposition = false;
    this.createChapter = false;
    this.changeChapterNumeration = false;
  }

  deleteChapter(chapter: Chapter) {
    this.chapterService.deleteChapter(chapter.chapterId)
      .pipe(take(1))
      .subscribe(() => {
        let i = this.chaptersArray.indexOf(chapter);
        this.chaptersArray.splice(i, 1);
        this.updateNumerationAfterDelete();
      });
  }

  changeChapterNumeration: boolean = false;
  showChangeNumeration() {
    this.setChangeNumeration();
    if (this.onInitSort) {
      this.chaptersArray = this.currentComposition.chapters;
      this.chaptersArray.sort(function (a, b) {
        if (a.chapterNumber > b.chapterNumber)
          return 1;
        if (a.chapterNumber < b.chapterNumber)
          return -1;
        return 0;
      });
      this.onInitSort = false;
    }

  }
  setChangeNumeration() {
    this.changeChapterNumeration = true;
    this.editChapterButton = true;
    this.editChapter = false;
    this.editComposition = false;
    this.createChapter = false;
  }

  // Chapter block
  chapterTitle = new FormControl('', [Validators.required]);
  errorChapterTitle() {
    if (this.chapterTitle.hasError('required'))
      return 'You must enter a value';
  }

  chapterContext: string = '';

  addChapterToDb() {
    if (this.isChapterDataValid()) {
      let addChapterNumber = this.currentComposition.chapters.length + 1;
      this.submitButtonStatus = true;

      const chapterToAdd: Chapter = {
        chapterNumber: addChapterNumber,
        chapterTitle: this.chapterTitle.value,
        context: this.chapterContext,
        imgSource: this.files[0],
        compositionId: this.compositionId
      } as Chapter;
      this.chapterService.createChapter(chapterToAdd).
        pipe(take(1))
        .subscribe(() => {
          this.isSuccessfull = true;
          this.getCompositionForCurrentId();
          setTimeout(() => {
            this.isSuccessfull = false;
            this.submitButtonStatus = false;
          }, 3000);
        }, error => {
            this.hasError = true;
            this.errorMessage = error;
            setTimeout(() => {
              this.hasError = false;
              this.submitButtonStatus = false;
            }, 3000);
        });
    }
  }

  isChapterDataValid() {
    if (this.chapterContext.length > 0
      && this.chapterTitle.valid)
      return true;
    return false;
  }

  //Chapter sequence
  chaptersArray: Chapter[] = [
  ];

  chaptersNumeration: ChapterNumeration[] = [];

  drop(event: CdkDragDrop<string[]>) {
    this.chaptersNumeration.length = 0;
    moveItemInArray(this.chaptersArray, event.previousIndex, event.currentIndex);
    this.updateNumeration();
  }

  private updateNumeration() {
    this.chaptersArray.forEach((item, index) => {
      const chaptNumObj: ChapterNumeration = {
        currentNumber: index + 1,
        chapterId: item.chapterId,
        compositionId: this.compositionId
      } as ChapterNumeration;
      this.chaptersNumeration.push(chaptNumObj);
    });
    this.chapterService.updateNumeration(this.chaptersNumeration)
      .pipe(take(1))
      .subscribe();
  }

  private updateNumerationAfterDelete() {
    this.chaptersNumeration.length = 0;
    moveItemInArray(this.chaptersArray, 1, 1);
    this.chaptersArray.forEach((item, index) => {
      const chaptNumObj: ChapterNumeration = {
        currentNumber: index + 1,
        chapterId: item.chapterId,
        compositionId: this.compositionId
      } as ChapterNumeration;
      this.chaptersNumeration.push(chaptNumObj);
    });
    this.chapterService.updateNumeration(this.chaptersNumeration)
      .pipe(take(1))
      .subscribe();
  }
  //Update chapter

  chapterTitleEdit = new FormControl('', [Validators.required]);
  errorChapterTitleEdit() {
    if (this.chapterTitleEdit.hasError('required'))
      return 'You must enter a value';
  }

  chapterContextEdit: string = '';
  submitUpdateChapter() {

  }

  //dragNdrop
  files: any = [];

  uploadFile(event) {
    for (let index = 0; index < event.length; index++) {
      const element = event[index];
      if (this.files.length > 0)
        this.deleteAttachment(0);
      this.files.push(element.name)
    }
  }

  deleteAttachment(index) {
    this.files.splice(index, 1)
  }

  filesEdit: any = [];

  uploadFileEdit(event) {
    for (let index = 0; index < event.length; index++) {
      const element = event[index];
      if (this.files.length > 0)
        this.deleteAttachment(0);
      this.files.push(element.name)
    }
  }

  deleteAttachmentEdit(index) {
    this.files.splice(index, 1)
  }
}

interface Genre {
  genre: string;
  viewGenre: string;
}
