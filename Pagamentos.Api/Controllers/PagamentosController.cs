using Fcg.Common.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagamentos.Api.Application.DTOs;
using Pagamentos.Api.Application.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Pagamentos.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagamentosController : MainController
    {
        private readonly IPagamentoService _pagamento;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PagamentosController(IPagamentoService pagamento, IHttpContextAccessor httpContextAccessor)
        {
            _pagamento = pagamento;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("ObterPagamento")]
        [SwaggerOperation(Summary = "Obtém um pagamento pelo ID do pedido", 
                          Description = "Retorna um pagamento específico com base no ID do pedido informado.")]
        public async Task<IActionResult> ObterPagamento(Guid pedidoId)
        {
            var pagamento = await _pagamento.ObterPagamento(pedidoId);
            return CustomResponse(pagamento);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpGet("ObterPagamentos")]
        [SwaggerOperation(Summary = "Obtém todos os pagamentos", 
                          Description = "Retorna uma lista de todos os pagamentos cadastrados.")]
        public async Task<IActionResult> ObterPagamentos()
        {
            var pagamentos = await _pagamento.ObterPagamentos();
            return pagamentos.Any()
                ? CustomResponse(pagamentos)
                : CustomResponse("Nenhum pagamento encontrado", StatusCodes.Status404NotFound);
        }

        [Authorize(Roles = "Usuario, Administrador")]
        [HttpPost("EfetuarPagamento")]
        [SwaggerOperation(Summary = "Efetua pagamentos", 
                          Description = "Permite que usuários efetuem o pagamento de um pedido.")]
        public async Task<IActionResult> EfetuarPagamento(PagamentoDto pagamento)
        {
            PreencherNomeEmailCliente(pagamento);

            await _pagamento.EfetuarPagamento(pagamento);
            return CustomResponse("Pagamento efetuado com sucesso");
        }

        #region Métodos Privados
        private void PreencherNomeEmailCliente(PagamentoDto pagamento)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var nome = user?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
            var email = user?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            pagamento.ClienteNome = nome;
            pagamento.ClienteEmail = email;
        }
        #endregion
    }
}
    