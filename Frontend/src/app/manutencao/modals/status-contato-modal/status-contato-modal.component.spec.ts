/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { StatusContatoModalComponent } from './status-contato-modal.component';

describe('StatusContatoModalComponent', () => {
  let component: StatusContatoModalComponent;
  let fixture: ComponentFixture<StatusContatoModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StatusContatoModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StatusContatoModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
