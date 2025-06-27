import { Injectable } from '@angular/core';
import { EmpresasSAPService } from 'src/app/core/services/sap/empresas-sap.service';
import { EmpresasSapDTO } from '@shared/interfaces/empresas-sap-dto';
import { ManutencaoCommonService } from 'src/app/sap/shared/services/manutencao-common.service';

@Injectable({
  providedIn: 'root'
})
export class ManutencaoEmpresasSAPService extends ManutencaoCommonService<EmpresasSapDTO, number>{

  constructor(protected  service: EmpresasSAPService) {
    super(service);
  }

}
