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

@Component({
  selector: 'app-manage-composition',
  templateUrl: './manage-composition.component.html',
  styleUrls: ['./manage-composition.component.css'],
  providers: [CompositionService]
})
export class ManageCompositionComponent implements OnInit, OnDestroy {

  subscription: Subscription;
  accountId: number;
  compositionId: number;

  constructor(private activatedRoute: ActivatedRoute,
    private compositionService: CompositionService) {
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

    this.compositionService.getCompositionById(this.compositionId)
      .pipe(take(1))
      .subscribe((response: Composition) => {
        this.currentComposition = response;
        this.setCompositonsEditFields();
        this.setTagsAndTagsForComposition();
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
    this.editComposition === true ? this.editComposition = false : this.setEditCompositionState();
  }
  setEditCompositionState() {
    this.editComposition = true;
    this.createChapter = false;
    this.editChapter = false;
    this.changeChapterNumeration = false;
  }

  createChapter: boolean = false;
  showCreateChapter() {
    this.createChapter === true ? this.createChapter = false : this.setCreateChapterState();
  }
  setCreateChapterState() {
    this.createChapter = true;
    this.editComposition = false;
    this.editChapter = false;
    this.changeChapterNumeration = false;
  }

  editChapter: boolean = false;
  showEditChapter(chapter) {
    this.editChapter === true ? this.editChapter = false : this.setEditChapterState();
    console.log(chapter);
  }
  setEditChapterState() {
    this.editChapter = true;
    this.editComposition = false;
    this.createChapter = false;
    this.changeChapterNumeration = false;
  }

  changeChapterNumeration: boolean = false;
  showChangeNumeration() {
    this.changeChapterNumeration == true ? this.changeChapterNumeration = false : this.setChangeNumeration();
  }
  setChangeNumeration() {
    this.changeChapterNumeration = true;
    this.editChapter = false;
    this.editComposition = false;
    this.createChapter = false;
  }

  movies = [
    'Episode I - The Phantom Menace',
    'Episode II - Attack of the Clones',
    'Episode III - Revenge of the Sith',
    'Episode IV - A New Hope',
    'Episode V - The Empire Strikes Back',
    'Episode VI - Return of the Jedi',
    'Episode VII - The Force Awakens',
    'Episode VIII - The Last Jedi',
    'Episode IX – The Rise of Skywalker'
  ];

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.movies, event.previousIndex, event.currentIndex);
    console.log(this.movies);
  }
}

interface Genre {
  genre: string;
  viewGenre: string;
}
