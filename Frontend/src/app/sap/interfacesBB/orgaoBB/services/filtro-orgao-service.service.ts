import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { OrgaoBBService } from './orgao-bb.service';

@Injectable({
  providedIn: 'root'
})
export class FiltroOrgaoServiceService {

  constructor(private fb: FormBuilder, private orgaoService: OrgaoBBService,
    private messageService: HelperAngular) { }

  orgaoBBSubject = new BehaviorSubject<any>({});

  form: FormGroup;
  inicializarFormulario(): FormGroup {
    return this.form = this.fb.group({
      nomeBBOrgao: null,
      codigoBBTribunal: null,
      codigoBBComarca: null,
      codigo: null
    }
    );
  }

  get valoresFiltro() {
    let valores = Object.assign({}, this.form.value);
    return valores;
  }

  filtrar() {
    if (this.isFiltroValido) {
      this.orgaoService.consultarPorFiltros(
        this.valoresFiltro
      ).pipe(take(1))
        .subscribe(itens => {
          this.orgaoBBSubject.next(itens);
        });
    }
  }

  get isFiltroValido() {
    if (this.valoresFiltro.nomeBBOrgao
       || this.valoresFiltro.codigoBBTribunal
       || this.valoresFiltro.codigoBBComarca
       || this.valoresFiltro.codigo
       ) {
      return true;
    } else {
      this.messageService.MsgBox2(
        'Pelo menos um dos critérios de busca deve ser utilizado.',
        'A busca não pode ser realizada!', 'warning', 'Ok');
      return false;
    }
  }

}
