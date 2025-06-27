import { Component, Input, OnInit } from '@angular/core';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Component({
  selector: 'app-alerta-download',
  templateUrl: './alerta-download.component.html',
  styleUrls: ['./alerta-download.component.scss']
})
export class AlertaDownloadComponent implements OnInit {

  constructor(
    private exportarService: TransferenciaArquivos
  ) { }

  ngOnInit() {
  }

  @Input() quantidade: number;
  @Input() titulo: string;
  @Input() status: string;
  @Input() dias: number;
  @Input() cor: string;
  @Input() linkDownload: string;

  async download(): Promise<void> {
    try {
      await this.exportarService.baixarArquivo(this.linkDownload);
    }
    catch (error) {
      throw error;
    }
  }

}
