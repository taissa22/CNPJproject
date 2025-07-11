﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.SolicitacaoAcessoContext.Entities
{
    public partial class SolAprovacaoPerfil
    {
        public decimal Id { get; set; }
        public decimal? IdSolicitacaoAcesso { get; set; }
        public string CodUsuarioSolicitante { get; set; }
        public string CodPerfil { get; set; }
        public string IndAprovacaoAdministrador { get; set; }
        public string CodUsuarioAdministrador { get; set; }
        public DateTime? DatAcaoAdministrador { get; set; }
        public string IndAprovacaoGestor { get; set; }
        public string CodUsuarioGestor { get; set; }
        public DateTime? DatAcaoGestor { get; set; }

        public virtual AcaUsuario CodUsuarioAdministradorNavigation { get; set; }
        public virtual AcaUsuario CodUsuarioGestorNavigation { get; set; }
        public virtual AcaUsuario CodUsuarioSolicitanteNavigation { get; set; }
        public virtual AcaSolicitacaoAcesso IdSolicitacaoAcessoNavigation { get; set; }
    }
}