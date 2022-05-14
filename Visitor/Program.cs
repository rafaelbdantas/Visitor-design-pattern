using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor
{
  public class Cliente
  {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Endereco { get; set; }
    public DateTime Nascimento { get; set; }
    public string Sexo { get; set; }
    public bool Ativo { get; set; }
    public decimal Renda { get; set; }
  }

  public interface IClienteServico 
  {
    IEnumerable<Cliente> Clientes { get; set; }
    void Inserir(Cliente cliente);
    int Excluir(int Id);
    IEnumerable<Cliente> Listar();
    Cliente Obter(int Id);

    void Visit(IClienteServicoVisitor servicoVisitor);
  }

  public class ClienteServico : IClienteServico
  {
    public IEnumerable<Cliente> Clientes { get; set; }
    public ClienteServico()
    {
      var clienteFaker = new Faker<Cliente>("pt_BR")
                 .RuleFor(c => c.Id, f => f.IndexFaker)
                 .RuleFor(c => c.Nome, f => f.Name.FullName(Bogus.DataSets.Name.Gender.Female))
                 .RuleFor(c => c.Email, f => f.Internet.Email(f.Person.FirstName).ToLower())
                 .RuleFor(c => c.Telefone, f => f.Person.Phone)
                 .RuleFor(c => c.Endereco, f => f.Address.StreetAddress())
                 .RuleFor(c => c.Nascimento, f => f.Date.Recent(100))
                 .RuleFor(c => c.Sexo, f => f.PickRandom(new string[] { "masculino", "feminino" }))
                 .RuleFor(c => c.Ativo, f => f.PickRandomParam(new bool[] { true, true, false }))
                 .RuleFor(o => o.Renda, f => f.Random.Decimal(500, 2000));
      Clientes = clienteFaker.Generate(10);
    }
    public int Excluir(int Id)
    {
      throw new NotImplementedException();
    }

    public void Inserir(Cliente cliente)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Cliente> Listar()
    {
      return Clientes;
    }

    public Cliente Obter(int Id)
    {
      if (Clientes != null && Clientes.Any())
        return Clientes.FirstOrDefault(x => x.Id == Id);
      else return new Cliente();
    }

    public void Visit(IClienteServicoVisitor clienteServico)
    {
      clienteServico.Executar(this);
    }
  }

  public interface IClienteServicoVisitor 
  {
    void Executar(IClienteServico cliente);
  }

  public class EmailClienteServicoVisitor : IClienteServicoVisitor
  {
    public void Executar(IClienteServico clienteServico)
    {
      foreach (var cliente in clienteServico.Clientes)
      {
        var mensagem = $@"Olá senhor(a) {cliente.Nome}. Seja bem-vindo a nossa loja";
        EnviarEmail(cliente.Email, mensagem);
      }      
      //Enviar Email
    }

    public void EnviarEmail(string email, string mensagem)
    {

    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      var servico = new ClienteServico();
      servico.Inserir(new Cliente() { });
      //
      servico.Visit(new EmailClienteServicoVisitor());
    }

  }
}
