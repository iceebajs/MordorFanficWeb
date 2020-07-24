import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Composition } from './../../shared/interfaces/composition/composition.interface';
import { CompositionService } from './../../shared/services/composition.service';
import { AccountService } from './../../shared/services/account.service';
import { FormControl, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { COMMA, ENTER, SPACE } from '@angular/cdk/keycodes';
import { MatAutocompleteSelectedEvent, MatAutocomplete } from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { map, startWith, take } from 'rxjs/operators';
import { CompositionTag } from '../../shared/interfaces/composition-tags/composition-tag.interface';
import { Tag } from '../../shared/interfaces/tags/tag.interface';

@Component({
  selector: 'app-create-composition',
  templateUrl: './create-composition.component.html',
  styleUrls: ['./create-composition.component.css'],
  providers: [CompositionService, AccountService]
})
export class CreateCompositionComponent implements OnInit {

  previewContext: string;

  currentAccountId: number;
  hasError: boolean = false;
  errorMessage: string = '';
  isSuccessfull: boolean = false;

  constructor(private compositionService: CompositionService, private accountService: AccountService) {
    this.filteredTags = this.tagControl.valueChanges.pipe(
      startWith(null),
      map((tag: string | null) => tag ? this._filter(tag) : this.allTags.slice())
    );
  }

  ngOnInit(): void {
    let identity = localStorage.getItem('id');
    this.accountService.getUserAccountId(identity)
      .pipe(take(1))
      .subscribe(response => this.currentAccountId = response);
    this.compositionService.getAllTags()
      .pipe(take(1))
      .subscribe((response: Tag[]) => {
        for (let t of response)
          this.allTags.push(t.tag);
      });
  }

  createComposition() {
    if (this.dataIsValid()) {
      const composition: Composition = this.mapComposition();
      this.mapTags();
      this.tagsArray === [] ? this.addCompositionTags(composition) : this.addTags(composition);
    }
    else {
      this.hasError = true;
      setTimeout(() => this.hasError = false, 3000);
    }
  }

  private addTags(composition: Composition) {
    this.compositionService.addTags(this.tagsArray)
      .pipe(take(1))
      .subscribe(() => this.addCompositionTags(composition));
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
        this.createCompositionStep(composition, tagsId);
      });
  }

  private createCompositionStep(composition: Composition, tagsId) {
    this.compositionService.createComposition(composition)
      .pipe(take(1))
      .subscribe((response: number) => {
        this.mapCompositionTags(response, tagsId);
        this.isSuccessfull = true;
        setTimeout(() => this.isSuccessfull = false, 3000);
      }, error => {
        this.hasError = true;
        this.errorMessage = error;
        setTimeout(() => this.hasError = false, 3000);
      });
  }

  compositionTags: CompositionTag[] = [];
  private mapCompositionTags(compositionId: number, tagsId) {
    this.compositionTags = [];
    for (let t of this.tags) {
      const compTag: CompositionTag = {
        tagId: tagsId[this.allTags.indexOf(t)],
        compositionId: compositionId
      } as CompositionTag;
      this.compositionTags.push(compTag);
    }
    this.compositionService.addCompositionTags(this.compositionTags)
      .pipe(take(1))
      .subscribe();
  }

  mapComposition() {
    const composition: Composition = {
      title: this.compositionTitle.value,
      previewContext: this.previewContext,
      genre: this.selectedGenre.value,
      userId: this.currentAccountId
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

  dataIsValid(): boolean {
    if (this.selectedGenre.valid
      && this.compositionTitle.valid
      && this.tags.length > 0)
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
  selectable = true;
  removable = true;
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
}

interface Genre {
  genre: string;
  viewGenre: string;
}
