using Flunt.Notifications;
using Perlink.Oi.Juridico.Application.Manutencao.Adapters;
using Perlink.Oi.Juridico.Application.Manutencao.Inputs.BaseCalculos;
using Perlink.Oi.Juridico.Application.Manutencao.Result.BaseCalculos;
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
    public class BaseCalculoCommandHandler : Notifiable, ICommandHandler<RegistrarBaseCalculoCommand>,
                                                         ICommandHandler<AtualizarBaseCalculoCommand>,
                                                         ICommandHandler<ExcluirBaseCalculoCommand>,
                                                         ICommandHandler<PesquisarBaseCalculoCommand>,
                                                         ICommandHandler<ExportarBaseCalculoCommand>
    { 
        private readonly IUow _uow;
        private readonly IBaseCalculoRepository _baseCalculoRepository;
        private readonly IBaseCalculoAdoRepository _baseCalculoAdoRepository;
        private readonly IBaseCalculoComposer _baseCalculoComposer;

        public BaseCalculoCommandHandler(IUow uow, IBaseCalculoRepository baseCalculoRepository,
                                         IBaseCalculoAdoRepository baseCalculoAdoRepository, 
                                         IBaseCalculoComposer baseCalculoComposer)
        {
            _uow = uow;
            _baseCalculoRepository = baseCalculoRepository;
            _baseCalculoAdoRepository = baseCalculoAdoRepository;
            _baseCalculoComposer = baseCalculoComposer;
        }

        public ICommandResult Handle(PesquisarBaseCalculoCommand command)
        {
            var sortOrders = new List<SortOrder> { new SortOrder { Property = command.Propriedade, Direction = command.Direcao } };

            var listaFiltrada = _baseCalculoAdoRepository.ObterTodosPorFiltro(command.Descricao, command.PageNumber,
                                                                              command.PageSize, sortOrders, command.IsExportMethod)
                                                         .Select(dto => BaseCalculoAdapter.ToCommandResult(dto));

            var totalElementos = _baseCalculoAdoRepository.GetTotalCount(command.Descricao);

            return new PesquisarBaseCalculoCommandResult(listaFiltrada, totalElementos);
        }

        public ICommandResult Handle(RegistrarBaseCalculoCommand command)
        {
            if (command == null)
            {
                AddNotification("BaseCalculo.CommandoInvalido", "O comando é inválido.");
                return null;
            }

            var baseCalculo = _baseCalculoComposer.Create(command.Descricao);

            AddNotifications(baseCalculo.Notifications);

            if (Invalid) return null;

            _baseCalculoRepository.Save(baseCalculo);
            _uow.Commit();

            return new BaseCalculoCommandResult(baseCalculo.Id, baseCalculo.Descricao, baseCalculo.EhBaseInicial);
        }

        public ICommandResult Handle(AtualizarBaseCalculoCommand command)
        {
            if (command == null)
            {
                AddNotification("BaseCalculo.CommandoInvalido", "O comando é inválido.");
                return null;
            }

            var baseCalculo = _baseCalculoRepository.Get(command.CodigoBaseCalculo.Value);

            if (baseCalculo == null)
            {
                AddNotification("BaseCalculo.NaoExiste", "Base de Cálculo não foi encontrado.");
                return null;
            }

            var podeRemoverBaseInicial = baseCalculo.PodeMarcarBaseInicial(command.ehBaseInicial);

            if (podeRemoverBaseInicial)
            {
                var baseCalculoComBaseInicialAnterior = _baseCalculoRepository.ObterPorBaseInicial();

                if (baseCalculoComBaseInicialAnterior == null)
                {
                    AddNotification("BaseCalculo.NaoEncontrado", "Erro ao buscar a base de calculo com calculo inicial anterior.");
                    return null;
                }

                baseCalculoComBaseInicialAnterior.DesmarcarBaseInicial();
                _baseCalculoRepository.Update(baseCalculoComBaseInicialAnterior);
            }

            baseCalculo.Atualizar(command.Descricao);

            AddNotifications(baseCalculo.Notifications);

            if (Invalid) return null;

            _baseCalculoRepository.Update(baseCalculo);
            _uow.Commit();

            return new BaseCalculoCommandResult(baseCalculo.Id, baseCalculo.Descricao, baseCalculo.EhBaseInicial);
        }

        public ICommandResult Handle(ExcluirBaseCalculoCommand command)
        {
            if (command == null)
            {
                AddNotification("BaseCalculo.CommandoInvalido", "O comando é inválido.");
                return null;
            }

            var baseCalculo = _baseCalculoRepository.Get(command.CodigoBaseCalculo);

            if (baseCalculo == null)
            {
                AddNotification("BaseCalculo.NaoExiste", "Base de Cálculo não foi encontrado.");
                return null;
            }

            if (baseCalculo.EhBaseInicial)
            {
                AddNotification("BaseCalculo.NaoExcluirBaseInicial", "Não é possivel excluir uma Base de Cálculo Inicial.");
            }

            _baseCalculoRepository.Remove(baseCalculo);
            _uow.Commit();

            return new BaseCalculoCommandResult(baseCalculo.Id, baseCalculo.Descricao, baseCalculo.EhBaseInicial);
        }

        public ICommandResult Handle(ExportarBaseCalculoCommand command)
        {
            if (command == null)
            {
                AddNotification("BaseCalculo.CommandoInvalido", "O comando é inválido.");
                return null;
            }

            var sortOrders = new List<SortOrder> { new SortOrder { Property = command.Propriedade, Direction = command.Direcao } };

            var listaFiltrada = _baseCalculoAdoRepository.ObterTodosPorFiltro(command.Descricao, 0, 0, sortOrders, command.IsExportMethod)
                                                         .Select(dto => BaseCalculoAdapter.ToCommandResult(dto));

            return new ExportarBaseCalculoCommandResult(listaFiltrada);
        }
    }
}
