using AutoMapper;
using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.ViewModel;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class ProcessoViewModel : BaseViewModel<long>
    {
        public AbstractValidator<ProcessoViewModel> Validator => new ProcessoValidator();

        public long CodigoTipoProcesso { get; set; }
        public string NumeroProcessoCartorio { get; set; }
        public long CodigoComarca { get; set; }
        public long CodigoVara { get; set; }
        public long CodigoTipoVara { get; set; }
        public long CodigoProfissonal { get; set; }
        public long CodigoParteEmpresa { get; set; }
        public long CodigoEstabelecimento { get; set; }
        public long CodigoParteOrgao { get; set; }



        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<ProcessoViewModel, Processo>();

            mapper.CreateMap<Processo, ProcessoViewModel>();

        }

        internal class ProcessoValidator : AbstractValidator<ProcessoViewModel>
        {
            public ProcessoValidator()
            {
            
            }
        }
    }
}
