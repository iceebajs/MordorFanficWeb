import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Composition } from './../../shared/interfaces/composition/composition.interface';
import { CompositionService } from './../../shared/services/composition.service';
import { AccountService } from './../../shared/services/account.service';
import { FormControl, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatAutocompleteSelectedEvent, MatAutocomplete } from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { map, startWith, take } from 'rxjs/operators';
import { CompositionTag } from '../../shared/interfaces/composition-tags/composition-tag.interface';

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
  }

  createComposition() {
    if (this.dataIsValid()) {
      const composition: Composition = {
        title: this.compositionTitle.value,
        previewContext: this.previewContext,
        genre: this.selectedGenre.value,
        userId: this.currentAccountId,
        compositionTags: this.compositionTags
      } as Composition;

      this.compositionService.createComposition(composition)
        .pipe(take(1))
        .subscribe(() => {
          this.isSuccessfull = true;
          setTimeout(() => this.isSuccessfull = false, 3000);
        }, error => {
          this.hasError = true;
          this.errorMessage = error;
          setTimeout(() => this.hasError = false, 3000);
        });
    }
    else {
      this.hasError = true;
      setTimeout(() => this.hasError = false, 3000);
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
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  tagControl = new FormControl();
  filteredTags: Observable<string[]>;
  tags: string[] = [
  ];
  compositionTags: CompositionTag[];
  allTags: string[] = ['fantasy', 'fantastic', 'dungeon'];

  @ViewChild('tagInput') tagInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;

  addTag(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

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
