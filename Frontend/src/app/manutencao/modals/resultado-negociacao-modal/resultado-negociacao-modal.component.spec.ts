/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ResultadoNegociacaoModalComponent } from './resultado-negociacao-modal.component';

describe('ResultadoNegociacaoModalComponent', () => {
  let component: ResultadoNegociacaoModalComponent;
  let fixture: ComponentFixture<ResultadoNegociacaoModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResultadoNegociacaoModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResultadoNegociacaoModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
