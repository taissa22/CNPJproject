import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { textosValidacaoFormulario } from '@shared/utils';
import { ConsultaSaldoGarantiaService } from '../service/consulta-saldo-garantia.service';

@Component({
  selector: 'app-modal-adicionar-agendamento',
  templateUrl: './modal-adicionar-agendamento.component.html',
  styleUrls: ['./modal-adicionar-agendamento.component.scss']
})
export class ModalAdicionarAgendamentoComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef,private fb:FormBuilder, private service: ConsultaSaldoGarantiaService) { }

  registerForm: FormGroup
  nomeAgendamento: string;



  ngOnInit() {
    this.registerForm = this.fb.group({nomeAgendamento: ['', [Validators.required]]})

    this.service.fecharModal.subscribe(item =>
      {
        if(item){
          this.bsModalRef.hide()
        }
      })

  }

  validacaoTextos(nomeControl, nomeCampo, nomeFeminino){
    return textosValidacaoFormulario(this.registerForm, nomeControl, nomeCampo, nomeFeminino)
  }

  salvarAlteracao(){
    this.service.realizarAgendamento(this.nomeAgendamento, this.service.json);
  }

}
