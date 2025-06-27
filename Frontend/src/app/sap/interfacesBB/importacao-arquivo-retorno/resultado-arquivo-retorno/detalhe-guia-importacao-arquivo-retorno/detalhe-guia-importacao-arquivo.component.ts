import { CriteriosImportacaoArquivoRetornoService } from 'src/app/sap/filtros/criterios-importacao-arquivo-retorno/services/criterios-importacao-arquivo-retorno.service';
import { ImagemGuiaService } from './../services/imagem-guia.service';
import { ArquivoImportacaodto } from '@shared/interfaces/arquivo-Importacao-dto';
import { DetalheGuiaImportacaoService } from '../../services/detalhe-guia-importacao.service';
import { Component, OnInit } from '@angular/core';
import { InfoTooltip } from 'src/app/core/models/info-tooltip.model';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-detalhe-guia-importacao-arquivo-retorno',
  templateUrl: './detalhe-guia-importacao-arquivo.component.html',
  styleUrls: ['./detalhe-guia-importacao-arquivo.component.scss']
})
export class DetalheGuiaImportacaoArquivoRetornoComponent implements OnInit {

  public infoTooltip: { totalGuias: number, totalValores: number }
  = {totalGuias: 0, totalValores: 0}
  public temPermissao = true;

  constructor(private service: DetalheGuiaImportacaoService,
  private imagemService: ImagemGuiaService,
  private criterioService: CriteriosImportacaoArquivoRetornoService) { }

  //pipe importacaArquivoBbAlias'

  dadosArquivo: ArquivoImportacaodto = this.service.dadosArquivo;

  guias;
  guiasHeader;

  ultimaGuiaSelecionada = null;

  ngOnInit() {

    this.service.guiasOk().subscribe(item =>
    {
      this.infoTooltip.totalGuias = item.data.length
      this.service
      this.guias = item.data;
      this.guiasHeader = Object.keys(item.data[0]).filter(item => !this.excluirHeader.includes(item));
      this.guias.map(e => e.selected = false);
    })
  }

  excluirHeader =["codigoLancamento", "codigoProcesso"]

   selectGuia(guia, guiaIndex) {
    this.guias.map(e => e.selected = false);
    if(this.ultimaGuiaSelecionada == guiaIndex)
      this.ultimaGuiaSelecionada = null
    else {
      this.guias[guiaIndex].selected = true;
      this.ultimaGuiaSelecionada = guia;
    }
  }

  onClickExportar() {
    const numeroLoteBB = this.dadosArquivo.numeroLoteBB;

    this.service.exportar(numeroLoteBB);
  }

  abrirImagemGuia() {
    this.imagemService.inicializar(this.ultimaGuiaSelecionada);
  }

  manterDados(){
    this.criterioService.manterDados = true;
  }
}
