using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.Relatorios.Entity;
using Shared.Application.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Relatorios.Contingencia.ViewModel
{
    public class GrupoEmpresaContabilSapArquivoDownloadViewModel : BaseViewModel<long>
    {
        public byte[] Arquivo { get; set; }
        public string NomeDoArquivo { get; set; }
    }

    public class GrupoEmpresaContabilSapFormataViewModel : BaseViewModel<long>
    {
        [Name("Código")]
        public long Id { get; set; }

        [Name("Grupo")]
        public string NomeGrupo { get; set; }

        [Name("Empresas do Grupo")]
        public IEnumerable<EmpresaXIdViewModel> ArrayEmpresas { get; set; }

        public IList<GrupoEmpresaContabilSapFormataViewModel> ToViewModel(IList<GrupoEmpresaContabilSap> lstGrupos)
        {
            IList<GrupoEmpresaContabilSapFormataViewModel> lstFormatadaExportacao = new List<GrupoEmpresaContabilSapFormataViewModel>();

            foreach (var grupo in lstGrupos)
            {
                var gxe = new GrupoEmpresaContabilSapFormataViewModel()
                {
                    Id = grupo.Id,
                    NomeGrupo = grupo.NomeGrupo,
                    ArrayEmpresas = grupo.GrupoEmpresaContabilSapParte.Select(x => new EmpresaXIdViewModel { NomeEmpresa = x.Empresa.Nome, Id = x.EmpresaId }).ToArray()
                };

                lstFormatadaExportacao.Add(gxe);
            }

            return lstFormatadaExportacao;
        }
    }

    public class GrupoEmpresaContabilSapViewModel : BaseViewModel<long>
    {
        public string NomeGrupo { get; set; }
        public IList<EmpresaXIdViewModel> EmpresasGrupo { get; set; }

        public GrupoEmpresaContabilSapViewModel ToViewModel(GrupoEmpresaContabilSap model)
        {
            return new GrupoEmpresaContabilSapViewModel()
            {
                Id = model.Id,
                NomeGrupo = model.NomeGrupo,
                EmpresasGrupo = model.GrupoEmpresaContabilSapParte?.Select(y => new EmpresaXIdViewModel { Id = y.EmpresaId, NomeEmpresa = y.Empresa.Nome }).ToList()
            };
        }
    }

    public class EmpresaXIdViewModel : BaseViewModel<long>
    {
        public string NomeEmpresa { get; set; }
    }
}