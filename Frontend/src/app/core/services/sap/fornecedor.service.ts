import { FornecedorEditarDto } from './../../../shared/interfaces/fornecedor-editar-dto';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '..';
import { DropDownModel } from '@shared/interfaces/dropdown-model';
import { pluck } from 'rxjs/operators';
import { FornecedorFiltroDTO } from '@shared/interfaces/fornecedor-filtro-dto';
import { Fornecedor } from '@shared/interfaces/fornecedor';
import { CadastrarFornecedorDTO } from '@shared/interfaces/cadastrar-fornecedor-dto';

@Injectable({
  providedIn: 'root'
})
export class FornecedorService {

  constructor(private http: ApiService) { }

  public obterBancoDropDown(): Observable<DropDownModel[]> {
    return this.http
      .get('/Banco/ObterBancoDropDown').pipe(pluck('data'));
  }


  public obterProfissionaisDropDown(): Observable<DropDownModel[]> {
    return this.http
      .get('/Profissional/obterProfissionaisDropDown').pipe(pluck('data'));
  }

  public obterEscritoriosDropDown(): Observable<DropDownModel[]> {
    return this.http
      .get('/Profissional/obterEscritoriosDropDown').pipe(pluck('data'));
  }

  public getFornecedores(json: FornecedorFiltroDTO) : Observable<Fornecedor[]> {
    return this.http.post('/Fornecedor/RecuperarFornecedorPorFiltro', json);
  }

  public excluirFornecedor(codigoFornecedor: number) {
    return this.http.get('/Fornecedor/ExcluirFornecedor?codigoFornecedor=' + codigoFornecedor);
  }

  public cadastrarFornecedor(fornecedor: CadastrarFornecedorDTO) {
    return this.http.post('/Fornecedor/CadastrarFornecedor', fornecedor);
  }

  public editarFornecedor(fornecedor: FornecedorEditarDto) {
    return this.http.post('/Fornecedor/AtualizarFornecedor', fornecedor);
  }
}