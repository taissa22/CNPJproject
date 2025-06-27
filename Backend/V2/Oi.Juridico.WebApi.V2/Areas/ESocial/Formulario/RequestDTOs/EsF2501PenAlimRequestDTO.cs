using DocumentFormat.OpenXml.Spreadsheet;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Extensions;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501PenAlimRequestDTO
    {
        public byte? PenalimTprend { get; set; }
        public string? PenalimCpfdep { get; set; }
        public decimal? PenalimVlrpensao { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            if (PenalimTprend.HasValue && (string.IsNullOrEmpty(PenalimCpfdep) || !PenalimVlrpensao.HasValue) ||
                !string.IsNullOrEmpty(PenalimCpfdep) && (!PenalimTprend.HasValue || !PenalimVlrpensao.HasValue) ||
                PenalimVlrpensao.HasValue && (string.IsNullOrEmpty(PenalimCpfdep) || !PenalimTprend.HasValue))
            {
                mensagensErro.Add("Todos os campos da pensão alimenticia devem ser preenchidos.");
            }

            if (!string.IsNullOrEmpty(PenalimCpfdep))
            {
                if (!PenalimCpfdep.CPFValido())
                {
                    mensagensErro.Add("CPF inválido.");
                }
            }

            if (PenalimVlrpensao.HasValue && PenalimVlrpensao == 0)
            {
                mensagensErro.Add("Valor da pensão deve ser maior que 0 (zero).");
            }

            if (PenalimTprend.HasValue && !EnumExtension.ToList<ESocialTipoRendimento>().Any(x => x.ToByte() == PenalimTprend))
            {
                mensagensErro.Add("Tipo Rendimento inválido.");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
