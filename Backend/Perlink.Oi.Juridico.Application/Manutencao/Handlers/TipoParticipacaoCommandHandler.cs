using Flunt.Notifications;
using Perlink.Oi.Juridico.Application.Manutencao.Adapters;
using Perlink.Oi.Juridico.Application.Manutencao.Inputs.TiposParticipacao;
using Perlink.Oi.Juridico.Application.Manutencao.Results.TiposParticipacao;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface;
using Perlink.Oi.Juridico.Domain.Manutencao.Composer.Interfaces;
using Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.AdoRepositories;
using Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.EFRepositories;
using Shared.Domain;
using Shared.Domain.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Handlers
{
    public class TipoParticipacaoCommandHandler : Notifiable, ICommandHandler<RegistrarTipoParticipacaoCommand>,
                                                              ICommandHandler<PesquisarTipoParticipacaoCommand>,
                                                              ICommandHandler<ExportarTipoParticipacaoCommand>
    {
        private readonly IUow _uow;
        private readonly ITipoParticipacaoRepository _tipoParticipacaoRepository;
        private readonly ITipoParticipacaoAdoRepository _tipoParticipacaoAdoRepository;
        private readonly ITipoParticipacaoComposer _tipoParticipacaoComposer;

        public TipoParticipacaoCommandHandler(IUow uow, ITipoParticipacaoRepository tipoParticipacaoRepository,
                                                        ITipoParticipacaoAdoRepository tipoParticipacaoAdoRepository,
                                                        ITipoParticipacaoComposer tipoParticipacaoComposer)
        {
            _uow = uow;

            _tipoParticipacaoRepository = tipoParticipacaoRepository;
            _tipoParticipacaoAdoRepository = tipoParticipacaoAdoRepository;
            _tipoParticipacaoComposer = tipoParticipacaoComposer;
        }

        public ICommandResult Handle(PesquisarTipoParticipacaoCommand command)
        {
            var sortOrders = new List<SortOrder> { new SortOrder { Property = command.Propriedade, Direction = command.Direcao } };

            var listaFiltrada = _tipoParticipacaoAdoRepository.ObterTodosPorFiltro(command.Descricao,
                                                                                   command.PageNumber, command.PageSize,
                                                                                   sortOrders, command.IsExportMethod)
                                                           .Select(dto => TipoParticipacaoAdapter.ToCommandResult(dto));

            var totalElementos = _tipoParticipacaoAdoRepository.GetTotalCount(command.Descricao);

            return new PesquisarTipoParticipacaoCommandResult(listaFiltrada, totalElementos);
        }

        public ICommandResult Handle(RegistrarTipoParticipacaoCommand command)
        {
            if (command == null)
            {
                AddNotification("TipoParticipacao.CommandoInvalido", "O comando é inválido.");
                return null;
            }

            var tipoParticipacao = _tipoParticipacaoComposer.Create(command.Descricao);

            AddNotifications(tipoParticipacao.Notifications);

            if (Invalid) return null;

            _tipoParticipacaoRepository.Save(tipoParticipacao);
            _uow.Commit();

            return new TipoParticipacaoCommandResult(tipoParticipacao.Id, tipoParticipacao.Descricao);
        }

        public ICommandResult Handle(AtualizarTipoParticipacaoCommand command)
        {
            if (command == null)
            {
                AddNotification("TipoParticipacao.CommandoInvalido", "O comando é inválido.");
                return null;
            }

            command.Validate();

            if (command.Invalid)
            {
                AddNotifications(command.Notifications);
                return null;
            }

            var tipoParticipacao = _tipoParticipacaoRepository.Get(command.CodigoTipoParticipacao.Value);

            if (tipoParticipacao == null)
            {
                AddNotification("TipoParticipacao.NaoExiste", "Tipo Participação não foi encontrado.");
                return null;
            }

            tipoParticipacao.Atualizar(command.Descricao);

            AddNotifications(tipoParticipacao.Notifications);

            if (Invalid) return null;

            _tipoParticipacaoRepository.Update(tipoParticipacao);

            _uow.Commit();

            return new TipoParticipacaoCommandResult(tipoParticipacao.Id, tipoParticipacao.Descricao);
        }

        public ICommandResult Handle(ExcluirTipoParticipacaoCommand command)
        {
            if (command == null)
            {
                AddNotification("TipoParticipacao.CommandoInvalido", "O comando é inválido.");
                return null;
            }

            var tipoParticipacao = _tipoParticipacaoRepository.Get(command.CodigoTipoParticipacao);

            if (tipoParticipacao == null)
            {
                AddNotification("tipoparticipacao.NaoExiste", "Tipo Participação não foi encontrado.");
                return null;
            }

            _tipoParticipacaoRepository.Remove(tipoParticipacao);

            _uow.Commit();

            return new TipoParticipacaoCommandResult(tipoParticipacao.Id, tipoParticipacao.Descricao);
        }

        public ICommandResult Handle(ExportarTipoParticipacaoCommand command)
        {
            if (command == null)
            {
                AddNotification("TipoParticipacao.CommandoInvalido", "O comando é inválido.");
                return null;
            }

            var sortOrders = new List<SortOrder> { new SortOrder { Property = command.Propriedade, Direction = command.Direcao } };

            var listaFiltrada = _tipoParticipacaoAdoRepository.ObterTodosPorFiltro(command.Descricao, 0, 0, sortOrders, command.IsExportMethod)
                                                              .Select(dto => TipoParticipacaoAdapter.ToCommandResult(dto));

            return new ExportarTipoParticipacaoCommandResult(listaFiltrada);
        }
    }
}
