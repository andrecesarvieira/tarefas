using GerenciadorTarefas.Domain.Core;
using GerenciadorTarefas.Domain.Tarefas.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GerenciadorTarefas.Domain.Tarefas.Entities
{
    public record Tarefa : Entity
    {
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public Guid UsuarioId { get; private set; }
        public bool Concluida{ get; private set; }
        public Prazo Prazo { get; private set; }
        public Prioridade Prioridade { get; private set; }

        public Tarefa(string titulo, string descricao, Guid usuarioId, Prazo prazo, Prioridade prioridade)
        {
            Validar(titulo, prazo, usuarioId);

            Titulo = titulo;
            Descricao = descricao;
            UsuarioId = usuarioId;
            Prazo = prazo;
            Prioridade = prioridade;
        }

        private static void Validar(string titulo, Prazo prazo, Guid usuarioId)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("O título da tarefa é obrigatório.", nameof(titulo));
            if (prazo.Data < DateTime.UtcNow)
                throw new ArgumentException("O prazo da tarefa deve ser uma data futura.", nameof(prazo));
            if (usuarioId == Guid.Empty)
                throw new ArgumentException("O ID do usuário é obrigatório.", nameof(usuarioId));
        }

        public void Concluir() {
            Concluida = true;
        }
    }
}
