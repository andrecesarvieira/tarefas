using FluentAssertions;
using GerenciadorTarefas.Domain.Tarefas.Entities;
using GerenciadorTarefas.Domain.Tarefas.ValueObjects;
using Xunit;

namespace GerenciadorTarefas.Tests
{
    public class TarefaTests
    {
        [Fact]
        public void CriarTarefa_ComTituloValido_DeveCriarTarefa()
        {
            var titulo = "Comprar leite";
            var descricao = "Comprar leite no supermercado";
            var usuarioId = Guid.NewGuid();
            var prazo = new Prazo(DateTime.UtcNow.AddDays(7));
            var prioridade = new Prioridade(Prioridade.NivelPrioridade.Alta);

            var tarefa = new Tarefa(titulo, descricao, usuarioId, prazo, prioridade);

            tarefa.Titulo.Should().Be(titulo);
        }
        
        [Fact]
        public void CriarTarefa_ComTituloInvalido_DeveLancarExcecao()
        {
            var titulo = "";
            var descricao = "Comprar leite no supermercado";
            var usuarioId = Guid.NewGuid();
            var prazo = new Prazo(DateTime.UtcNow.AddDays(7));
            var prioridade = new Prioridade(Prioridade.NivelPrioridade.Alta);

            Assert.Throws<ArgumentException>(() => new Tarefa(titulo, descricao, usuarioId, prazo, prioridade));
        }

        [Fact]
        public void CriarTarefa_ComPrazoFuturo_DeveCriarTarefa()
        {
            var titulo = "Comprar leite";
            var descricao = "Comprar leite no supermercado";
            var usuarioId = Guid.NewGuid();
            var prazo = new Prazo(DateTime.UtcNow.AddDays(7));
            var prioridade = new Prioridade(Prioridade.NivelPrioridade.Alta);

            var tarefa = new Tarefa(titulo, descricao, usuarioId, prazo, prioridade);

            tarefa.Prazo.Should().Be(prazo);
        }

        [Fact]
        public void CriarTarefa_ComPrazoPassado_DeveLancarExcecao()
        {
            var usuarioId = Guid.NewGuid();
            var dataPassada = DateTime.UtcNow.AddDays(-7);

            var action = () => new Prazo(dataPassada);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CriarTarefa_ComUsuarioVazio_DeveLancarExcecao()
        {
            var titulo = "Comprar leite";
            var descricao = "Comprar leite no supermercado";
            Guid? usuarioId = null;
            var prazo = new Prazo(DateTime.UtcNow.AddDays(7));
            var prioridade = new Prioridade(Prioridade.NivelPrioridade.Alta);

            Assert.Throws<ArgumentException>(() => new Tarefa(titulo, descricao, usuarioId ?? Guid.Empty, prazo, prioridade));
        }
    }
}
