/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AgendaAudienciaTrabalhistaComponent } from './agenda-audiencia-trabalhista.component';

describe('AgendaAudienciaTrabalhistaComponent', () => {
  let component: AgendaAudienciaTrabalhistaComponent;
  let fixture: ComponentFixture<AgendaAudienciaTrabalhistaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgendaAudienciaTrabalhistaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgendaAudienciaTrabalhistaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
