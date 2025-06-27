import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalDownloadExportacoesComponent } from './modal-download-exportacoes.component';

describe('ModalDownloadExportacoesComponent', () => {
  let component: ModalDownloadExportacoesComponent;
  let fixture: ComponentFixture<ModalDownloadExportacoesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalDownloadExportacoesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalDownloadExportacoesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
