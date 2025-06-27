export class Formatter {
    static formatTelefone(area: string, telefone: string): string {
        if (telefone.length === 9) {
            return `(${area}) ${telefone.substring(0, 1)} ${telefone.substring(1, 5)}-${telefone.substring(5)}`;
        }
        return `(${area}) ${telefone.substring(0, 4)}-${telefone.substring(4)}`;
    }
}
