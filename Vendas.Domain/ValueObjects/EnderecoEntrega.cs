using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.ValueObjects;
public class EnderecoEntrega : ValueObject
{
    public string Cep { get; private set; }
    public string Logradouro { get; private set; }
    public string Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Estado { get; private set; }
    public string Cidade { get; private set; }
    public string Pais { get; private set; }

    private EnderecoEntrega(string cep, string logradouro, string complemento,
                            string bairro, string estado, string cidade,
                            string pais) 
    {
        Guard.AgainstNullOrWhiteSpace(cep, nameof(cep));
        Guard.AgainstNullOrWhiteSpace(logradouro, nameof(logradouro));
        Guard.AgainstNullOrWhiteSpace(bairro, nameof(bairro));
        Guard.AgainstNullOrWhiteSpace(estado, nameof(estado));
        Guard.AgainstNullOrWhiteSpace(cidade, nameof(cidade));
        Guard.AgainstNullOrWhiteSpace(pais, nameof(pais));

        //Validação de formato para CEP pode ser adicionada aqui
        //Exemplo: Guard.AgainstInvalidCepFormat(cep, nameof(cep));
        if(!Regex.IsMatch(cep, @"^\d{5}-\d{3}$"))
           throw new ArgumentException("Formato de CEP inválido. O formato correto é 00000-000.", nameof(cep));

        Cep = cep;
        Logradouro = logradouro;
        Complemento = complemento ?? string.Empty; // Complemento é opcional
        Bairro = bairro;
        Estado = estado;
        Cidade = cidade;
        Pais = pais;
    }
    public static EnderecoEntrega Criar(string cep, string logradouro, string complemento,
                                         string bairro, string estado, string cidade,
                                         string pais)
    {
        return new EnderecoEntrega(cep, logradouro, complemento, bairro, estado, cidade, pais);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Cep;
        yield return Logradouro;
        yield return Complemento ?? string.Empty;
        yield return Bairro;
        yield return Estado;
        yield return Cidade;
        yield return Pais;
    }
    public string FormatarCompleto()
    {
        return $"{Logradouro}, {Complemento} - {Bairro}, {Cidade} - {Estado}, {Pais} - CEP: {Cep}";
    }
}
