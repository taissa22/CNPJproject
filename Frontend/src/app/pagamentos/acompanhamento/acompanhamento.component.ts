import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { AcompanhamentoService } from '../services/acompanhamento.service';
import {
  CargaCompromisso,
  CargaCompromissoParcela,
  ClasseCredito
} from './acompanhamento';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';
import { ModalDownloadExportacoesComponent } from './modal-download-exportacoes/modal-download-exportacoes.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-acompanhamento',
  templateUrl: './acompanhamento.component.html',
  styleUrls: ['./acompanhamento.component.scss']
})
export class AcompanhamentoComponent implements OnInit {
  breadcrumb: string;
  titulo: string = 'Acompanhamento dos Compromissos da RJ';
  filterForm: FormGroup;
  cargas: CargaCompromisso[] = [];
  parcelas: CargaCompromissoParcela[] = [];
  cargaAtual: CargaCompromisso | undefined;
  total: number = 0;
  isAgendar: boolean = false;
  classeCreditoDataSource: ClasseCredito[] = [];

  constructor(
    private fb: FormBuilder,
    private acompanhamentoService: AcompanhamentoService,
    private breadcrumbsService: BreadcrumbsService,
    private modalService: NgbModal,
    private dialog: DialogService
  ) {
    this.filterForm = this.fb.group({
      tipoProcesso: new FormControl(0),
      buscaCampoProcesso: new FormControl({ value: '1', disabled: true }),
      condicaoProcesso: new FormControl({ value: 1, disabled: true }),
      codigoProcesso: new FormControl(''),
      statusParcela: new FormControl(0),
      nome: new FormControl(''),
      documento: new FormControl(''),
      carga: new FormControl(null),
      compromisso: new FormControl(null),
      tipoPesquisa: new FormControl(2),
      vencimentoDe: new FormControl(''),
      vencimentoAte: new FormControl(''),
      classeCredito: new FormControl(null)
    });
  }

  ngOnInit() {
    this.obterCargaDeCompromisso(null, true);
  }

  isValidDate(date: any): boolean {
    return date instanceof Date && !isNaN(date.getTime());
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(
      Permissoes.ACESSAR_ACOMPANHAR_COMPROMISSOS_RJ
    );

    this.obterClasseCredito();
  }

  async obterClasseCredito(): Promise<void> {
    try {
      const response = await this.acompanhamentoService
        .obterClasseCredito()
        .toPromise();

      this.classeCreditoDataSource = response;
      console.log(this.classeCreditoDataSource);
    } catch (error) {
      console.error('Não foi possível carregar as classes de credito', error);
    }
  }

  async obterCargaDeCompromisso(
    filters?: any,
    iniciando: boolean = false
  ): Promise<void> {
    try {
      const size = iniciando ? -1 : 50;
      const response = await this.acompanhamentoService
        .obterCargaDeCompromisso(1, size, filters)
        .toPromise();

      this.total = response.total;
      this.cargas = response.items;
      // this.cargaAtual = this.cargas.length > 0 ? this.cargas[0] : undefined;

      if (response.total > 50) {
        await this.dialog.alert(
          'A pesquisa não poderá ser realizada.',
          'O resultado da pesquisa ultrapassa o limite de 50 compromissos permitidos para exibição em tela. Por favor, utilize a exportação agendada.'
        );
      }
    } catch (error) {
      console.error(
        'Não foi possível carregar a carga de compromissos.',
        error
      );
    }
  }

  async agendarCompromisso(payload: any): Promise<void> {
    try {
      const response = await this.acompanhamentoService
        .agendarCompromisso(payload)
        .toPromise();
      await this.dialog.alert(
        'Agendamento Realizado!',
        'O compromisso foi agendado com sucesso. Você poderá acompanha-lo clicando no botão "Baixar/Acompanhar Agendamentos"'
      );
    } catch (error) {
      console.error('Erro ao realizar agendamento', error);
    }
  }

  // Método para quando clicar em "Agendar"
  setIsAgendar(value: boolean): void {
    this.isAgendar = value;
  }

  getStatusLabel(status: number): string {
    switch (status) {
      case 0:
        return 'Todos';
      case 1:
        return 'Agendada';
      case 2:
        return 'Atrasada';
      case 3:
        return 'Excluída';
      case 4:
        return 'Pagamento em tramitação';
      case 5:
        return 'Paga';
      case 6:
        return 'Lote cancelado pelo usuário';
      case 7:
        return 'Lote retornado com erro no SAP';
      case 9:
        return 'Cancelado';
      case 10:
        return 'Estornada';
      case 11:
        return 'Erro - Processo Excluído';
    }
  }

  getStatusClass(status: number): string {
    switch (status) {
      case 1:
        return 'status-agendado';
      case 2:
        return 'status-atrasado';
      case 3:
        return 'status-excluida';
      case 4:
        return 'status-tramitacao';
      case 5:
        return 'status-pago';
      case 6:
        return 'status-lote-cancelado-usuario';
      case 7:
        return 'status-lote-erro-sap';
      case 8:
        return 'status-vencimento-parcela';
      case 9:
        return 'status-cancelado';
      case 10:
        return 'estornada';
      case 11:
        return 'status-erro-processo-excluido';
    }
  }

