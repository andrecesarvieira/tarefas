using System;
using System.Collections.Generic;
using System.Text;

namespace GerenciadorTarefas.Domain.Notificacoes.DTOs
{
    public class TarefaVencendoDTO
    {
        public Guid Id { get; private set; }
        public string Titulo { get; private set; } = string.Empty;
        public DateTime DataVencimento { get; private set; }

        public TarefaVencendoDTO(Guid id, string titulo, DateTime dataVencimento)
        {
            Id = id;
            Titulo = titulo;
            DataVencimento = dataVencimento;
        }

    }
}
