import { Injectable } from '@angular/core';
import { ManutencaoCommonService } from 'src/app/sap/shared/services/manutencao-common.service';
import { FornecedoresContingenciaSap } from '@shared/interfaces/fornecedores-contingencia-sap';
import { BehaviorSubject } from 'rxjs';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { FornecedoresContingenciaSAPService } from 'src/app/core/services/sap/FornecedoresContingenciaSap.service';

export const fornecedoresHeaderOrdenada = ['id',
'nome',
'codigo',
'cnpj',
'valorCartaFianca',
'dataVencimentoCartaFianca',
'statusFornecedor']
@Injectable({
  providedIn: 'root'
})
export class FornecedoresContingenciaSapService extends ManutencaoCommonService<FornecedoresContingenciaSap, number> {

  public fornecedoresSelecionado = new BehaviorSubject<any>(null);
  modoSalvar = 'Cadastrar';
  private valor: any;

  fornecedorContingenciaSubject = new BehaviorSubject<any>({});

  fecharModal = new BehaviorSubject<boolean>(false);

  constructor(public service: FornecedoresContingenciaSAPService
) {
    super(service);
}


}

