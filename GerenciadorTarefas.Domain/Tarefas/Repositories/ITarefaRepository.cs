using GerenciadorTarefas.Domain.Tarefas.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GerenciadorTarefas.Domain.Tarefas.Repositories
{
    public interface ITarefaRepository
    {
        Task<Tarefa> ObterPorId(Guid id);
        Task Adicionar(Tarefa tarefa);
        Task Atualizar(Tarefa tarefa);
        Task Remover(Guid id);
    }
}
