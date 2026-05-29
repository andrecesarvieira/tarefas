using System;
using System.Collections.Generic;
using System.Text;

namespace GerenciadorTarefas.Domain.Core
{
    public abstract record class Entity
    {
        public Guid Id { get; protected set; }= Guid.NewGuid();
    }
}
