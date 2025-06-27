using Flunt.Notifications;
using Perlink.Oi.Juridico.Application.Manutencao.Adapters;
using Perlink.Oi.Juridico.Application.Manutencao.Inputs.TiposAudiencias;
using Perlink.Oi.Juridico.Application.Manutencao.Results.TiposAudiencias;
using Perlink.Oi.Juridico.Application.Processos.Adapters;
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
    public class TipoAudienciaCommandHandler : Notifiable, ICommandHandler<RegistrarTipoAudienciaCommand>,
                                                           ICommandHandler<AtualizarTipoAudienciaCommand>,
                                                           ICommandHandler<ExcluirTipoAudienciaCommand>,
                                                           ICommandHandler<PesquisarTipoAudienciaCommand>,
                                                           ICommandHandler<ExportarTipoAudienciaCommand>
    {
        private readonly IUow _uow;
        private readonly ITipoAudienciaRepository _tipoAudienciaRepository;
        private readonly ITipoAudienciaAdoRepository _tipoAudienciaAdoRepository;
        private readonly ITipoAudienciaComposer _tipoAudienciaComposer;

        public TipoAudienciaCommandHandler(IUow uow, ITipoAudienciaRepository tipoAudienciaRepository,
                                                     ITipoAudienciaAdoRepository tipoAudienciaAdoRepository,
                                                     ITipoAudienciaComposer tipoAudienciaComposer)
        {
            _uow = uow;

            _tipoAudienciaRepository = tipoAudienciaRepository;
            _tipoAudienciaAdoRepository = tipoAudienciaAdoRepository;
            _tipoAudienciaComposer = tipoAudienciaComposer; 
        }

        public ICommandResult Handle(PesquisarTipoAudienciaCommand command)
        {
            var sortOrders = command.SortOrders.Select(vm => SortOrderAdapter.ToDTO(vm)).ToList();

            var listaFiltrada = _tipoAudienciaAdoRepository.ObterTodosPorFiltro(command.CodTipoProcesso, command.Descricao, 
                                                                                command.PageNumber, command.PageSize, 
                                                                                sortOrders, command.IsExportMethod)
                                                           .Select(dto => TipoAudienciaAdapter.ToCommandResult(dto));

            var totalElementos = _tipoAudienciaAdoRepository.GetTotalCount(command.CodTipoProcesso, command.Descricao);

            return new PesquisarTipoAudienciaCommandResult(listaFiltrada, totalElementos);
        }

        public ICommandResult Handle(RegistrarTipoAudienciaCommand command)
        {
            if (command == null)
            {
                AddNotification("TipoAudiencia.CommandoInvalido", "O comando é inválido.");
                return null;
            }

            var tipoAudiencia = _tipoAudienciaComposer.Create(command.Descricao, command.Sigla, command.TipoProcesso);

            AddNotifications(tipoAudiencia.Notifications);

            if (Invalid) return null;

            _tipoAudienciaRepository.Save(tipoAudiencia);
            _uow.Commit();

            return new TipoAudienciaCommandResult(tipoAudiencia.CodigoTipoAudiencia, tipoAudiencia.Descricao, tipoAudiencia.Sigla,
                                                  tipoAudiencia.EstaAtivo, tipoAudiencia.EhCivelConsumidor,
                                                  tipoAudiencia.EhCivelEstrategico, tipoAudiencia.EhTrabalhista,
                                                  tipoAudiencia.EhTrabalhistaAdmin, tipoAudiencia.EhTributarioAdmin,
                                                  tipoAudiencia.EhTributarioJud, tipoAudiencia.EhJuizado,
                                                  tipoAudiencia.EhAdministrativo, tipoAudiencia.EhCivelAdmin,
                                                  tipoAudiencia.EhCriminalJud, tipoAudiencia.EhCriminalAdmin,
                                                  tipoAudiencia.EhProcon, tipoAudiencia.EhPex, false, null, null,tipoAudiencia.LinkVirtual);
        }

        public ICommandResult Handle(AtualizarTipoAudienciaCommand command)
        {
            if (command == null)
            {
                AddNotification("TipoAudiencia.CommandoInvalido", "O comando é inválido.");
                return null;
            }

            command.Validate();

            if (command.Invalid)
            {
                AddNotifications(command.Notifications);
                return null;
            }

            var tipoAudiencia = _tipoAudienciaRepository.Get(command.CodigoTipoAudiencia.Value);

            if (tipoAudiencia == null)
            {
                AddNotification("TipoAudiencia.NaoExiste", "Tipo audiência não foi encontrado.");
                return null;
            }

            tipoAudiencia.Atualizar(command.Descricao, command.Sigla, command.TipoProcesso, command.EstaAtivo);

            AddNotifications(tipoAudiencia.Notifications);

            if (Invalid) return null;

            _tipoAudienciaRepository.Update(tipoAudiencia);

            _uow.Commit();

            return new TipoAudienciaCommandResult(tipoAudiencia.CodigoTipoAudiencia, tipoAudiencia.Descricao, tipoAudiencia.Sigla,
                                                  tipoAudiencia.EstaAtivo, tipoAudiencia.EhCivelConsumidor,
                                                  tipoAudiencia.EhCivelEstrategico, tipoAudiencia.EhTrabalhista,
                                                  tipoAudiencia.EhTrabalhistaAdmin, tipoAudiencia.EhTributarioAdmin,
                                                  tipoAudiencia.EhTributarioJud, tipoAudiencia.EhJuizado,
                                                  tipoAudiencia.EhAdministrativo, tipoAudiencia.EhCivelAdmin,
                                                  tipoAudiencia.EhCriminalJud, tipoAudiencia.EhCriminalAdmin,
                                                  tipoAudiencia.EhProcon, tipoAudiencia.EhPex, false, null, null, tipoAudiencia.LinkVirtual);
        }

        public ICommandResult Handle(ExcluirTipoAudienciaCommand command)
        {
            if (command == null)
            {
                AddNotification("TipoAudiencia.CommandoInvalido", "O comando é inválido.");
                return null;
            }

            var tipoAudiencia = _tipoAudienciaRepository.Get(command.CodigoTipoAudiencia);

            if (tipoAudiencia == null)
            {
                AddNotification("TipoAudiencia.NaoExiste", "Tipo audiência não foi encontrado.");
                return null;
            }

            _tipoAudienciaRepository.Remove(tipoAudiencia);

            _uow.Commit();

            return new TipoAudienciaCommandResult(tipoAudiencia.CodigoTipoAudiencia, tipoAudiencia.Descricao, tipoAudiencia.Sigla,
                                                  tipoAudiencia.EstaAtivo, tipoAudiencia.EhCivelConsumidor,
                                                  tipoAudiencia.EhCivelEstrategico, tipoAudiencia.EhTrabalhista,
                                                  tipoAudiencia.EhTrabalhistaAdmin, tipoAudiencia.EhTributarioAdmin,
                                                  tipoAudiencia.EhTributarioJud, tipoAudiencia.EhJuizado,
                                                  tipoAudiencia.EhAdministrativo, tipoAudiencia.EhCivelAdmin,
                                                  tipoAudiencia.EhCriminalJud, tipoAudiencia.EhCriminalAdmin,
                                                  tipoAudiencia.EhProcon, tipoAudiencia.EhPex, false, null, null, tipoAudiencia.LinkVirtual);
        }

        public ICommandResult Handle(ExportarTipoAudienciaCommand command)
        {
            if (command == null)
            {
                AddNotification("TipoAudiencia.CommandoInvalido", "O comando é inválido.");
                return null;
            }

            var sortOrders = new List<SortOrder> { new SortOrder { Property = command.Propriedade, Direction = command.Direcao } };

            var listaFiltrada = _tipoAudienciaAdoRepository.ObterTodosPorFiltro(command.CodTipoProcesso, command.Descricao,
                                                                                0, 0, sortOrders, command.IsExportMethod)
                                                           .Select(dto => TipoAudienciaAdapter.ToCommandResult(dto));

            return new ExportarTipoAudienciaCommandResult(listaFiltrada);
        }
    }
}
