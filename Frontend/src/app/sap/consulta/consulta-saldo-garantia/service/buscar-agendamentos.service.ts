import { Injectable } from '@angular/core';
import { of, BehaviorSubject } from 'rxjs';
import { DownloadService } from 'src/app/core/services/sap/download.service';
import { OrdenacaoPaginacaoDTO } from '@shared/interfaces/ordenacao-paginacao-dto';
import { AgendamentosSaldoGarantiasService } from 'src/app/core/services/sap/agendamentos-saldo-garantias.service';
import { map, tap, take } from 'rxjs/operators';
import { concatenarNomeExportacao, removerAcentos, TiposProcessosMapped } from '@shared/utils';
import { ConsultaTipoProcessoService } from './consulta-tipo-processo.service';

@Injectable({
  providedIn: 'root'
})
export class BuscarAgendamentosService {

  filtrosSubject = new BehaviorSubject<OrdenacaoPaginacaoDTO>({
    pagina: 1,
    quantidade: 5,
    total: 0
  });

  constructor(private service: AgendamentosSaldoGarantiasService,
    private downloadService: DownloadService,
    private tipoProcessoService: ConsultaTipoProcessoService) { }

  excluir(item) {
    return this.service.excluir(item);
  }

  download(nomeArquivo, tipoProcesso) {
    return this.service.baixarAgendamento(nomeArquivo)
      .pipe(take(1))
      .subscribe(res => {
        this.downloadService.prepararDownload(res, nomeArquivo);
      });
  }

  nomeTipoProcessoSemEspaco(tipoProcesso) {
   let nome
    TiposProcessosMapped.filter(i => i.idTipo == tipoProcesso)
   .map(item => nome = item.nome)
    nome = nome.replace(/\s/g, '');
    nome = removerAcentos(nome);
    return nome;

  }

  consultar() {
    return this.service.consultarAgendamentos(
      this.filtrosSubject.value
    ).pipe(
      tap(
        res => this.filtrosSubject.next({
          ...this.filtrosSubject.value,
          total: res['total']
        })
      ),
      map(res => res['data'])
    );
  }

  consultarMais() {
    this.filtrosSubject.next({
      ...this.filtrosSubject.value,
      pagina: this.filtrosSubject.value.pagina + 1,
      total: 0
    });
    return this.consultar().pipe(take(1));
  }

  limpar() {
    this.filtrosSubject.next({
      pagina: 1,
      quantidade: 5,
      total: 0,
    });
  }


}
