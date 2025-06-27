import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { listLocales } from 'ngx-bootstrap/chronos';
import { connectableObservableDescriptor } from 'rxjs/internal/observable/ConnectableObservable';
import { ptBrLocale, BsLocaleService, defineLocale } from 'ngx-bootstrap';
import { FormControl, FormGroup } from '@angular/forms';
 
@Component({
  selector: 'mes-ano-new',
  templateUrl: './mes-ano-new.component.html',
  styleUrls: ['./mes-ano-new.component.scss']
})
export class MesAnoNewComponent implements OnInit {

  @Input() id : string; 
  @Input() control : FormControl; 
  @Input() disabled: boolean = false;
  @Input() maxDate: Date = null;
  @Input() minDate: Date = null;
  @Input() required: boolean = false;
  @Input() ngbTooltip: any = null;
  @Input() disableTooltip: any = null;
  @Input() msgMinDate: string = "Data menor que a permitida";
  @Input() msgMaxDate: string = "Data maior que a permitida";

  name = 'Angular';
  dataValue: Date;

  format = 'pt-br';
  mensagemInvalida = 'Data Inválida';


  valorDigitado = 'teste';
  
  customBsConfig: Partial<BsDatepickerConfig> = new BsDatepickerConfig();
  
  constructor(private localeService: BsLocaleService) {
    ptBrLocale.invalidDate = this.mensagemInvalida;
    defineLocale(this.format, ptBrLocale); 
    this.calanderConfigInit();
  }
  
  ngOnInit(){
    this.customBsConfig.containerClass = 'mes-ano-container';
    setTimeout(() => {
      if (this.disabled) {
        this.control.disable();
      }
    }, 0);    
  }  

  onOpenCalendar(container) {
    container.monthSelectHandler = (event: any): void => {
      container._store.dispatch(container._actions.select(event.date));
    };     
    container.setViewMode('month');
  }

  calanderConfigInit() {    
    this.localeService.use(this.format);
    this.customBsConfig.monthLabel = 'MMM';
    this.customBsConfig.dateInputFormat = 'MM/YYYY';
    // this.customBsConfig.container = 

  }  

}

