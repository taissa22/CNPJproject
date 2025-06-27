import { CriteriosImportacaoArquivoRetornoService } from 'src/app/sap/filtros/criterios-importacao-arquivo-retorno/services/criterios-importacao-arquivo-retorno.service';
import { ConsultaArquivoRetorno } from './../../../../shared/interfaces/consulta-arquivo-retorno';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ArquivoImportacaodto } from '@shared/interfaces/arquivo-Importacao-dto';
import { ResultadoArquivoRetornoService } from '../services/resultado-arquivo-retorno.service';
import { ConsultaArquivoRetornoService } from '../services/consulta-arquivo-retorno.service';

@Component({
  selector: 'app-resultado-arquivo-retorno',
  templateUrl: './resultado-arquivo-retorno.component.html',
  styleUrls: ['./resultado-arquivo-retorno.component.scss']
})
export class ResultadoArquivoRetornoComponent implements OnInit, OnDestroy {

  resultadosArquivoRetorno: ArquivoImportacaodto[] = []; // Dados da tela
  verMais = true;
  resultadoWatcherSubscription: Subscription;

  constructor(private service: ResultadoArquivoRetornoService,
    private route: ActivatedRoute,
    private router: Router,
    private filtrosService: CriteriosImportacaoArquivoRetornoService) { }

  ngOnInit() {
    this.resultadoWatcherSubscription =
      this.service.resultadoSubject.subscribe((e: ArquivoImportacaodto[]) =>{
        this.resultadosArquivoRetorno = e;
        if(e.length == this.service.filtrosUtilizadosSubject$.value['total'])
          this.verMais = false;
        else this.verMais = true;
      });
  }
  manterDados(){
    this.filtrosService.manterDados = true;
  }

  ngOnDestroy() {
    // this.service.limparResultados();
    this.resultadoWatcherSubscription.unsubscribe();
  }

  carregarMais() {
    this.service.verMaisArquivoRetorno();
  }

  exportar() {
    this.service.exportarArquivoRetorno();
  }

  onCardClick(e) {
    this.service.importacaoSelecionadoSubject.next(e);
    this.router.navigate(['sap/interfaceBB/importacao/resultado/guiasOk']);

  }




}
