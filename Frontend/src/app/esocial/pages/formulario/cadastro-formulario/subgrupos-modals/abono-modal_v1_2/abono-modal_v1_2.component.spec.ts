/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AbonoModal_v1_2Component } from './abono-modal_v1_2.component';

describe('AbonoModal_v1_2Component', () => {
  let component: AbonoModal_v1_2Component;
  let fixture: ComponentFixture<AbonoModal_v1_2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AbonoModal_v1_2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AbonoModal_v1_2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
