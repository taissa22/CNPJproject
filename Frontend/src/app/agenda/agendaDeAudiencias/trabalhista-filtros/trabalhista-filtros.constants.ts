import { SortDirectionEnum } from '@shared/enums/SortDirectionEnum.enum';
import { SortOrderView } from '@shared/interfaces/Sort/SortOrderView';

/**  Ordenação default da tela de resultado */
export const defaultSortOrder: SortOrderView[] = [
  {direction: SortDirectionEnum.crescente, property:'siglaEstado', columnName: 'Estado'},
  {direction:SortDirectionEnum.crescente, property:'comarca',  columnName: 'Comarca',},
  {direction:SortDirectionEnum.crescente, property:'codVara', columnName: 'Vara'},
  {direction:SortDirectionEnum.crescente, property:'tipoVara', columnName: 'Tipo de Vara'},
  {direction:SortDirectionEnum.decrescente, property:'dataAudiencia', columnName: 'Data da Audiência'},
  {direction:SortDirectionEnum.crescente, property:'horarioAudiencia', columnName: 'Hora da Audiência'},
];


