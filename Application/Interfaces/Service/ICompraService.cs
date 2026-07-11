using FCG.CatalogAPI.Application.DTOs;

namespace FCG.CatalogAPI.Application.Interfaces.Service
{
    public interface ICompraService
    {
        Task EfetuarCompra(CompraInput input);
    }
}
