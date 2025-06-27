import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AgendamentosSaldoGarantiasService } from 'src/app/core/services/sap/agendamentos-saldo-garantias.service';
import { map } from 'rxjs/operators';
import { BuscarAgendamentosService } from '../service/buscar-agendamentos.service';

@Injectable({
  providedIn: 'root'
})
export class AgendamentosSaldoGarantiaResolver implements Resolve<any> {

  constructor(private service: BuscarAgendamentosService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    this.service.limpar();
    return this.service.consultar();
  }
}
