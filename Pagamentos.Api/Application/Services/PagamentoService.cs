using Fcg.Common.Enums;
using Fcg.Common.Extensions;
using Fcg.Common.Messaging.Abstractions;
using Fcg.Common.Messaging.RabbitMQ;
using Fcg.Common.Middleware.Exceptions;
using Microsoft.Extensions.Options;
using Pagamentos.Api.Application.Constants;
using Pagamentos.Api.Application.DTOs;
using Pagamentos.Api.Application.Mappers;
using Pagamentos.Api.Domain.Entities;
using Pagamentos.Api.Domain.Events;
using Pagamentos.Api.Domain.Interfaces;

namespace Pagamentos.Api.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<PagamentoService> _logger;
        private readonly RabbitMqSettings _settings;

        public PagamentoService(
            IPagamentoRepository pagamentoRepository,
            IEventPublisher eventPublisher,
            IOptions<RabbitMqSettings> options,
            ILogger<PagamentoService> logger)
        {
            _pagamentoRepository = pagamentoRepository;
            _eventPublisher = eventPublisher;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task<PagamentoResponseDto> ObterPagamento(Guid pedidoId)
        {
            var pagamento = await _pagamentoRepository.ObterPorId(pedidoId);

            if (pagamento == null)
                throw new KeyNotFoundException("Pagamento não encontrado com o pedido informado");

            return pagamento.ToDto();
        }

        public async Task<IEnumerable<PagamentoResponseDto>> ObterPagamentos()
        {
            var pagamentos = await _pagamentoRepository.ObterTodos();
            return pagamentos.Select(p => p.ToDto());
        }

        public async Task EfetuarPagamento(PagamentoDto pagamentoDto)
            => await ProcessarPagamento(pagamentoDto, _pagamentoRepository.Adicionar);

        #region Métodos Privados
        private async Task ProcessarPagamento(PagamentoDto pagamentoDto, Func<Pagamento, Task> operacao)
        {
            var pagamento = pagamentoDto.ToDomain();

            if (await _pagamentoRepository.Existe(pagamento.Id))
                throw new ConflitoException("Já existe um pagamento para este pedido.");

            await operacao(pagamento);

            try
            {
                var fila = ObterFilaParaProduto(pagamentoDto.Produto);
                await _eventPublisher.PublishAsync(new PagamentoRealizadoEvent
                {
                    PedidoId = pagamento.PedidoId,
                    ValorPago = pagamento.ValorPago,
                    FormaPagamento = pagamento.FormaPagamento,
                    DataPagamento = pagamento.DataCadastro,
                    ClienteNome = pagamentoDto.ClienteNome,
                    ClienteEmail = pagamentoDto.ClienteEmail
                }, fila);
            }
            catch (Exception ex)
            {
                _logger.LogService(
                    ServiceConstants.ServiceName,
                    "ProcessarPagamento",
                    "Erro",
                    $"Erro ao tentar enviar o evento para fila do produto {pagamentoDto.Produto}",
                    new
                    {
                        PedidoId = pagamento.PedidoId,
                        ValorPago = pagamento.ValorPago,
                        FormaPagamento = pagamento.FormaPagamento,
                        DataPagamento = pagamento.DataCadastro
                    },
                    ex);
            }
        }

        private string ObterFilaParaProduto(Produto produto)
        {
            return produto switch
            {
                Produto.Jogo => _settings.Queues["FilaJogo"],
                Produto.Manual => _settings.Queues["FilaManual"],
                Produto.Credito => _settings.Queues["FilaCredito"],
                _ => throw new InvalidOperationException($"Produto não suportado: {produto}")
            };
        }
        #endregion
    }
}