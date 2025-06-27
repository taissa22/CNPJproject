export interface EmpresasSAPGridExibicaoDTO {
    id: number;
    sigla: string;
    nome: string;
    indicaEnvioArquivoSolicitacao: boolean;
    indicaAtivo: boolean;
  codigoOrganizacaocompra: string;
  selected?: boolean;
}
