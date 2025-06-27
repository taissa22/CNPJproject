/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EstadosSelecaoComponent } from './estados-selecao.component';

describe('EstadosSelecaoComponent', () => {
  let component: EstadosSelecaoComponent;
  let fixture: ComponentFixture<EstadosSelecaoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EstadosSelecaoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EstadosSelecaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
