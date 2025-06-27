import { BehaviorSubject, Observable } from 'rxjs';

export interface IManutencaoCommonService {
    trocarPagina(pagina: number);
    trocarQuantidade(quantidade: number);
    ordenar(campo: string, ascendente: boolean);
    exportar(nomeArquivo: string): Promise<any>;
    exportarPorFiltro(nomeArquivo: string, json: any): Promise<any>;
    consultarPorFiltros(filtros?: any): Observable<any>;
    excluir(id: any): Observable<any>;
    refreshFiltros(args: object): void;
    refreshFiltroSemQuantidade(args: object): void;
}
