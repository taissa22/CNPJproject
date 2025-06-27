export class FechamentoCCMediaModel {
    id : string
    dataFechamento : Date
    numeroMesesMediaHistorica : number
    indMensal: Boolean
    dataIndMensal: Date
    indBaseGerada: Boolean
    dataGeracao : Date
    usuario: UsuarioModel
    empresaCentralizadora: EmpresaCentralizadoraModel
    valorCorte: number
    percentualHairCut: number
    solicitacaoFechamentoContingencia: number
    empresasParticipantes: string
    fechamentoGerado: Boolean
}
export class UsuarioModel {
  id : string
  nome : string
}
export class EmpresaCentralizadoraModel {
  codigo : number
  nome : string
}

