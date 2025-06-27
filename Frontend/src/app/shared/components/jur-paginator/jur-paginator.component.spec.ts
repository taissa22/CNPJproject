import { ComponentFixture, TestBed } from '@angular/core/testing';
import { fakeAsync, tick } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { JurPaginator } from './jur-paginator.component';
import { PageEvent } from './page-event';

function randomPositiveNumber(notThis: number = 0): number {
  const result: number = Math.ceil(Math.random() * 10);
  if (result === notThis) {
    return randomPositiveNumber(notThis);
  }
  return result;
}

describe('JurPaginator', () => {
  let component: JurPaginator;
  let fixture: ComponentFixture<JurPaginator>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule],
      schemas: [NO_ERRORS_SCHEMA],
      declarations: [JurPaginator]
    });
    fixture = TestBed.createComponent(JurPaginator);
    component = fixture.componentInstance;
  });

  describe('properties', () => {
    beforeEach(() => {
      component.ngOnInit();
      component.length = 141;
    });

    describe('pageSize', () => {
      it(`should change 'pageIndex' to '0' when changed`, () => {
        component.pageIndex = 1;
        component.pageSize = 10;
        expect(component.pageIndex).toBe(0);
      });
      it(`should NOT change 'pageIndex' when changed to same value`, () => {
        const pageIndex: number = randomPositiveNumber();
        const pageSize: number = randomPositiveNumber(8);

        component.pageIndex = pageIndex;
        component.pageSize = pageSize;
        expect(component.pageIndex).toBe(0);

        component.pageIndex = pageIndex;
        component.pageSize = pageSize;
        expect(component.pageIndex).toBe(pageIndex);
      });

      it(`should raise 'page' event when changed`, fakeAsync(() => {
        let pageEvent: PageEvent;
        component.page.subscribe(event => (pageEvent = event));
        component.pageSize = 10;
        tick();
        expect(pageEvent).toBeTruthy();
      }));
      it(`should NOT raise 'page' event when changed to same value`, fakeAsync(() => {
        let eventCount: number = 0;
        component.page.subscribe(_ => eventCount++);
        component.pageSize = 10;
        tick();
        component.pageSize = 10;
        tick();
        expect(eventCount).toBe(1);
      }));
    });

    describe('length', () => {
      it(`should change 'pageIndex' to '0' when changed`, () => {
        component.pageIndex = randomPositiveNumber();
        component.length = randomPositiveNumber();
        expect(component.pageIndex).toBe(0);
      });
      it(`should NOT change 'pageIndex' when changed to same value`, () => {
        const length: number = randomPositiveNumber();
        component.pageIndex = randomPositiveNumber();
        component.length = length;
        expect(component.pageIndex).toBe(0);

        component.pageIndex = randomPositiveNumber();
        component.length = length;
        expect(component.pageIndex).toBeGreaterThan(0);
      });
    });

    describe('pageSizeOptions', () => {
      it(`should change 'pageIndex' to '0' when changed`, () => {
        component.pageIndex = randomPositiveNumber();
        component.pageSizeOptions = [5, 10, 15];
        expect(component.pageIndex).toBe(0);
      });
      it(`should NOT change 'pageIndex' when changed to same value`, () => {
        const pageSizeOptions: Array<number> = [5, 10, 15];
        component.pageIndex = randomPositiveNumber();
        component.pageSizeOptions = pageSizeOptions;
        expect(component.pageIndex).toBe(0);

        component.pageIndex = randomPositiveNumber();
        component.pageSizeOptions = pageSizeOptions;
        expect(component.pageIndex).toBeGreaterThan(0);
      });

      it(`should change 'pageSize' to 'pageSizeOptions[0]' when changed`, () => {
        component.pageSize = randomPositiveNumber();
        component.pageSizeOptions = [5, 10, 15];
        expect(component.pageSize).toBe(5);
      });
      it(`should NOT change 'pageSize' when changed to same value`, () => {
        const pageSizeOptions: Array<number> = [5, 10, 15];
        component.pageSize = randomPositiveNumber();
        component.pageSizeOptions = pageSizeOptions;
        expect(component.pageSize).toBe(pageSizeOptions[0]);

        component.pageSize = pageSizeOptions[2];
        component.pageSizeOptions = pageSizeOptions;
        expect(component.pageSize).not.toBe(pageSizeOptions[0]);
      });
    });

    describe('pageIndex', () => {
      it(`should raise 'page' event when changed`, fakeAsync(() => {
        let pageEvent: PageEvent;
        component.page.subscribe(event => (pageEvent = event));
        component.pageIndex = 1;
        tick();
        expect(pageEvent).toBeTruthy();
      }));
      it(`should NOT raise 'page' event when changed to same value`, fakeAsync(() => {
        let eventCount: number = 0;
        component.page.subscribe(_ => eventCount++);
        component.pageIndex = 1;
        tick();
        component.pageIndex = 1;
        tick();
        expect(eventCount).toBe(1);
      }));
    });
  });

  describe('events', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    describe('page', () => {
      it(`should emit when 'pageIndex' or 'pageSize' change`, fakeAsync(() => {
        let eventCount: number = 0;
        component.page.subscribe(_ => eventCount++);
        component.pageIndex++;
        component.pageSize++;
        tick();
        expect(eventCount).toBe(2);
      }));
      it(`should have value matching to properties`, fakeAsync(() => {
        let pageEvent: PageEvent;
        component.page.subscribe(event => (pageEvent = event));
        component.pageIndex++;
        tick();
        expect(pageEvent).toBeTruthy();
        expect(pageEvent.pageIndex).toBe(component.pageIndex);
        expect(pageEvent.length).toBe(component.length);
        expect(pageEvent.pageSize).toBe(component.pageSize);
      }));
    });
  });

  describe('functions', () => {
    describe('nextPage', () => {
      it(`should increase 'pageIndex' by one when 'hasNextPage' equals to 'true'`, () => {
        spyOn(component, 'hasNextPage').and.returnValue(true);
        const pageIndex: number = randomPositiveNumber();
        component.pageIndex = pageIndex;
        component.nextPage();
        expect(component.pageIndex).toBe(pageIndex + 1);
      });

      it(`should do nothing when 'hasNextPage' equals to 'false'`, () => {
        spyOn(component, 'hasNextPage').and.returnValue(false);
        const pageIndex: number = randomPositiveNumber();
        component.pageIndex = pageIndex;
        component.hasNextPage();
        expect(component.pageIndex).toBe(pageIndex);
      });
    });

    describe('previousPage', () => {
      it(`should decrease 'pageIndex' by one when 'hasPreviousPage' equals to 'true'`, () => {
        spyOn(component, 'hasPreviousPage').and.returnValue(true);
        const pageIndex: number = randomPositiveNumber();
        component.pageIndex = pageIndex;
        component.previousPage();
        expect(component.pageIndex).toBe(pageIndex - 1);
      });

      it(`should do nothing when 'hasPreviousPage' equals to 'false'`, () => {
        spyOn(component, 'hasPreviousPage').and.returnValue(false);
        const pageIndex: number = randomPositiveNumber();
        component.pageIndex = pageIndex;
        component.previousPage();
        expect(component.pageIndex).toBe(pageIndex);
      });
    });

    describe('firstPage', () => {
      it(`should change 'pageIndex' to '0'`, () => {
        component.pageIndex = randomPositiveNumber();
        component.firstPage();
        expect(component.pageIndex).toBe(0);
      });
    });

    describe('lastPage', () => {
      it(`should define 'pageIndex' to the index of the last page`, () => {
        const numberOfPages: number = randomPositiveNumber() + 1;
        spyOn(component, 'getNumberOfPages').and.returnValue(numberOfPages);
        component.pageIndex = 0;
        component.lastPage();
        expect(component.pageIndex).toBe(numberOfPages - 1);
      });
    });

    describe('hasPreviousPage', () => {
      it(`should have a previous page when 'pageIndex' is greater than '0'`, () => {
        component.pageIndex = randomPositiveNumber() + 1;
        expect(component.hasPreviousPage()).toBeTruthy();
      });

      it(`should NOT have a previous page when 'pageIndex' is equals to '0'`, () => {
        component.pageIndex = 0;
        expect(component.hasPreviousPage()).toBeFalsy();
      });
    });

    describe('hasNextPage', () => {
      it(`should have a next page when 'pageIndex' is less than 'getNumberOfPages()' by '2' or more`, () => {
        component.pageIndex = randomPositiveNumber();
        spyOn(component, 'getNumberOfPages').and.returnValue(
          Math.max(component.pageIndex + 2, randomPositiveNumber())
        );
        expect(component.hasNextPage()).toBeTruthy();
      });

      it(`should NOT have a next page when 'pageIndex' is less than 'getNumberOfPages()' by '1' or less`, () => {
        component.pageIndex = randomPositiveNumber();
        spyOn(component, 'getNumberOfPages').and.returnValue(
          Math.min(component.pageIndex + 1, randomPositiveNumber())
        );
        expect(component.hasNextPage()).toBeFalsy();
      });
    });

    describe('getNumberOfPages', () => {
      beforeEach(() => {
        component.pageSize = randomPositiveNumber() + 2;
      });

      it(`should have one page when 'length' is less than 'pageSize`, () => {
        component.length = component.pageSize - 1;
        expect(component.getNumberOfPages()).toBe(1);
      });

      it(`should have one page when 'length' is equal to 'pageSize'`, () => {
        component.length = component.pageSize;
        expect(component.getNumberOfPages()).toBe(1);
      });

      it(`should have two pages when 'length' is greatter than 'pageSize' by '1'`, () => {
        component.length = component.pageSize + 1;
        expect(component.getNumberOfPages()).toBe(2);
      });

      it(`should return the first integer greated than 'length' divided by 'pageSize'`, () => {
        component.length = randomPositiveNumber();
        expect(component.getNumberOfPages()).toBe(
          Math.ceil(component.length / component.pageSize)
        );
      });
    });
  });
});
