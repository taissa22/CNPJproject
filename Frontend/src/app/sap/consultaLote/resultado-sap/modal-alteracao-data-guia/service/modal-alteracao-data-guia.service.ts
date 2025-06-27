import { DownloadService } from 'src/app/core/services/sap/download.service';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { LoteService } from 'src/app/core/services/sap/lote.service';
import { BehaviorSubject } from 'rxjs';
import { formatDate } from '@angular/common';

export interface Json {
  codigoLote: number;
  numeroLoteBB: number;
  enviarServidor: boolean;
  dataGuia: string;
}
@Injectable({
  providedIn: 'root'
})
export class ModalAlteracaoDataGuiaService {

  constructor(private fb: FormBuilder, private loteService: LoteService,
  private messageService: HelperAngular, private downloadService: DownloadService) { }

  form: FormGroup;

  fechaModal = new BehaviorSubject<boolean>(false);

  resultadoSelecionado = new BehaviorSubject(null);

  inicializarForm() : FormGroup{
    this.form = this.fb.group({
      opcaoSelecionada: "1",
      dataGuia: {value: '', disabled: true},
      enviarServidor: true
    });
    return this.form;
  }



  salvar(formulario: FormGroup) {
    if (formulario.valid) {
      this.loteService.regerarArquivoBB(this.montarJson(formulario)).subscribe(item => {
        if (item.sucesso) {
          const buffer = this.downloadService.converterBase64ParaBuffer(item.data.file);
          this.downloadService.prepararDownload(buffer, item.data.fileName);
          this.fechaModal.next(true);
        } else {
          this.messageService.MsgBox2(item.mensagem, 'Ops!', 'warning', 'Ok');
          this.fechaModal.next(false);
        }
      });
    }
  }

  private get ResultadoSelecionado() {
    return this.resultadoSelecionado.value;
  }

 private montarJson(formulario: FormGroup): Json  {
   let json: Json = Object.assign({}, formulario.value);
   json.dataGuia ? json.dataGuia = formatDate(json.dataGuia, 'yyyy/MM/dd 00:00:00', 'en_US') : null;
   json.codigoLote = this.ResultadoSelecionado.id;
   json.numeroLoteBB = this.ResultadoSelecionado.numeroLoteBB;
   return json;
  }


}
