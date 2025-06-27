import { Component, OnInit } from '@angular/core';
import { BBResumoProcessamentoGuiaExibidaDTO } from '@shared/interfaces/BB-Resumo-Processamento-Guia-Exibida-DTO';
import { BBResumoProcessamentoService } from 'src/app/core/services/sap/bbresumo-processamento.service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs/internal/Observable';
import { Subscription } from 'rxjs/internal/Subscription';
import { HeaderService } from '@core/services/header.service';

@Component({
  selector: 'app-imagem-guia',
  templateUrl: './imagem-guia.component.html',
  styleUrls: ['./imagem-guia.component.scss']
})
export class ImagemGuiaComponent implements OnInit {
  private detalheGuia$: Observable<BBResumoProcessamentoGuiaExibidaDTO>;

  constructor(
    private header: HeaderService,
    private resumoProcessamentoService: BBResumoProcessamentoService,
    private route: ActivatedRoute
  ) {}

  subscription: Subscription;
  detalheGuia: BBResumoProcessamentoGuiaExibidaDTO;

  ngOnInit() {
    this.header.setHeaderVisibility(false);

    this.detalheGuia$ = this.route.paramMap.pipe(
      switchMap((params: ParamMap) => {
        return this.resumoProcessamentoService.buscarImagemGuia(
          params.get('codigoProcesso'),
          params.get('codigoLancamento')
        );
      })
    );

    this.subscription = this.detalheGuia$.subscribe(item => {
      this.detalheGuia = item['data'];
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
    this.header.setHeaderVisibility(true);
  }
}
