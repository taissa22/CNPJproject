import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgendamentoPexModalComponent } from './agendamento-pex-modal.component';

describe('AgendamentoPexModalComponent', () => {
  let component: AgendamentoPexModalComponent;
  let fixture: ComponentFixture<AgendamentoPexModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgendamentoPexModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgendamentoPexModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
