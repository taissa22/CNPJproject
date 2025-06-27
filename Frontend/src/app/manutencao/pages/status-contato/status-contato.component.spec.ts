/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { StatusContatoComponent } from './status-contato.component';

describe('StatusContatoComponent', () => {
  let component: StatusContatoComponent;
  let fixture: ComponentFixture<StatusContatoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StatusContatoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StatusContatoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
