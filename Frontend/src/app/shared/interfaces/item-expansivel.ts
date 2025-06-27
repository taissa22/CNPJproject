import { IColoredMessage } from '../interfaces/colored-message';

export interface ItemExpansivel {
  isOpen: boolean;
  id: number;
  titulo?: string;
  subtitulo?: string;
  descricao?: string;
  numeroLoteBB?: string;
  estado?: IColoredMessage;
  estadoDetalhado?: string;
  helperLabel?: string;
  observacao?: string;
  iconeObservacao?: string;
}
