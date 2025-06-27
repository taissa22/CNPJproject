using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface ICotacaoService
    {
        CommandResult Criar(CriarCotacaoCommand command);

        CommandResult Atualizar(AtualizarCotacaoCommand command);

        CommandResult Remover(int CodigoIndice, DateTime dataCotacao);
    }
}