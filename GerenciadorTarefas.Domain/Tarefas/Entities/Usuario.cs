using GerenciadorTarefas.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GerenciadorTarefas.Domain.Tarefas.Entities
{
    public record Usuario : Entity
    {
        public string Nome { get; private set; } = string.Empty;

        public Usuario(string nome)
        {
            Validar(nome);

            Nome = nome;
        }

        private static void Validar(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do usuário é obrigatório.", nameof(nome));
        }
    }
}
