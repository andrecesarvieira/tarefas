using GerenciadorTarefas.Domain.Tarefas.Entities;
using GerenciadorTarefas.Domain.Tarefas.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GerenciadorTarefas.Domain.Tarefas.Factories
{
    public class TarefaFactory
    {
        public static Tarefa CriarTarefa(string titulo, string descricao, Guid usuarioId, DateTime prazo, Prioridade prioridade)
        {
            return new Tarefa(titulo, descricao, usuarioId, new Prazo(prazo), prioridade);
        }
    }
}
