/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AgendamentosPageComponent } from './agendamentos-page.component';

describe('AgendamentosPageComponent', () => {
  let component: AgendamentosPageComponent;
  let fixture: ComponentFixture<AgendamentosPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgendamentosPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgendamentosPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
