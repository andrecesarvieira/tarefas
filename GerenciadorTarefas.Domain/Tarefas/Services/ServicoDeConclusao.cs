using GerenciadorTarefas.Domain.Tarefas.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace GerenciadorTarefas.Domain.Tarefas.Services
{
    public class ServicoDeConclusao
    {
        private readonly ITarefaRepository _tarefaRepository;

        public ServicoDeConclusao(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task ConcluirTarefa(Guid tarefaId) {

            var tarefa = await _tarefaRepository.ObterPorId(tarefaId);
            if (tarefa.Concluida)
            {
                throw new InvalidOperationException("A tarefa já está concluída.");
            }

            tarefa.Concluir();
            await _tarefaRepository.Atualizar(tarefa);
        }
    }
}
