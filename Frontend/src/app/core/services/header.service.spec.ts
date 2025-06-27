import { fakeAsync, TestBed, tick } from '@angular/core/testing';
import { HeaderService } from './header.service';

describe('HeaderService', () => {
  let service: HeaderService;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [HeaderService] });
    service = TestBed.get(HeaderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should have headerVisibility defined as "true"', () => {
    expect(service.headerVisible).toBeTruthy();
  });

  it('should change visibility value', () => {
    service.setHeaderVisibility(false);
    expect(service.headerVisible).toBeFalsy();
  });

  it('should triger visibility change', fakeAsync(() => {
    let value: boolean = true;
    service.headerVisibilityChange().subscribe(v => (value = v));
    service.setHeaderVisibility(false);
    tick();
    expect(value).toBeFalsy();
  }));
});
