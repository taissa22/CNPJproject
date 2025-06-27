import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-mes-ano-range',
  templateUrl: './mes-ano-range.component.html',
  styleUrls: ['./mes-ano-range.component.scss']
})
export class MesAnoRangeComponent implements OnInit {

  constructor(private fb: FormBuilder) { }

  dadosDaBusca: FormGroup;

  ngOnInit() {

    this.dadosDaBusca = this.fb.group({
      mesAnoInicial: { year: 2017, month: 8 },
      mesAnoFinal: { year: 2017, month: 8 }
    });
    // TODO: ajustar componente de renge
    this.dadosDaBusca.valueChanges.subscribe(data => {

      console.log('data: ', data.mesAnoInicial['year'])

      if ((data.mesAnoInicial['year'] * 1) < (data.mesAnoFinal['year'] * 1)) {

        console.log('true')
      } else if ((data.mesAnoInicial['year'] * 1) == (data.mesAnoFinal['year'] * 1)) {

        if ((data.mesAnoInicial['month'] * 1) <= (data.mesAnoFinal['month'] * 1)) {
          console.log('true')
        } else {
          console.log('false')
        }

      } else {
        console.log('falso')
      }

      // if (data.mesAnoInicial['year'] >= data.mesAnofinal['year']) {
      //   console.log('true')
      // } else {
      //   console.log('false')
      // }
    }

    );
  }


}
