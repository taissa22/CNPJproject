using Perlink.Oi.Juridico.Application.Processos.ViewModel.AgendaAudiencia;
using Perlink.Oi.Juridico.Domain.Processos.DTO;

namespace Perlink.Oi.Juridico.Application.Processos.Adapters
{
    public class AudienciaProcessoAdapter
    {
        public static AudienciaProcessoResultadoVM ToViewModel(AudienciaProcessoResultadoDTO dto)
        {
            return new AudienciaProcessoResultadoVM
            {
                CodProcesso = dto.CodProcesso,
                SeqAudiencia = dto.SeqAudiencia,
                SiglaEstado = dto.SiglaEstado,
                Comarca = dto.Comarca,
                CodVara = dto.CodVara,
                TipoVara = dto.TipoVara,
                DataAudiencia = dto.DataAudiencia,
                HorarioAudiencia = dto.HorarioAudiencia,
                TipoAudiencia = dto.TipoAudiencia,
                Preposto = dto.Preposto,
                EscritorioAudiencia = dto.EscritorioAudiencia,
                AdvogadoAudiencia = dto.AdvogadoAudiencia,
                PrepostoAcompanhante = dto.PrepostoAcompanhante,
                EscritorioAcompanhante = dto.EscritorioAcompanhante,
                AdvogadoAcompanhante = dto.AdvogadoAcompanhante,
                TipoProcesso = dto.TipoProcesso,
                Estrategico = dto.Estrategico,
                NumeroProcesso = dto.NumeroProcesso,
                ClassificacaoHierarquica = dto.ClassificacaoHierarquica,
                EmpresaGrupo = dto.EmpresaGrupo,
                Endereco = dto.Endereco,
                EscritorioProcesso = dto.EscritorioProcesso                  
            };
        }

        public static AudienciaProcessoCompletoResultadoVM ToExportViewModel(AudienciaProcessoCompletoResultadoDTO dto)
        {
            return new AudienciaProcessoCompletoResultadoVM
            {
                CodProcesso = dto.CodProcesso,
                SeqAudiencia = dto.SeqAudiencia,
                SiglaEstado = dto.SiglaEstado,
                Comarca = dto.Comarca,
                CodVara = dto.CodVara,
                TipoVara = dto.TipoVara,
                DataAudiencia = dto.DataAudiencia,
                HorarioAudiencia = dto.HorarioAudiencia,
                TipoAudiencia = dto.TipoAudiencia,
                Preposto = dto.Preposto,
                EscritorioAudiencia = dto.EscritorioAudiencia,
                AdvogadoAudiencia = dto.AdvogadoAudiencia,
                TelefoneAdvogadoEscritorio = dto.NroAdvogadoEscritorio != string.Empty ? $"({dto.DddAdvogadoEscritorio}) {dto.NroAdvogadoEscritorio}" : string.Empty,
                PrepostoAcompanhante = dto.PrepostoAcompanhante,
                EscritorioAcompanhante = dto.EscritorioAcompanhante,
                AdvogadoAcompanhante = dto.AdvogadoAcompanhante,
                TelefoneAdvogadoAcompanhante = dto.NroAdvogadoAcompanhante != string.Empty ? $"({dto.DddAdvogadoAcompanhante}) {dto.NroAdvogadoAcompanhante}" : string.Empty,
                TipoProcesso = dto.TipoProcesso,
                Estrategico = dto.Estrategico,
                NumeroProcesso = dto.NumeroProcesso,
                ClassificacaoHierarquica = dto.ClassificacaoHierarquica,
                EmpresaGrupo = dto.EmpresaGrupo,
                Endereco = dto.Endereco,
                EscritorioProcesso = dto.EscritorioProcesso,
                Migrado = dto.Migrado,
                Reclamadas = dto.Reclamadas,
                Reclamante = dto.Reclamante,
                Pedido = dto.Pedido
            };
        }

        public static AudienciaProcessoResultadoVM ToDownloadViewModel(AudienciaProcessoResultadoDTO dto)
        {
            return new AudienciaProcessoResultadoVM
            {
                CodProcesso = dto.CodProcesso,
                SeqAudiencia = dto.SeqAudiencia,
                SiglaEstado = dto.SiglaEstado,
                Comarca = dto.Comarca,
                CodVara = dto.CodVara,
                TipoVara = dto.TipoVara,
                DataAudiencia = dto.DataAudiencia,
                HorarioAudiencia = dto.HorarioAudiencia,
                TipoAudiencia = dto.TipoAudiencia,
                Preposto = dto.Preposto,
                EscritorioAudiencia = dto.EscritorioAudiencia,
                AdvogadoAudiencia = dto.AdvogadoAudiencia,
                PrepostoAcompanhante = dto.PrepostoAcompanhante,
                EscritorioAcompanhante = dto.EscritorioAcompanhante,
                AdvogadoAcompanhante = dto.AdvogadoAcompanhante,
                TipoProcesso = dto.TipoProcesso,
                Estrategico = dto.Estrategico,
                NumeroProcesso = dto.NumeroProcesso,
                ClassificacaoHierarquica = dto.ClassificacaoHierarquica,
                EmpresaGrupo = dto.EmpresaGrupo,
                Endereco = dto.Endereco,
                EscritorioProcesso = dto.EscritorioProcesso,
                DddAdvogadoEscritorio = dto.DddAdvogadoEscritorio,
                NroAdvogadoEscritorio = dto.NroAdvogadoEscritorio,
                DddAdvogadoAcompanhante = dto.DddAdvogadoAcompanhante,
                NroAdvogadoAcompanhante = dto.NroAdvogadoAcompanhante,
                Migrado = dto.Migrado
            };
        }
    }
}
