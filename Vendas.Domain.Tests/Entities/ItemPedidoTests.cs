using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Entities;

namespace Vendas.Domain.Tests.Entities
{
    public class ItemPedidoTests
    {
        //Metodo Auxiliar para criar ItemPedido
        private static ItemPedido CriarItemValido(decimal preco = 100m, int quantidade = 2)
        {
            return new ItemPedido(
                produtoId: Guid.NewGuid(),
                nomeProduto: "Produto Teste",
                precoUnitario: preco,
                quantidade: quantidade,
                descontoAplicado: 0m
            );
        }
        // Teste para criar ItemPedido valido
        [Fact(DisplayName = "Deve criar ItemPedido com sucesso quando dados são válidos")]
        public void Criar_DeveRetornarItemPedido_QuandoDadosValidos()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var nomeProduto = "Teclado Mecânico";
            var precoUnitario = 150m;
            var quantidade = 3;
            var descontoAplicado = 0m;
            // Act
            var itemPedido = new ItemPedido(produtoId, nomeProduto, precoUnitario, quantidade, descontoAplicado);
            // Assert
            itemPedido.ProdutoId.Should().Be(produtoId);
            itemPedido.NomeProduto.Should().Be(nomeProduto);
            itemPedido.PrecoUnitario.Should().Be(precoUnitario);
            itemPedido.Quantidade.Should().Be(quantidade);
            itemPedido.DescontoAplicado.Should().Be(descontoAplicado);
            itemPedido.ValorTotal.Should().Be(450m);
        }
        //Teste para criar ItemPedido com parametros inválidos
        [Theory(DisplayName = "Deve lançar DomainException quando parametros são invalidos")]
        [InlineData("", "Mouse Gamer", 10, 1, 0, "produtoId não pode ser Guid. Empty.")]
        [InlineData("guid", "", 10, 1, 0, "nomeProduto não pode ser nulo ou vazio.")]
        [InlineData("guid", "Monitor", 0, 1, 0, "O preço unitário deve ser maior que zero.")]
        [InlineData("guid", "Cadeira", 10, 0, 0, "A quantidade deve ser maior que zero.")]
        public void Criar_DeveLancarDomainException_QuandoParametrosInvalidos(
            string produtoIdStr, string nomeProduto, decimal preco, int qtd, decimal descontoAplicado, string mensagem)
        {
            // Arrange
            Guid produtoId = produtoIdStr == "guid" ? Guid.NewGuid() : Guid.Empty;
            // Act
            Action act = () => new ItemPedido(produtoId, nomeProduto, preco, qtd, descontoAplicado);
            // Assert
            act.Should().Throw<DomainException>().WithMessage(mensagem);
        }
    }
}
