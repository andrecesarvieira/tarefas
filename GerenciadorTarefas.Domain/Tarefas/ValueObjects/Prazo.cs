using GerenciadorTarefas.Domain.Core;

namespace GerenciadorTarefas.Domain.Tarefas.ValueObjects
{
    public record Prazo : ValueObject
    {
        public DateTime Data { get; private set; }

        public Prazo(DateTime data)
        {
            Validar(data);
            Data = data;
        }
        private static void Validar(DateTime data)
        {
            if (data < DateTime.UtcNow)
                throw new ArgumentException("O prazo da tarefa deve ser uma data futura.", nameof(data));
        }
    }
}