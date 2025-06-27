using System.Text.Json.Serialization;

namespace Oi.Juridico.WebApi.V2.DTOs.ControleDeAcesso.Menu
{
    public record GetItemsMenuResponse(int idMenu, string Icone, string Nome, string Url, string Modulos)
    {
        public GetItemsMenuResponse[]? SubItens { get; set; }
    }
}
