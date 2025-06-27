import { Component, Input, OnInit } from '@angular/core';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Component({
  selector: 'sisjur-progress-bar',
  templateUrl: './sisjur-progress-bar.component.html',
  styleUrls: ['./sisjur-progress-bar.component.scss']
})
export class SisjurProgressBarComponent implements OnInit {

  constructor(
    private exportarService: TransferenciaArquivos
  ) { }

  ngOnInit() {
    this.calculaPercentual();
  }

  @Input() titulo: string;

  @Input() quantidadeRegistros: number;
  @Input() quantidadeTotalRegistros: number;
  @Input() exibeQuantidade: boolean;

  @Input() exibePercentual: boolean;
  @Input() corPercentual: string;
  percentual: number;

  @Input() tamanhoConteudo: string;
  // xl = w28rem & h1rem
  // sm = w12rem & h0.5rem

  @Input() exibeDownload: boolean;
  @Input() linkDownload: string;

  calculaPercentual(){
    return this.percentual = (this.quantidadeRegistros / this.quantidadeTotalRegistros) * 100;
  }

  async download(): Promise<void> {
    try {
      await this.exportarService.baixarArquivo(this.linkDownload);
    }
    catch (error) {
      throw error;
    }
  }

}
