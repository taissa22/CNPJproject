using AutoMapper;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.DTO;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Shared.Application.ViewModel;
using System;
using System.Collections.Generic;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;

namespace Perlink.Oi.Juridico.Application.GrupoDeEstados.ViewModel
{
    public class GrupoEstadosViewModel : BaseViewModel<long>
    {
        public string Nome { get; set; }
        public string NomeAnterior { get; set; }
        public bool Persistido { get; set; }
        public int EstadosIniciais { get; set; }
        public IEnumerable<EstadoViewModel> EstadosGrupo { get; set; }
    }
}
