import { PermissoesSapService } from 'src/app/sap/permissoes-sap.service';
import swal from 'sweetalert2';
import { LoteService } from 'src/app/core/services/sap/lote.service';
import { Injectable } from '@angular/core';
import { ApiService } from './../api.service';
import { Observable, BehaviorSubject } from 'rxjs';
import { pluck, take } from 'rxjs/operators';
import { DetalhamentoResultadoLote } from '../../models/detalhamento-resultado-lote';
import { ItemExpansivel } from '@shared/interfaces/item-expansivel';
import { HelperAngular } from '@shared/helpers/helper-angular';


@Injectable({
  providedIn: 'root'
})
export class DetalheResultadoService {


  constructor(private http: ApiService,
    private loteService: LoteService,
    private permissoesService: PermissoesSapService,
    private msgService: HelperAngular) { }

  public currentItem;
  public currentObject;
  public itemReceived = new BehaviorSubject<number>(0);
  public objectReceived = new BehaviorSubject({} as ItemExpansivel);
  public sinalChange = new BehaviorSubject<boolean>(null);

  public newLoteBB = new BehaviorSubject<number>(null);

  setCurrentItem(item) {
    console.log(item);
    this.objectReceived.next(this.currentObject);
    this.currentObject = item;
    item.isOpen = true;
    this.currentItem = item.id;
    this.itemReceived.next(item.id);
  }

  setSinal(sinal: boolean) {
    this.sinalChange.next(sinal);

  }



  //get
  getDetalhamentoLote(numeroLote: number): Observable<DetalhamentoResultadoLote> {

    return this.http
      .get('/Lotes/RecuperaDetalhes?CodigoLote=' + numeroLote).pipe(pluck('data'));

  }





  regerarLoteBB(lote: number) {


    swal.fire({
      title: 'Regerar nº lote',
      html: 'Deseja regerar o nº do lote BB?',
      icon: 'question',
      confirmButtonColor: '#6F62B2',
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      cancelButtonColor: '#9597a6',
      showCancelButton: true,
      showConfirmButton: true
    }).then((result) => {
      if (result.value) {
        this.loteService.getAlterarNumeroBB(lote).pipe(take(1)).subscribe(response => {
          if (response.sucesso) {
            this.newLoteBB.next(response.data);

          } else {
            this.msgService.MsgBox(response.mensagem, 'Ops!');

          }



        });
      }
    });
  }

  verificarPermissaoGerarLoteBB(): boolean {
    let permissao = false;

    if (this.permissoesService.f_RegerarLotesBBCivelCons || this.permissoesService.f_RegerarLotesBBJuizado) {

      permissao = true;

    }
    return permissao;

  }





}