  aplicarMascaraDocumento(): void {
    let documento = this.filterForm.get('documento').value;

    documento = documento.replace(/\D/g, '');

    if (documento.length <= 11) {
      documento = documento.slice(0, 11);

      documento = documento.replace(/(\d{3})(\d)/, '$1.$2');
      documento = documento.replace(/(\d{3})(\d)/, '$1.$2');
      documento = documento.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
    } else {
      documento = documento.slice(0, 14);

      documento = documento.replace(/^(\d{2})(\d)/, '$1.$2');
      documento = documento.replace(/^(\d{2})\.(\d{3})(\d)/, '$1.$2.$3');
      documento = documento.replace(/\.(\d{3})(\d)/, '.$1/$2');
      documento = documento.replace(/(\d{4})(\d{1,2})$/, '$1-$2');
    }

    this.filterForm.get('documento').setValue(documento, { emitEvent: false });
  }

  aplicarMascara(valor: string): string {
    valor = valor.replace(/\D/g, '');

    if (valor.length <= 11) {
      valor = valor.replace(/(\d{3})(\d)/, '$1.$2');
      valor = valor.replace(/(\d{3})(\d)/, '$1.$2');
      valor = valor.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
    } else if (valor.length <= 14) {
      valor = valor.replace(/^(\d{2})(\d)/, '$1.$2');
      valor = valor.replace(/^(\d{2})\.(\d{3})(\d)/, '$1.$2.$3');
      valor = valor.replace(/\.(\d{3})(\d)/, '.$1/$2');
      valor = valor.replace(/(\d{4})(\d{1,2})$/, '$1-$2');
    } else {
      valor = valor.replace(/^(\d{5})(\d)/, 'OAB-$1');
    }

    return valor;
  }

  onSubmit() {
    const filters = this.filterForm.value;

    const payload = {
      tipoProcesso:
        filters.tipoProcesso !== null ? Number(filters.tipoProcesso) : null,
      condicaoProcesso:
        filters.condicaoProcesso !== null &&
        !isNaN(Number(filters.condicaoProcesso))
          ? Number(filters.condicaoProcesso)
          : 1,
      codigoProcesso:
        filters.codigoProcesso !== '' ? filters.codigoProcesso : '',
      statusParcela:
        filters.statusParcela !== null ? Number(filters.statusParcela) : null,
      nome: filters.nome || null,
      documento: filters.documento || null,
      carga: filters.carga !== null ? Number(filters.carga) : null,
      compromisso:
        filters.compromisso !== null ? Number(filters.compromisso) : null,
      tipoPesquisa:
        filters.tipoPesquisa !== null ? Number(filters.tipoPesquisa) : null,
      vencimentoDe: filters.vencimentoDe
        ? this.formatDate(filters.vencimentoDe)
        : null,
      vencimentoAte: filters.vencimentoAte
        ? this.formatDate(filters.vencimentoAte)
        : null,

      classeCredito: filters.classeCredito
    };

    // Marca todos os campos como 'tocados' para forçar a detecção de mudanças
    this.filterForm.markAsTouched();
    this.filterForm.markAsDirty();

    if (this.isAgendar) {
      this.agendarCompromisso(payload);
    } else {
      this.obterCargaDeCompromisso(payload);
    }
  }

  clearFilters(): void {
    this.filterForm.reset({
      tipoProcesso: 0,
      codigoProcesso: '',
      condicaoProcesso: 1,
      statusParcela: 0,
      buscaCampoProcesso: 1,
      nome: '',
      documento: '',
      carga: null,
      compromisso: null,
      tipoPesquisa: null,
      vencimentoDe: null,
      vencimentoAte: null
    });

    // this.filterForm.get('vencimentoDe').setValue(null);
    // this.filterForm.get('vencimentoAte').setValue(null);
    this.filterForm.get('vencimentoDe').updateValueAndValidity();
    this.filterForm.get('vencimentoAte').updateValueAndValidity();

    this.obterCargaDeCompromisso(null, true);
  }

  // clearFilters() {
  //   this.filterForm.reset(); // Reseta todos os campos do formulário
  // }

  formatDate(date: any): string | null {
    if (!date) {
      return null;
    }

    const validDate = new Date(date);
    if (isNaN(validDate.getTime())) {
      console.error('Data inválida:', date);
      return null;
    }

    return validDate.toISOString().split('T')[0];
  }

  async baixarAgendamentos(): Promise<void> {
    try {
      const modalRef = this.modalService.open(
        ModalDownloadExportacoesComponent,
        {
          centered: true,
          // backdrop: 'static',
          size: 'lg',
          windowClass: 'modal-agend-vep'
        }
      );
    } catch (error) {
      console.error('Não foi possível abrir o modal de download.', error);
    }
  }

  classeCredito(carga: CargaCompromisso) {
    return carga.classeCredito != '' && carga.classeCredito != null
      ? ' | Crédito: ' + carga.classeCredito
      : '';
  }
}
