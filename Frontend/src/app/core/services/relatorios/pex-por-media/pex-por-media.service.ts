import { HttpClient, HttpErrorResponse,HttpParams } from '@angular/common/http';
import { HttpErrorResult } from '../../../http/http-error-result';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, take, tap } from 'rxjs/operators';
import { OrdenacaoPaginacaoDTO } from '@shared/interfaces/ordenacao-paginacao-dto';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class PexPorMediaService {

constructor(  protected http: HttpClient, private exportarService: TransferenciaArquivos, private messageService: HelperAngular ) { }

paginacaoVerMais = new BehaviorSubject<OrdenacaoPaginacaoDTO>({
  pagina: 1,
  quantidade: 10,
  total: 0
});

limpar() {
  this.paginacaoVerMais.next({
    pagina: 1,
    quantidade: 10,
    total: 0,
  });
}

consultarMais(dtIni: string, dtFim: string) {
  this.paginacaoVerMais.next({
    ...this.paginacaoVerMais.value,
    pagina: this.paginacaoVerMais.value.pagina + 1,
    total: 0
  });
  return this.consultarfechamentosVerMais(dtIni, dtFim).pipe(take(1));
}

consultarfechamentos() {
  return this.buscarporFiltro(
    this.paginacaoVerMais.value['pagina'],10, '', ''
  ).pipe(
    tap(
      res => this.paginacaoVerMais.next({
        ...this.paginacaoVerMais.value,
        total: res['total']
      })
    ),
    map(res => res['data'])
  );
}

consultarfechamentosVerMais(dtIni: string, dtFim: string) {
  return this.buscarporFiltro(
    this.paginacaoVerMais.value['pagina'],10, dtIni == null ? '' : dtIni.toString(), dtFim == null ? '' : dtFim.toString()
  ).pipe(
    tap(
      res => this.paginacaoVerMais.next({
        ...this.paginacaoVerMais.value,
        total: res['total']
      })
    ),
    map(res => res['data'])
  );
}

buscarporFiltro(pag,qtd, dtinicio,dtfim): Observable<any> {
  const backEndURL = environment.api_url + '/FechamentoPexMedia/listar';

  let params = new HttpParams().set('pagina', pag)
                               .set('dataInicio', dtinicio)
                               .set('datafim', dtfim)
                               .set('quantidade', qtd);


  return this.http.get<any>(backEndURL, { params: params });
}

async downloadZip(fechamentoId: number, dataFechamento: Date, dataGeracao: Date): Promise<void> {
   try {
     const backEndURL = `${environment.api_url}/FechamentoPexMedia/download/${fechamentoId}/${dataFechamento.toISOString()}/${dataGeracao.toISOString()}`;

    const response: any = await this.http.get<any>(backEndURL, this.exportarService.downloadOptions).toPromise();
    const file = response.body;
    const fileName = this.exportarService.obterNomeArquivo(response);
     this.exportarService.download(file, fileName);
   } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (error.status == 404) {
        this.messageService.MsgBox2('Arquivo não encontrado, o mesmo encontra-se na fila e será gerado pelo executor', 'Nenhum Arquivo Encontrado', 'warning', 'Ok');
        return;
      } else {
        this.messageService.MsgBox2('Erro Desconhecido', 'Erro', 'warning', 'Ok');
      }
   }
  }
}
