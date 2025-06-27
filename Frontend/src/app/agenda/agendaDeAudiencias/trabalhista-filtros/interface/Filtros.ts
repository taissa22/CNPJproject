import { DualListModel } from '@core/models/dual-list.model';
import { Combobox } from '@shared/interfaces/combobox';

export interface FiltrosAgendaAudienciaResolver{
  listaAdvogado: Combobox[];
  listaComarca: Combobox[];
  listaEmpresa: Combobox[];
  listaEstado: Combobox[];
  listaPreposto: Combobox[];
  listaEscritorio: Combobox[];
}
