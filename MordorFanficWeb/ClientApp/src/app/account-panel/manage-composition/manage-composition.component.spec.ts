import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageCompositionComponent } from './manage-composition.component';

describe('ManageCompositionComponent', () => {
  let component: ManageCompositionComponent;
  let fixture: ComponentFixture<ManageCompositionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageCompositionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageCompositionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
