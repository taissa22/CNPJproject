export class CsvFile {
    public header: string[];
    public data: string[];
    private unmappedHeader: string[];
    private sep: string;
    private mappingFunction: (headerData: string) => string;
    private replacerFunction: (key: string, value: string) => string;
    private csvFile;
    /**
     *  Cria um novo arquivo .csv
     *  @param data: Dados do CSV
     *  @param columns: Colunas do CSV
     *  @param sep: Separador à ser utilizado no CSV,
     *  @param headerMappingFunction: Função que mapeia o header à uma "tradução"
     *  @param replacer: Função que altera valores KEY para valores VALUE
     */
    constructor(data: string[],
                columns: string[],
                sep: string,
                headerMappingFunction?: (headerData: string) => string,
                replacer = (key, value) => value === null ? '' : value) {
        this.unmappedHeader = columns;
        this.mappingFunction = headerMappingFunction;
        if (headerMappingFunction) {
            this.header = this.unmappedHeader.map(col => headerMappingFunction(col));
        }
        this.data = data;
        this.sep = sep;
        this.replacerFunction = replacer;
        this.mapDataToCsv();
    }

    private mapDataToCsv() {
        this.csvFile = this.data.map(row =>
                    this.unmappedHeader.map(fieldName => JSON.stringify(row[fieldName],
                                                                        this.replacerFunction)).join(this.sep));
    }

    private addHeader() {
        this.csvFile.unshift(this.header.join(this.sep));
    }

    public save(name: string) {
        this.addHeader();
        const csvArray = this.csvFile.join('\r\n');

        const blob = new Blob(["\ufeff" + csvArray], {
            type: 'text/csv; charset=utf-18'
        });

        if (navigator.msSaveBlob) {
            navigator.msSaveBlob(blob, name);
            return;
        }

        const link = document.createElement('a');
        if (link.download !== undefined) {
            link.setAttribute('href', URL.createObjectURL(blob));
            link.setAttribute('download', name);
            link.style.visibility = 'hidden';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }
}
