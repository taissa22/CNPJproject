using Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.Impl
{
    public class PerfilAppService : IPerfilAppService
    {
        private readonly IPerfilService service;

        public PerfilAppService(IPerfilService service)
        {
            this.service = service;
        }

        public async Task<PerfilDTO> ObterDetalhePerfil(string codigoPerfil)
        {
            return await service.ObterDetalhePerfil(codigoPerfil);
        }

        public async Task<IList<PerfilDTO>> ListarGestoresDefault()
        {
            return await service.ObterGestoresAprovadoresAtivos();
        }

        public async Task Atualizar(PerfilDTO perfil)
        {
            await service.AtualizarPerfil(perfil);
        }

        public async Task<PerfilDTO>Criar()
        {
            return await service.CriarNovoUsuarioPerfil();
        }

        public async Task Salvar(PerfilDTO perfilDTO)
        {
            await service.SalvarUsuarioPerfil(perfilDTO);
        }

        public async Task<byte[]> Exportar(string id, bool exportarPermissao)
        {
            StringBuilder csv = new StringBuilder();

            var perfil = await service.ObterDetalhePerfil(id);

            if (!exportarPermissao)
            {
                var usuariossPerfil = await service.ObterUsuariosAssociadasAoPerfil(id);               

                csv.AppendLine($">>> USUÁRIOS ASSOCIADOS AO PERFIL {perfil.NomeId.ToUpper()}");
                csv.AppendLine();
                csv.AppendLine($"Nome; Login; Email; Ativo; Último Acesso;");
                if (usuariossPerfil.Count() > 0)
                {
                    foreach (var usuario in usuariossPerfil)
                    {
                        csv.Append($"\"{usuario.Nome}\";");
                        csv.Append($"\"{usuario.Codigo}\";");                        
                        csv.Append($"\"{usuario.Email}\";");
                        csv.Append($"\"{(usuario.Ativo ? "Sim" : "Não")}\";");
                        csv.Append($"\"{(usuario.DataUltimoAcesso.HasValue ? usuario.DataUltimoAcesso.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty)}\";");
                        csv.AppendLine(string.Empty);
                    }
                }

                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return bytes;
            }
            else
            {
                var permissoesPerfil = await service.ObterPermissoesAssociadasAoPerfil(id);

                csv.AppendLine($">>> PERMISSÕES ASSOCIADAS AO PERFIL {perfil.NomeId.ToUpper()}");
                csv.AppendLine();
                csv.AppendLine($"Descrição; Caminho; Código;");
                if (permissoesPerfil.Count() > 0)
                {
                    foreach (var permissao in permissoesPerfil.Select(x => new { x.Descricao, x.Caminho, x.CodigoMenu }).Distinct())
                    {
                        csv.Append($"\"{permissao.Descricao}\";");
                        csv.Append($"\"{permissao.Caminho}\";");
                        csv.Append($"\"{permissao.CodigoMenu}\"");
                        csv.AppendLine(string.Empty);

                    }
                }

                byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return bytes;
            }
        }
    }
}