import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-fechamento',
  templateUrl: './fechamento.component.html',
  styleUrls: ['./fechamento.component.scss']
})
export class FechamentoComponent implements OnInit {
  
  modalBool: boolean;

  constructor(private router: Router) { }

  ngOnInit() {
    this.modalBool = false;
  }

  openModal(){
    this.modalBool = !this.modalBool;
    console.log(this.modalBool)
  }

  
}
