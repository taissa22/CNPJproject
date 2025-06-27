using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB
{
    /**
     * ViewModel para os parametros juridicos correspondentes ao Upload de arquivos 
     */
    public class ImportacaoParametroJuridicoUploadViewModel
    {
        public int QuantidadeMaxArquivosUpload { get; set; }
        public long TamanhoMaxArquivosUpload { get; set; }

        public static void Mapping(Profile mapper)
        {
        }
    }
}
