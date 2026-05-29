# Ubiquitous Language - Gerenciador de Tarefas

## Entidades
- **Tarefa**: Representação de um trabalho a ser realizado. Possui título, descrição, prazo e status.
- **Usuário**: Pessoa que cria e gerencia tarefas.

## Value Objects
- **Prazo**: Data até quando a tarefa deve ser concluída. Não pode ser no passado.
- **Prioridade**: Nível de importância da tarefa (Baixa, Média, Alta).

## Agregados
- **Tarefa** (Aggregate Root): Contém Prazo e Prioridade. Uma tarefa pertence a um usuário.

## Bounded Contexts
1. **Tarefas**: Gestão de criação, edição e conclusão de tarefas.
2. **Notificações**: Envio de alertas sobre tarefas próximas do vencimento.

## Eventos de Domínio
- **TarefaCriada**: Quando uma tarefa é criada.
- **TarefaConcluida**: Quando uma tarefa é marcada como concluída.
- **TarefaVencendo**: Quando uma tarefa está próxima do prazo (mais de 3 dias para vencer).

## Regras de Negócio
1. Título da tarefa não pode ser vazio.
2. Prazo não pode ser no passado.
3. Tarefa concluída não pode ser editada.
4. Apenas o dono da tarefa pode concluí-la.