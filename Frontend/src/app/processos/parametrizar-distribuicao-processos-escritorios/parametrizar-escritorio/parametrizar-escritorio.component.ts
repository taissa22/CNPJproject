import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { ColunaGenerica } from '@manutencao/models/coluna-generica.model';
import { Page } from '@shared/types/page';
import { EscritorioModalComponent } from '../../modals/escritorio-modal/escritorio-modal.component';
import { ParametrizarDistribuicaoProcessosService } from '../../services/parametrizar-distribuicao-processos.service';
import { DialogService } from '@shared/services/dialog.service';
import { HelperAngular } from '@shared/helpers/helper-angular';

@Component({
  selector: 'parametrizar-escritorio',
  templateUrl: './parametrizar-escritorio.component.html',
  styleUrls: ['./parametrizar-escritorio.component.scss']
})
export class ParametrizarEscritorioComponent implements OnInit {

  @Input() distribuicao: any;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;  

  constructor(
    private dialog: DialogService,
    private messageService: HelperAngular,
    private service: ParametrizarDistribuicaoProcessosService,
  ) { }

  ngOnInit() {
    this.obterEscritorios();
  }

  dadosEscritorios = [];
  totalDadosEscritorios: number;
  colunas: Array<ColunaGenerica> = [
    new ColunaGenerica('Nome', 'Nome', true, '22%', 'Nome', {
      display: 'inline-block'
    }),
    new ColunaGenerica('Solicitante', 'Solicitante', true, '25%', 'Solicitante', {
      display: 'inline-block'
    }),
    new ColunaGenerica('Vigência', 'Vigencia', true, '22%', 'Vigencia', {
      display: 'inline-block'
    }),
    new ColunaGenerica('% Processos', 'Processos', true, '11%', 'Processos', {
      display: 'inline-block'
    }),
    new ColunaGenerica('Prioridade', 'Prioridade', true, '11%', 'Prioridade', {
      display: 'inline-block'
    })
  ];
  ordenacaoColuna: 'Nome' | 'Solicitante' | 'Vigencia' | 'Processos' | 'Prioridade';
  ordenacaoDirecao: 'asc' | 'desc' | null = 'asc';
  ehAsc: true | false;
  campoPesquisa = '';

  obterEscritorios() {
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize};
    
    this.service.obterEscritorioDistribuicao(this.distribuicao.codigo, this.campoPesquisa, this.ordenacaoColuna, this.ehAsc, page.index - 1, page.size).then(x => {
      this.dadosEscritorios = x.lista;
      this.totalDadosEscritorios = x.total;
    })
  }

  iniciaValoresDaView() {
    return {
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  } 

  ordenar(data) {
    this.ordenacaoColuna = data;
    if (this.ordenacaoDirecao == 'asc') this.ordenacaoDirecao = 'desc';
    else if (this.ordenacaoDirecao == 'desc') {
      this.ordenacaoDirecao = null;
      this.ordenacaoColuna = null;
    }
    else this.ordenacaoDirecao = 'asc';
    this.ehAsc = this.ordenacaoDirecao == 'asc'
    this.obterEscritorios();
  }

}
