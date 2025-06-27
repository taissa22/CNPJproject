import { CentroCustoService } from './../../../../core/services/sap/centroCusto.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
  })
export class TrabalhistaResultadoCrudService {

constructor(private fb: FormBuilder) { }
registerForm: FormGroup;


inicializarForm(): FormGroup {
    this.registerForm = this.fb.group({
      descricaoCentroCusto: ['', [Validators.maxLength(100), Validators.required
        ]],
      centroCustoSAP: ['', [Validators.maxLength(10), Validators.required
     ]],
      indicaAtivo: true
    });

    return this.registerForm;
  }

}
