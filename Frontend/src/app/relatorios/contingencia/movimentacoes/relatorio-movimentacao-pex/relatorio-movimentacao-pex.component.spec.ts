import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RelatorioMovimentacaoPexComponent } from './relatorio-movimentacao-pex.component';

describe('RelatorioMovimentacaoPexComponent', () => {
  let component: RelatorioMovimentacaoPexComponent;
  let fixture: ComponentFixture<RelatorioMovimentacaoPexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RelatorioMovimentacaoPexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RelatorioMovimentacaoPexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
