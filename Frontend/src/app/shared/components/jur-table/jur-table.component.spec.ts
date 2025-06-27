import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { JurTable } from './jur-table.component';

describe('JurTable', () => {
  let component: JurTable<any>;
  let fixture: ComponentFixture<JurTable<any>>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      schemas: [NO_ERRORS_SCHEMA],
      declarations: [JurTable]
    });
    fixture = TestBed.createComponent(JurTable);
    component = fixture.componentInstance;
  });

  it('can load instance', () => {
    expect(component).toBeTruthy();
  });

  it(`dataSource has default value`, () => {
    expect(component.dataSource).toEqual([]);
  });

  it(`errorMessage has default value`, () => {
    expect(component.errorMessage).toEqual(`Nenhuma informação`);
  });
});
