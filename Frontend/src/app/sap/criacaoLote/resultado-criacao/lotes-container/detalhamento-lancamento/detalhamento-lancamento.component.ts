import { Component, OnInit, Input } from '@angular/core';
import { LoteCriacaoGeracaoLoteDto } from '@shared/interfaces/lote-criacao-geracao-lote-dto';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { LoteCriacaoEmpresaGrupoDTO } from '@shared/interfaces/lote-criacao-empresa-grupo-dto';
import { DetalhamentoLancamentoService } from './detalhamento-lancamento.service';
import { AbasContentService } from './abasContent/abas-content.service';
import { LoteService } from 'src/app/core/services/sap/lote.service';
import { CriacaoService } from '../../../criacao.service';
import { take } from 'rxjs/operators';
import { txtErroCriacaoLote } from './detalhamento-lancamento.constant';
import { CurrencyPipe } from '@angular/common';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'detalhamento-lancamento',
  templateUrl: './detalhamento-lancamento.component.html',
  styleUrls: ['./detalhamento-lancamento.component.scss']
})
export class DetalhamentoLancamentoComponent implements OnInit {


  private _txtIdentificacaoLote: string = '';
  @Input() existeBordero: boolean;
  @Input() loteValido: boolean;

  constructor(private criacaoService: CriacaoService,
    private service: DetalhamentoLancamentoService,
    private helperAngular: HelperAngular
  ) { }



  ngOnInit() {

    if (!this.loteValido) {
      this.helperAngular.MsgBox2(txtErroCriacaoLote.loteValido, txtErroCriacaoLote.gerarLote, 'warning', 'OK');
    }

  }



  gerarLote() {
    if (this._formValido) {
      this.criacaoService.criarLote();
      this.service.loteChanged.next(true);
    }
  }

  private get _formValido() {
    const listaErros: Array<string> = [];
    let textoErros: string = '';

    if (!this._txtIdentificacaoLote) {
      listaErros.push(txtErroCriacaoLote.identificaoLote);
    }
    if (this.existeBordero && this.criacaoService.borderosSubject.value.length === 0) {
      listaErros.push(txtErroCriacaoLote.bordero);
    }
    if (this.criacaoService.quantidadeLancamentosSelecionados <= 0) {
      listaErros.push(txtErroCriacaoLote.lancamento);
    }

    const valorLancamentos = Number(Math.round(parseFloat(this.criacaoService.valorTotalLancamentosSelecionados + 'e2' )) + 'e-2');

    if (this.existeBordero && valorLancamentos != this.criacaoService.valorTotalBordero) {
      listaErros.push(txtErroCriacaoLote.valorTotal);
    }
    if (listaErros.length > 0) {

      listaErros.forEach(erro => {
        textoErros += `${(listaErros.length === 1 ? '' : '- ')} ${erro} <br><br>`;
      });

      this.helperAngular.MsgBox2(
        textoErros,
        txtErroCriacaoLote.gerarLote,
        'warning',
        'OK',
        '',
        '',
        'content-popup'
      );
      return false;
    }
    else return true;
  }

  textoChange(e) {
    this._txtIdentificacaoLote = e;
    this.criacaoService.txtIdentificacaoLote.next(this._txtIdentificacaoLote);
  }

}
