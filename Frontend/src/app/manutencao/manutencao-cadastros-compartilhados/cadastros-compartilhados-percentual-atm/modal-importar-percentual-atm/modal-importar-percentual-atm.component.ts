import { HttpErrorResult } from '@core/http/http-error-result';
import { isNullOrUndefined } from 'util';
import { DialogService } from '@shared/services/dialog.service';
import { FormControl } from '@angular/forms';
import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  EventEmitter,
  Output
} from '@angular/core';
import { PercentualAtmCrudService } from './services/percentual-atm-crud.service';
import {
  NgbActiveModal,
  NgbModal,
  NgbModalRef
} from '@ng-bootstrap/ng-bootstrap';
import { StaticInjector } from '@manutencao/static-injector';
import { PercentualAtmService } from '@manutencao/services/percentual-atm.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'modal-importar-percentual-atm',
  templateUrl: './modal-importar-percentual-atm.component.html',
  styleUrls: ['./modal-importar-percentual-atm.component.scss']
})
export class ModalImportarPercentualAtmComponent implements OnInit {
  @ViewChild('arquivo', { static: false }) arquivo: ElementRef;

  nomeArquivoFormControl: FormControl = new FormControl('');
  vigencia: string = new Date().toString();
  arquivoCsv: File;
  tamanhoMaximoArquivoAnexo: number = 4194304;
  existeVigencia: boolean = false;
  fecharModal: boolean = true;
  dataVigenciaFormControl: FormControl = new FormControl('');
  enviarData = new Subject<Date>();

  @Output() recarregarLista: EventEmitter<void> = new EventEmitter();

  constructor(
    private dialog: DialogService,
    private crudService: PercentualAtmCrudService,
    private service: PercentualAtmService,
    public activeModal: NgbActiveModal
  ) {}

  private codTipoProcesso: number;

  ngOnInit() {}

  async verificarExisteVigencia(): Promise<void> {
    try {
      var validaData = new Date(this.vigencia).toISOString().substr(0, 10);
      this.existeVigencia = await this.crudService.existeVigencia(validaData);
    } catch (error) {
      console.error(error);
      this.dialog.err(
        'Não foi possível exportar as informações',
        (error as HttpErrorResult).messages.join('\n')
      );
    }
  }

  async criar(): Promise<void> {
    var result = true;

    if (this.tamanhoMaximoArquivoAnexo < this.arquivoCsv.size) {
      await this.dialog.err(
        'Desculpe, o upload do arquivo não poderá ser realizado',
        `O tamanho do arquivo importado ultrapassa o limite permitido de 4 Mb`
      );
      return;
    }
    if (isNullOrUndefined(this.arquivoCsv)) {
      await this.dialog.err(
        'Desculpe, mas o arquivo CSV não foi preenchido',
        'Nunhum arquivo CSV foi selecionado.'
      );
      return;
    }
    if (this.existeVigencia) {
      result = await this.dialog.confirm(
        'Importar % de ATM',
        'Já existem percentuais de ATM cadastrados para essa vigência.\nDeseja sobrescrever?'
      );
      this.fecharModal = result;
    }

    if (result) {
      try {
        await this.service.criar(this.arquivoCsv, this.vigencia, this.codTipoProcesso);
        await this.dialog.alert(
          'Importação realizada',
          'Percentuais registrados com sucesso.'
        );
      } catch (error) {
        console.error(error);
        await this.dialog.err(
          'Operação não realizada',
          (error as HttpErrorResult).messages.join('\n')
        );
        return;
      }
    }
  }

  aoAdicionarArquivo() {
    const arquivo: File = this.arquivo.nativeElement.files;

    if (!this.validarArquivoSelecionado(arquivo)) {
      this.dialog.err(
        'Desculpe, o upload do arquivo não poderá ser realizado',
        'O arquivo importado não contém dados.'
      );
      return;
    }
    if (this.obterExtensaoDoArquivo(arquivo[0].name) === '.csv') {
      this.nomeArquivoFormControl.setValue(arquivo[0].name);
    } else {
      this.dialog.err(
        'Desculpe, o upload do arquivo não poderá ser realizado',
        'O arquivo importado não é CSV.'
      );
      return;
    }
    this.arquivoCsv = arquivo[0];
  }

  obterExtensaoDoArquivo(nomeArquivo): string {
    const indexExtensaoDoArquivo = nomeArquivo.lastIndexOf('.');
    return nomeArquivo.slice(indexExtensaoDoArquivo);
  }

  validarArquivoSelecionado(arquivo: File): boolean {
    var validado = true;
    if (arquivo[0].size <= 17) validado = false;
    else if (isNullOrUndefined(arquivo)) validado = false;
    return validado;
  }

  downloadArquivoPadraoURL(): string {
    return this.crudService.downloadArquivoURL();
  }

  async confirmar(): Promise<void> {
    if (this.nomeArquivoFormControl.value === '') {
      this.dialog.err(
        'O upload do arquivo não poderá ser realizado',
        'Não existe arquivo anexado.'
      );
      return;
    } else if (
      !this.dataVigenciaFormControl.valid ||
      this.dataVigenciaFormControl.value === ''
    ) {
      this.dialog.err(
        'O upload do arquivo não poderá ser realizado',
        'É necessária uma data de vigência.'
      );
      return;
    }

    await this.verificarExisteVigencia();
    await this.criar();

    this.enviarData.next(this.dataVigenciaFormControl.value);
    if (this.fecharModal) await this.activeModal.close();
  }

  cancelar(): void {
    this.activeModal.close('cancel');
  }

  atualizarData(data: Date = null) {
    this.vigencia = new Date(data).toISOString().substr(0, 10);
  }

  public static exibeModal(codTipoProcesso: number):NgbModalRef {
    let modalRef = StaticInjector.Instance.get(NgbModal).open(ModalImportarPercentualAtmComponent, {
      centered: true,
      backdrop: 'static',
      size: 'lg'
    });
    modalRef.componentInstance.codTipoProcesso = codTipoProcesso;
    modalRef.result;
    return  modalRef;
  }


}
