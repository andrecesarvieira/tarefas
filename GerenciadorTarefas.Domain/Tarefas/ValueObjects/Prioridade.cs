using GerenciadorTarefas.Domain.Core;

namespace GerenciadorTarefas.Domain.Tarefas.ValueObjects
{
    public record Prioridade : ValueObject
    {
        public NivelPrioridade Nivel { get; private set; }

        public Prioridade(NivelPrioridade nivel)
        {
            Nivel = nivel;
        }

        public enum NivelPrioridade
        {
            Baixa = 1,
            Media = 2,
            Alta = 3
        }
    }
}