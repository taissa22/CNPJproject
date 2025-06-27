/**
 * NÃO TOQUE NESTE CÓDIGO
 * 
 * DINOSSAUROS HABITAM NELE, PODE SER BEM PERIGOSO.
 * 
 *                                                                                 
                                                                                
                                          &&&&&&&&&&%.                          
                                        &%% ,%%%%%%%%%%                         
                                        &%%%%%%%%%%%%&%                         
                                        &%%%%%%%&&&&&&%                         
                                        &%%%%&&%%%%%                            
                         .%          **%%%%%%%                                  
                  .&&    .&%.     &%&%%%%%%%&%%%%                               
                  .%&    .%%%&%&&%%%%%%%%%%%%%                                  
                %..%& %  .(&&%%%%%%%%%%%%%%%%%              &&                  
                %..&%&%     *&&%%%%%%%%%%%&&.            %& %% %                
                   %&          #&%%%&&%%&/               %%#&&/&                
                   %&            %&%(  &%/                  %&&                 
                                 %      %/                  %%                  
                                                 &&                             
                                                                                
                   .,                                      
 */

import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-paginacao-buttons',
  templateUrl: './paginacao-buttons.component.html',
  styleUrls: ['./paginacao-buttons.component.scss']
})
export class PaginacaoButtonsComponent implements OnInit {

  @Input() pageMax;
  @Input() totalRegistro;


  @Output() onClickNextPage = new EventEmitter<number>();

  @Input() public currentPage = 1;
  pager: any;
  private totalDeRegistros: any[];
  quantidadeBotoesPage = 4;

  constructor() { }

  ngOnInit() {
    this.totalDeRegistros = this.pageMax;

  }

  onPageChange() {
    this.onClickNextPage.emit(this.currentPage);
  }



}
