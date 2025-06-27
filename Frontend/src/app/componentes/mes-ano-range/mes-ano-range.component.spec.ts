/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { MesAnoRangeComponent } from './mes-ano-range.component';

describe('MesAnoRangeComponent', () => {
  let component: MesAnoRangeComponent;
  let fixture: ComponentFixture<MesAnoRangeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MesAnoRangeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MesAnoRangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
