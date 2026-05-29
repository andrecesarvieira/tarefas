using FluentAssertions;
using GerenciadorTarefas.Domain.Tarefas.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GerenciadorTarefas.Tests
{
    public class PrazoTests
    {
        [Fact]
        public void CriarPrazo_ComDataValida_DeveCriarComSucesso()
        {
            var dataFutura = DateTime.UtcNow.AddDays(7);
            var prazo = new Prazo(dataFutura);

            prazo.Data.Should().Be(dataFutura);
        }

        [Fact]
        public void CriarPrazo_ComDataPassada_DeveLancarExcecao()
        {
            var dataPassada = DateTime.UtcNow.AddDays(-7);

            var action = () => new Prazo(dataPassada);
            action.Should().Throw<ArgumentException>();
        }
    }
}
