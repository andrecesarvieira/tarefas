using GerenciadorTarefas.Domain.Notificacoes.DTOs;
using GerenciadorTarefas.Domain.Tarefas.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GerenciadorTarefas.Domain.Notificacoes.AntiCorruptionLayer
{
    public class TarefaAdapter
    {
        public static TarefaVencendoDTO AdaptarTarefa(Tarefa tarefa)
        {
            return new TarefaVencendoDTO(tarefa.Id, tarefa.Titulo, tarefa.Prazo.Data);
        }
    }
}
