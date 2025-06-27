import { GuiasProblema } from '@shared/interfaces/guias-problema';
import { GuiasOK } from '@shared/interfaces/GuiasOK';
import { ResumoImportacaoAquivo } from '@shared/interfaces/resumo-importacao-aquivo';

export interface ArquivoImportacao {
  arquivoGuiasNaoOk: string;
arquivoGuiasOk: string;
guiasComProblema: GuiasProblema[];
guiasOk: GuiasOK[];
nomeArquivo: string;
resumo: ResumoImportacaoAquivo;
}
