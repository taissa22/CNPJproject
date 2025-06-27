import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FiltroOrgaoServiceService } from '../services/filtro-orgao-service.service';
import { Subscription } from 'rxjs';
import { BBOrgaoService } from 'src/app/core/services/sap/bborgao.service';
import { ActivatedRoute } from '@angular/router';
import { OrgaoBBService } from '../services/orgao-bb.service';

@Component({
  selector: 'filtro-orgao-bb',
  templateUrl: './filtro-orgao-bb.component.html',
  styleUrls: ['./filtro-orgao-bb.component.scss']
})
export class FiltroOrgaoBbComponent implements OnInit , OnDestroy{

  constructor(private service: FiltroOrgaoServiceService, private orgaoService: OrgaoBBService
  )  { }
  form: FormGroup;

  tribunal;

  comarca;

  subscription: Subscription;

  ngOnInit() {
    this.form = this.service.inicializarFormulario();

    this.subscription = this.orgaoService.comboValueComarcaSubject.subscribe(
      comarca => this.comarca = comarca
    );
    this.subscription = this.orgaoService.comboValueTribunalSubject.subscribe(
      tribunal => this.tribunal = tribunal
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  buscar() {
    this.service.filtrar();
  }
}
