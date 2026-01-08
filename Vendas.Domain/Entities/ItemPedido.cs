using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Entities
{
    public sealed class ItemPedido: Entity
    {
        //Propriedades do dominio
        public Guid ProdutoId { get; private set; }
        public string NomeProduto { get; private set; } = string.Empty;
        public decimal PrecoUnitario { get; private set; }
        public int Quantidade { get; private set; }
        public decimal DescontoAplicado { get; private set; }
        public decimal ValorTotal { get; private set; }

        //Construtor
        internal ItemPedido(Guid produtoId, string nomeProduto, decimal precoUnitario, int quantidade, decimal descontoAplicado=0m)
        {
            Guard.AgainstEmptyGuid(produtoId, nameof(produtoId));
            Guard.AgainstNullOrWhiteSpace(nomeProduto, nameof(nomeProduto));
            Guard.Against<DomainException>(precoUnitario <= 0, "O preço unitário deve ser maior que zero.");
            Guard.Against<DomainException>(quantidade <= 0, "A quantidade deve ser maior que zero.");

            ProdutoId = produtoId;
            NomeProduto = nomeProduto;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;
            DescontoAplicado = descontoAplicado;
            CalcularValorTotal();
        }
        //Metodo para aplicar desconto
        public void AplicarDesconto(decimal desconto)
        {
            Guard.Against<DomainException>(desconto < 0, "O desconto não pode ser negativo.");
            Guard.Against<DomainException>(desconto > (PrecoUnitario * Quantidade), "O desconto não pode ser maior que o valor total do item.");

            DescontoAplicado = desconto;
            UpdateTimestamp();
            CalcularValorTotal();
        }   
        //Metodo para adicionar unidades
        public void AdicionarUnidades(int unidades)
        {
            Guard.Against<DomainException>(unidades <= 0, "Deve-se adicionar pelo menos uma unidade.");
            Quantidade += unidades;
            UpdateTimestamp();
            CalcularValorTotal();
        }
        //Metodo para remover unidades
        public void RemoverUnidades(int unidades)
        {
            Guard.Against<DomainException>(unidades <= 0, "Deve-se remover pelo menos uma unidade.");
            Guard.Against<DomainException>(unidades > Quantidade, "Não é possível remover mais unidades do que as existentes no item.");
            Quantidade -= unidades;
            Guard.Against<DomainException>(Quantidade == 0, "O item do pedido nao deve ter quantidade zero. " + 
                                                            "Use o metodo da classe Pedido para removê-lo");
            UpdateTimestamp();
            CalcularValorTotal();
        }
        //Metodo para alterar o preco unitario
        public void AlterarPrecoUnitario(decimal novoPreco)
        {
            Guard.Against<DomainException>(novoPreco <= 0, "O preço unitário deve ser maior que zero.");
            PrecoUnitario = novoPreco;
            UpdateTimestamp();
            CalcularValorTotal();
        }
        //Método para calcular o valor total do item do pedido
        private void CalcularValorTotal()
        {
            ValorTotal = (PrecoUnitario * Quantidade) - DescontoAplicado;
        }

    }
}
