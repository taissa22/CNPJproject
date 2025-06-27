import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PesquisaHeaderComponent } from './pesquisa-header.component';

describe('PesquisaHeaderComponent', () => {
  let component: PesquisaHeaderComponent;
  let fixture: ComponentFixture<PesquisaHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PesquisaHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PesquisaHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
