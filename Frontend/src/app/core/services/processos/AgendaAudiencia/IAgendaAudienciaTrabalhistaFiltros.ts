import { DualListModel } from 'src/app/core/models/dual-list.model'

export interface IAgendaAudienciaTrabalhistaFiltros {
  ListaAdvogado: DualListModel[];
  ListaComarca :DualListModel[] ;
  ListaEmpresa :DualListModel[];
  ListaEstado :DualListModel[];
  ListaPreposto :DualListModel[];
  ListaEscritorio :DualListModel[];

}
