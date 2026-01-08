using FluentAssertions;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.ValueObjects;


namespace Vendas.Domain.Tests.ValueObjects
{
    public class EnderecoEntregaTests
    {
        [Fact(DisplayName = "Deve criar um endereço de entrega válido, quando todos os dados forem válidos.")]
        public void Criar_DeveRetornarEnderecoValido_QuandoosDadosForemValidos()
        {
            // Arrange
            var cep = "12345-678";
            var logradouro = "Rua Exemplo";
            var complemento = "Apto 101";
            var bairro = "Bairro Exemplo";
            var estado = "Estado Exemplo";
            var cidade = "Cidade Exemplo";
            var pais = "Pais Exemplo";
            // Act
            var endereco = EnderecoEntrega.Criar(cep, logradouro, complemento, bairro, estado, cidade, pais);
            // Assert
            Assert.Equal(cep, endereco.Cep);
            Assert.Equal(logradouro, endereco.Logradouro);
            Assert.Equal(complemento, endereco.Complemento);
            Assert.Equal(bairro, endereco.Bairro);
            Assert.Equal(estado, endereco.Estado);
            Assert.Equal(cidade, endereco.Cidade);
            Assert.Equal(pais, endereco.Pais);

            endereco.Should().NotBeNull();
            endereco.Cep.Should().Be(cep);
            endereco.Logradouro.Should().Be(logradouro);
            endereco.Complemento.Should().Be(complemento);
            endereco.Bairro.Should().Be(bairro);
            endereco.Estado.Should().Be(estado);
            endereco.Cidade.Should().Be(cidade);
            endereco.Pais.Should().Be(pais);
            endereco.FormatarEndereco().Should().Contain("Rua Exemplo");
        }

        [Theory(DisplayName = "Deve lançar uma exceção DomainException ao tentar criar um endereço de entrega com cep inválido.")]
        [InlineData("12345678")] // Formato de cep sem hífen
        [InlineData("12-345678")] // Formato de cep com hífen em posição incorreta
        [InlineData("ABCDE-678")] // Formato de cep com letras
        public void Criar_DeveLancarDomainException_QuandoCepForInvalido(string cepInvalido)
        {
            // Arrange
            var logradouro = "Rua Exemplo";
            var complemento = "Apto 101";
            var bairro = "Bairro Exemplo";
            var estado = "Estado Exemplo";
            var cidade = "Cidade Exemplo";
            var pais = "Pais Exemplo";
            // Act
            Action act = () => EnderecoEntrega.Criar(cepInvalido, logradouro, complemento, bairro, estado, cidade, pais);
            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Formato de CEP inválido. O formato correto é 00000-000.");
        }
        [Fact(DisplayName = "Dois EnderecosEntrega com mesmos dados devem ser iguais (Value Object)")]
        public void EnderecosDevemSerIguais_QuandoPossuemMesmosValores()
        {
            // Arrange
            var cep = "12345-678";
            var logradouro = "Rua Exemplo";
            var complemento = "Apto 101";
            var bairro = "Bairro Exemplo";
            var estado = "Estado Exemplo";
            var cidade = "Cidade Exemplo";
            var pais = "Pais Exemplo";
            var endereco1 = EnderecoEntrega.Criar(cep, logradouro, complemento, bairro, estado, cidade, pais);
            var endereco2 = EnderecoEntrega.Criar(cep, logradouro, complemento, bairro, estado, cidade, pais);
            // Act & Assert
            endereco1.Should().Be(endereco2);
            (endereco1 == endereco2).Should().BeTrue();
            //endereco1.GetHashCode().Should().Be(endereco2.GetHashCode());
        }
        [Fact(DisplayName = "EnderecosEntrega devem ser diferentes quando algum campo for diferente")]
        public void EnderecosDevemSerDiferentes_QuandoAlgumCampoForDiferente()
        {
            // Arrange
            var cep = "12345-678";
            var logradouro = "Rua Exemplo";
            var complemento = "Apto 101";
            var bairro = "Bairro Exemplo";
            var estado = "Estado Exemplo";
            var cidade = "Cidade Exemplo";
            var pais = "Pais Exemplo";

            var cep1 = "12345-678";
            var logradouro1 = "Rua Exemplo1";
            var complemento1 = "Apto 101";
            var bairro1 = "Bairro Exemplo";
            var estado1 = "Estado Exemplo";
            var cidade1 = "Cidade Exemplo";
            var pais1 = "Pais Exemplo";

            var endereco1 = EnderecoEntrega.Criar(cep, logradouro, complemento, bairro, estado, cidade, pais);
            var endereco2 = EnderecoEntrega.Criar(cep1, logradouro1, complemento1, bairro1, estado1, cidade1, pais1);
            // Act & Assert
            endereco1.Should().NotBe(endereco2);   
        }
        [Fact(DisplayName = "EnderecoEntrega deve ser imutável após criação")]
        public void EnderecoDeveSerImutavel_AposCriacao()
        {
            //Arrange
            var cep = "12345-678";
            var logradouro = "Rua Exemplo";
            var complemento = "Apto 101";
            var bairro = "Bairro Exemplo";
            var estado = "Estado Exemplo";
            var cidade = "Cidade Exemplo";
            var pais = "Pais Exemplo";
            var endereco = EnderecoEntrega.Criar(cep, logradouro, complemento, bairro, estado, cidade, pais);

            //Act
            Action act = () =>
            {
                //Tentativa hipotética (não compila, apenas conceitual)
                // endereco.Cep = "99999-999";
            };
            //Assert
            endereco.GetType().GetProperties()
                .All(p => p.SetMethod == null || p.SetMethod.IsPrivate)
                .Should().BeTrue("as propriedades do VO devem ser imutáveis");
        }

        [Theory(DisplayName = "Deve lançar uma exceção DomainException quando campos obrigatórios forem nulos ou vazio.")]
        [InlineData(null, "Logradouro", "Bairro", "Estado", "Cidade", "Pais")] // Cep nulo
        [InlineData("12345-678", null, "Bairro", "Estado", "Cidade", "Pais")] // Logradouro  nulo
        [InlineData("12345-678", "Logradouro", "Bairro", "Estado", "Cidade", null)] // Pais nulo
        public void Criar_DeveLancarDomainException_QuandoCamposObrigatoriosNulosOuVazios(
            string cep, string logradouro, string bairro, string estado, string cidade, string pais)
        {
            // Act
            Action act = () => EnderecoEntrega.Criar(cep, logradouro, "Complemento", bairro, estado, cidade, pais);
            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("*não pode ser nulo ou vazio*");
        }
    }
}
