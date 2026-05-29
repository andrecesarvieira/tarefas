using FluentAssertions;
using GerenciadorTarefas.Domain.Tarefas.Entities;
using GerenciadorTarefas.Domain.Tarefas.Repositories;
using GerenciadorTarefas.Domain.Tarefas.Services;
using GerenciadorTarefas.Domain.Tarefas.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GerenciadorTarefas.Tests
{
    public class ServicoDeConclusaoTests
    {
        [Fact]
        public async Task ConcluirTarefa_ComTarefaValida_DeveConcluirComSucessoAsync()
        {
            var tarefaId = Guid.NewGuid();

            var titulo = "Estudar";
            var descricao = "DDD";
            var usuarioId = Guid.NewGuid();
            var prazo = new Prazo(DateTime.UtcNow.AddDays(7));
            var prioridade = new Prioridade(Prioridade.NivelPrioridade.Alta);

            var tarefa = new Tarefa(titulo, descricao, usuarioId, prazo, prioridade);

            var mockRepositorio = new Mock<ITarefaRepository>();
            mockRepositorio.Setup(r => r.ObterPorId(tarefaId)).ReturnsAsync(tarefa);

            var servico = new ServicoDeConclusao(mockRepositorio.Object);

            await servico.ConcluirTarefa(tarefaId);

            tarefa.Concluida.Should().BeTrue();
            mockRepositorio.Verify(r => r.Atualizar(tarefa), Times.Once);
        }

        [Fact]
        public async Task ConcluirTarefa_ComTarefaJaConcluida_DeveLancarExcecao()
        {
            var tarefaId = Guid.NewGuid();
            var tarefa = new Tarefa("Estudar", "DDD", Guid.NewGuid(),
                                    new Prazo(DateTime.UtcNow.AddDays(7)),
                                    new Prioridade(Prioridade.NivelPrioridade.Alta));

            tarefa.Concluir();

            var mockRepositorio = new Mock<ITarefaRepository>();
            mockRepositorio.Setup(r => r.ObterPorId(tarefaId)).ReturnsAsync(tarefa);

            var servico = new ServicoDeConclusao(mockRepositorio.Object);

            var action = () => servico.ConcluirTarefa(tarefaId);
            await action.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
