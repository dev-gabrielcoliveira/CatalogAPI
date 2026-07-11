using FCG.CatalogAPI.Application.DTOs;
using FCG.CatalogAPI.Application.Events;
using FCG.CatalogAPI.Application.Interfaces.Service;
using FCG.CatalogAPI.Application.Service;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.CatalogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprasController : ControllerBase
    {
        private readonly ICompraService _compraService;

        public ComprasController(ICompraService compraService)
        {
            _compraService = compraService;
        }

        
        [HttpPost]
        [Authorize(Policy = "Usuario")]
        public async Task<IActionResult> EfetuarCompra([FromBody] CompraInput input)
        {
            await _compraService.EfetuarCompra(input);

            return Accepted();

        }
    }
}
