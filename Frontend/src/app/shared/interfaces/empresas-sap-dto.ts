export interface EmpresasSapDTO{
  id: number;
  sigla: string;
  nome: string;
  indicaAtivo : boolean;
  codigoOrganizacaocompra : string;
  indicaEnvioArquivoSolicitacao: boolean;
  selected?: boolean;
  confirmaSiglaRepetidaNaAlteracao: boolean;

}
