import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgendamentoRelatorioMovimentacaoPexModalComponent } from './agendamento-relatorio-movimentacao-pex-modal.component';

describe('AgendamentoRelatorioMovimentacaoPexModalComponent', () => {
  let component: AgendamentoRelatorioMovimentacaoPexModalComponent;
  let fixture: ComponentFixture<AgendamentoRelatorioMovimentacaoPexModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgendamentoRelatorioMovimentacaoPexModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgendamentoRelatorioMovimentacaoPexModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
