/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AgendamentoVepModalComponent } from './agendamento-vep-modal.component';

describe('AgendamentoVepModalComponent', () => {
  let component: AgendamentoVepModalComponent;
  let fixture: ComponentFixture<AgendamentoVepModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgendamentoVepModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgendamentoVepModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
