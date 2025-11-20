using Fcg.Common.Enums;
using Fcg.Common.Messaging.Abstractions;
using Fcg.Common.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pagamentos.Api.Application.DTOs;
using Pagamentos.Api.Application.Services;
using Pagamentos.Api.Domain.Entities;
using Pagamentos.Api.Domain.Events;
using Pagamentos.Api.Domain.Interfaces;
using Xunit;

namespace Pagamentos.Api.Tests.Services
{
    public class PagamentoServiceTests
    {
        private readonly Mock<IPagamentoRepository> _pagamentoRepositoryMock;
        private readonly Mock<IEventPublisher> _eventPublisherMock;
        private readonly Mock<ILogger<PagamentoService>> _loggerMock;
        private readonly PagamentoService _service;

        public PagamentoServiceTests()
        {
            _pagamentoRepositoryMock = new Mock<IPagamentoRepository>();
            _eventPublisherMock = new Mock<IEventPublisher>();
            _loggerMock = new Mock<ILogger<PagamentoService>>();

            var settings = new ServiceBusSettings
            {
                Queues = new Dictionary<string, string>
                {
                    { "FilaJogo", "pagamento-jogo-realizado" }
                }
            };
            var optionsMock = new Mock<IOptions<ServiceBusSettings>>();
            optionsMock.Setup(o => o.Value).Returns(settings);

            _service = new PagamentoService(
                _pagamentoRepositoryMock.Object,
                _eventPublisherMock.Object,
                optionsMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task ObterPagamento_DeveRetornarPagamento_QuandoExiste()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var pagamento = Pagamento.Criar(pedidoId, 100, FormaPagamento.Cartao);
            _pagamentoRepositoryMock.Setup(r => r.ObterPorId(pedidoId)).ReturnsAsync(pagamento);

            // Act
            var result = await _service.ObterPagamento(pedidoId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pedidoId, result.PedidoId);
            Assert.Equal(100, result.ValorPago);
            Assert.Equal(FormaPagamento.Cartao, result.FormaPagamento);
        }

        [Fact]
        public async Task ObterPagamento_DeveLancarExcecao_QuandoNaoExiste()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            _pagamentoRepositoryMock.Setup(r => r.ObterPorId(pedidoId)).ReturnsAsync((Pagamento)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ObterPagamento(pedidoId));
        }

        [Fact]
        public async Task ObterPagamentos_DeveRetornarTodosPagamentos()
        {
            // Arrange
            var pagamentos = new List<Pagamento>
            {
                Pagamento.Criar(Guid.NewGuid(), 100, FormaPagamento.Cartao),
                Pagamento.Criar(Guid.NewGuid(), 200, FormaPagamento.Pix)
            };
            _pagamentoRepositoryMock.Setup(r => r.ObterTodos()).ReturnsAsync(pagamentos);

            // Act
            var result = await _service.ObterPagamentos();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.ValorPago == 100);
            Assert.Contains(result, r => r.ValorPago == 200);
        }

        [Fact]
        public async Task EfetuarPagamento_DeveAdicionarPagamento_QuandoNaoExiste()
        {
            // Arrange
            var dto = new PagamentoDto
            {
                PedidoId = Guid.NewGuid(),
                ValorPago = 150,
                FormaPagamento = FormaPagamento.Boleto
            };
            var pagamento = Pagamento.Criar(dto.PedidoId, dto.ValorPago, dto.FormaPagamento);

            _pagamentoRepositoryMock.Setup(r => r.Existe(It.IsAny<Guid>())).ReturnsAsync(false);
            _pagamentoRepositoryMock.Setup(r => r.Adicionar(It.IsAny<Pagamento>())).Returns(Task.CompletedTask);
            _eventPublisherMock.Setup(e => e.PublishAsync(It.IsAny<PagamentoRealizadoEvent>(), "pagamento-realizado")).Returns(Task.CompletedTask);

            // Act
            await _service.EfetuarPagamento(dto);

        }
    }
}
