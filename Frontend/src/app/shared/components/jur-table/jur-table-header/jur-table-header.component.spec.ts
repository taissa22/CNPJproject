import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { JurTableHeader } from './jur-table-header.component';

describe('JurTableHeader', () => {
  let component: JurTableHeader;
  let fixture: ComponentFixture<JurTableHeader>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      schemas: [NO_ERRORS_SCHEMA],
      declarations: [JurTableHeader]
    });
    fixture = TestBed.createComponent(JurTableHeader);
    component = fixture.componentInstance;
  });

  it('can load instance', () => {
    expect(component).toBeTruthy();
  });

  it(`columns has default value`, () => {
    expect(component.columns).toEqual([]);
  });
});
