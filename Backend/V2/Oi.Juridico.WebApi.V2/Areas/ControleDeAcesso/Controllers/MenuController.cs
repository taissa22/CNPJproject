using Microsoft.Extensions.Caching.Memory;
using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Data;
using Oi.Juridico.WebApi.V2.DTOs.ControleDeAcesso.Menu;
using System.Threading;
using Z.EntityFramework.Plus;
using Oi.Juridico.Contextos.V2.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.Json;
using Oracle.ManagedDataAccess.Client;
using Polly;
using System.Data;
using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.ControleDeAcesso.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly ControleDeAcessoContext _db;
        private readonly ILogger<MenuController> _logger;
        private List<MenuItem> _items = new();

        public MenuController(ControleDeAcessoContext db, ILogger<MenuController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpPost("pesquisar")]
        public async Task<GetMenuPermitidoResponse[]> GetMenuPesquisarAsync(CancellationToken ct)
        {
            var permissoes = await _db.ObtemPermissoesAsync(User.Identity!.Name);

            // obtem itens do menu que o usuário possui permissão
            var itens = (await _db.AcaMenu
                .AsNoTracking()
                .Include(x => x.CodModuloNavigation)
                .Where(x => x.CodAplicacao == "JUR" && permissoes.Select(p => p.CodMenu).Contains(x.CodMenu) && x.Menu.Any())
                .SelectMany(x => x.Menu, (am, m) => new
                {
                    m.Nome,
                    Modulos = "", // am.CodModuloNavigation.Select(z => z.DscModulo), // removido o tooltip do pesquisar
                    Path = m.Url.Replace("\\", "/").Replace("\"", "\\\""),
                    am.DscCaminhoMenuTela
                })
                .OrderBy(x => x.Nome)
                .ToListAsync(ct))
                .Select(x => new GetMenuPermitidoResponse(x.Nome, String.Join(" | ", x.Modulos), x.Path, x.DscCaminhoMenuTela))
                .ToArray();

            return itens;
        }

        [HttpGet("breadcrumb")]
        public async Task<ActionResult<string>> GetBreadcrumbAsync(string permissao, CancellationToken ct)
        {
            var breadcrumb = await _db.AcaMenu
                .AsNoTracking()
                .Where(x => x.CodAplicacao == "JUR" && x.CodMenu == permissao)
                .Select(x => x.DscCaminhoMenuTela)
                .DeferredFirstOrDefault()
                .FromCacheAsync(new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove }, ct);

            if (breadcrumb is null)
            {
                return NotFound();
            }
            else
            {
                return breadcrumb;
            }
        }

        [HttpGet("ItemsMenu")]
        public async Task<List<GetItemsMenuResponse>> GetItemsMenuAsync(CancellationToken ct)
        {
            var permissoes = await _db.ObtemPermissoesAsync(User.Identity!.Name);

            // obtém o menu
            var lista1 = await _db.AcaMenu
                .Include(x => x.Menu)
                .AsNoTracking()
                .Where(x => x.CodAplicacao == "JUR" && permissoes.Select(p => p.CodMenu).Contains(x.CodMenu) && x.Menu.Any())
                .SelectMany(x => x.Menu, (am, m) => new Item(m.ParentId, m.Id, m.Icone, m.Nome, m.Ordem, m.Url, am.CodModuloNavigation.Select(z => z.DscModulo)))
                .ToListAsync(ct);

            var lista = new List<Item>();
            int qtdeRegs = 0;

            do
            {
                var ids = lista1
                    .Select(q => q.ParentId)
                    .Where(q => !lista.Select(l => l.ParentId).Contains(q))
                    .ToList();

                lista.AddRange(lista1);

                lista1 = await _db.Menu
                    .AsNoTracking()
                    .Where(x => ids.Contains(x.Id))
                    .Select(x => new Item(x.ParentId, x.Id, x.Icone, x.Nome, x.Ordem, x.Url, Array.Empty<string>()))
                    .ToListAsync(ct);

                qtdeRegs = lista1.Where(x => !lista.Any(q => q.Id == x.Id)).Count();

            } while (qtdeRegs > 0);

            var menu = lista
                .GroupBy(x => new { x.ParentId, x.Id, x.Icone, x.Nome, x.Ordem, x.Url })
                .Select(x => new Itens(x.Key.ParentId, x.Key.Id, x.Key.Icone, x.Key.Nome, x.Key.Ordem, x.Key.Url, x.Select(z => string.Join(" | ", z.Modulos))))
                // ordena por parentId
                .OrderBy(x => x.ParentId)
                // ordena pela ordem
                .ThenBy(x => x.Ordem == null)
                .ThenBy(x => x.Ordem)
                // depois ordena por ordem alfabética
                .ThenBy(x => x.Nome)
                .Distinct()
                .ToArray();

            List<GetItemsMenuResponse> obj = new();

            foreach (var item in menu.Where(x => x.ParentId == null))
            {
                if (menu.Any(x => x.ParentId == item.Id))
                {
                    var i = MontaMenu(item.Id, menu, item.Id == 281); // somente monta o tooltip para a manutenção
                    obj.Add(new GetItemsMenuResponse(item.Id, item.Icone, item.Nome, item.Url?.Replace("\\", "/").Replace("\"", "\\\"") ?? "", "") { SubItens = i.ToArray() });
                }
                else if (!string.IsNullOrEmpty(item.Url))
                {
                    obj.Add(new GetItemsMenuResponse(item.Id, item.Icone, item.Nome, item.Url?.Replace("\\", "/").Replace("\"", "\\\"") ?? "", ""));
                }
            }

            return obj;
        }

        private record Item(int? ParentId, int Id, string Icone, string Nome, short? Ordem, string Url, IEnumerable<string> Modulos);
        private record Itens(int? ParentId, int Id, string Icone, string Nome, short? Ordem, string Url, IEnumerable<string> Modulos);

        private List<GetItemsMenuResponse> MontaMenu(int parentId, Itens[] menu, bool manutencao)
        {
            var obj = new List<GetItemsMenuResponse>();

            foreach (var item in menu.Where(x => x.ParentId == parentId))
            {
                var modulos = item.Modulos.FirstOrDefault() ?? "";
                if (menu.Any(x => x.ParentId == item.Id))
                {
                    var i = MontaMenu(item.Id, menu, manutencao);
                    obj.Add(new GetItemsMenuResponse(item.Id, item.Icone, item.Nome, item.Url?.Replace("\\", "/").Replace("\"", "\\\"") ?? "", manutencao ? modulos : "") { SubItens = i.ToArray() });
                }
                else
                {
                    obj.Add(new GetItemsMenuResponse(item.Id, item.Icone, item.Nome, item.Url?.Replace("\\", "/").Replace("\"", "\\\"") ?? "", manutencao ? modulos : ""));
                }
            }

            return obj;
        }

        private void MontaListaItems(string? parent, List<MenuItem> menuItems)
        {
            foreach (var item in menuItems)
            {
                if (item.MenuItems.Any())
                {
                    // se o menu possuir subnível, chama novamente o método
                    MontaListaItems(parent is not null ? parent : item.Text, item.MenuItems);
                }
                else
                {
                    item.Parent = parent!;
                    _items.Add(item);
                }
            }
        }
    }
}
