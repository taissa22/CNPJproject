export interface APIResponse {
    data: any;
    mensagem: string;
    sucesso: boolean;
    urlRedirect?: string;
    exibeNotificacao: boolean;
}
