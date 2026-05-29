using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace GerenciadorTarefas.Domain.Core
{
    public abstract record class DomainEvent
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime OcorridoEm { get; set; } = DateTime.UtcNow;
    }
}
