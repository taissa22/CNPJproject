import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RelatorioAtmPexComponent } from './relatorio-atm-pex.component';

describe('RelatorioAtmPexComponent', () => {
  let component: RelatorioAtmPexComponent;
  let fixture: ComponentFixture<RelatorioAtmPexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RelatorioAtmPexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RelatorioAtmPexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
