import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchDualListComponent } from './search-dual-list.component';

describe('SearchDualListComponent', () => {
  let component: SearchDualListComponent;
  let fixture: ComponentFixture<SearchDualListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchDualListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchDualListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
