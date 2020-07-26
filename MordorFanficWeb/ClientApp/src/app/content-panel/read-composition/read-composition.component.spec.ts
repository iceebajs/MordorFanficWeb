import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadCompositionComponent } from './read-composition.component';

describe('ReadCompositionComponent', () => {
  let component: ReadCompositionComponent;
  let fixture: ComponentFixture<ReadCompositionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReadCompositionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadCompositionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
