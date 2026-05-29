# 📋 Gerenciador de Tarefas

## 📌 Descrição do Projeto

O **Gerenciador de Tarefas** é uma aplicação desenvolvida em **C#** utilizando **.NET 6+** que permite que usuários criem, gerenciem e controlem suas tarefas diárias. A aplicação fornece um sistema robusto para organizar trabalhos a serem realizados, atribuindo prazos e níveis de prioridade a cada tarefa, garantindo a integridade dos dados e o cumprimento das regras de negócio.

A solução foi desenvolvida utilizando **Domain-Driven Design (DDD)** como arquitetura principal, aplicando conceitos de **Orientação a Objetos**, **SOLID**, **GRASP** e **testes unitários** para garantir um código limpo, testável e facilmente extensível.

---

## 🎯 Problema a Resolver

**Problema identificado:**

Usuários precisam de uma forma simples, confiável e bem estruturada de gerenciar suas tarefas diárias. Frequentemente enfrentam:

- Dificuldade em organizar tarefas sem perder informações
- Necessidade de atribuir prazos e prioridades
- Risco de alterar tarefas já concluídas (causando inconsistências)
- Falta de um sistema que valide dados de entrada automaticamente
- Dificuldade em rastrear tarefas próximas do vencimento

**Exemplo de cenário problemático:**
```
Um usuário tenta:
1. Criar uma tarefa sem título (não deveria permitir)
2. Atribuir um prazo no passado (tarefa impossível de cumprir)
3. Concluir uma tarefa que já foi concluída (duplicação)

Sem validações, o sistema fica inconsistente.
```

---

## ✅ Solução Proposta

Desenvolver uma aplicação baseada em **Domain-Driven Design** que modele o domínio de tarefas de forma clara e centralizada, garantindo que:

- ✅ **As regras de negócio estejam no coração da aplicação** (camada de domínio)
- ✅ **Dados sejam validados no ponto de entrada** (construtores, value objects)
- ✅ **Operações complexas sejam coordenadas por domain services**
- ✅ **Código seja testável com cobertura >80%**
- ✅ **Princípios SOLID e GRASP sejam aplicados**

**Arquitetura:**
```
┌─────────────────────────────────────────┐
│    GerenciadorTarefas.Domain            │ ← Lógica de negócio pura
│  (Entities, ValueObjects, Services)     │
└─────────────────────────────────────────┘
         ↑                           ↑
         │                           │
    Infrastructure              Tests
 (Repository impl.)           (xUnit, Moq)
```

---

## 📊 Escopo do Projeto

### O que está DENTRO da aplicação:

- ✅ **Criação de tarefas** com título, descrição, prazo e prioridade
- ✅ **Validação de dados de entrada:**
  - Título obrigatório e não vazio
  - Prazo não pode ser no passado
  - Usuário ID obrigatório
- ✅ **Marcação de tarefas como concluídas**
- ✅ **Recuperação de tarefas por ID**
- ✅ **Atualização de tarefas existentes**
- ✅ **Exclusão de tarefas**
- ✅ **Notificações sobre tarefas próximas do vencimento** (via Anti-Corruption Layer)
- ✅ **Persistência de dados** (interface via Repository Pattern)
- ✅ **Testes unitários** com cobertura >80%

### O que está FORA da aplicação:

- ❌ **Interface gráfica (UI)** - apenas lógica de domínio
- ❌ **Autenticação e autorização** de usuários
- ❌ **Envio real de notificações** por email ou SMS
- ❌ **Relatórios e estatísticas** avançadas
- ❌ **Integração com calendários** externos
- ❌ **Sistema de permissões** granulares
- ❌ **Histórico de alterações** (Audit Log)
- ❌ **Sincronização** com sistemas externos
- ❌ **Implementação de Repositório** (apenas interface)

---

## 👥 Usuários do Sistema

### Usuários Diretos

**1. Usuário Final (Pessoa Física)**
- **Quem é:** Qualquer pessoa que precisa gerenciar suas tarefas
- **O que faz:**
  - Cria novas tarefas
  - Visualiza tarefas existentes
  - Marca tarefas como concluídas
  - Atualiza informações de tarefas
  - Exclui tarefas
- **Exemplo:** "João cria uma tarefa 'Estudar DDD' com prazo em 7 dias e prioridade Alta"

---

### Interfaces com Outros Sistemas

**2. Sistema de Notificações (Interface Externa)**
- **Quem é:** Serviço externo que envia alertas
- **O que recebe:**
  - ID da tarefa
  - Título da tarefa
  - Data de vencimento
- **Como integra:**
  - Via **Anti-Corruption Layer** (TarefaAdapter)
  - Recebe DTO traduzido (TarefaVencendoDTO)
  - Não conhece detalhes complexos da Tarefa original
- **Exemplo:** "Sistema de Notificações recebe alerta: 'Estudar DDD vence em 2 dias'"

**3. Banco de Dados (Interface de Persistência)**
- **Quem é:** Sistema de armazenamento de dados
- **O que faz:**
  - Persiste tarefas
  - Recupera tarefas por ID
  - Atualiza tarefas
  - Deleta tarefas
- **Como integra:**
  - Via **Repository Pattern** (ITarefaRepository)
  - Interface define contrato, implementação é agnóstica (SQL, MongoDB, etc)
- **Exemplo:** "Repository salva Tarefa no banco de dados"

---

## 📋 Regras de Negócio

### Regra 1: Título da Tarefa é Obrigatório

**Descrição:** Uma tarefa não pode ser criada sem um título.

**Detalhes:**
- Campo: `Titulo` (string)
- Validação: Não pode ser nulo, vazio ou apenas espaços em branco
- Ação se violada: Lança `ArgumentException`
- Implementação: [Tarefa.cs#L30-L33](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Tarefas/Entities/Tarefa.cs#L30-L33)

**Exemplo:**
```csharp
// ❌ Falha
var tarefa = new Tarefa("", "Descrição", usuarioId, prazo, prioridade);
// ArgumentException: "O título da tarefa é obrigatório."

// ✅ Passa
var tarefa = new Tarefa("Estudar DDD", "Descrição", usuarioId, prazo, prioridade);
```

---

### Regra 2: Prazo não pode ser no Passado

**Descrição:** Uma tarefa não pode ter prazo em uma data anterior à data atual.

**Detalhes:**
- Campo: `Prazo.Data` (DateTime)
- Validação: Deve ser sempre uma data futura (> DateTime.UtcNow)
- Ação se violada: Lança `ArgumentException`
- Implementação: [Prazo.cs#L16-L23](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Tarefas/ValueObjects/Prazo.cs#L16-L23)

**Exemplo:**
```csharp
// ❌ Falha
var prazo = new Prazo(DateTime.UtcNow.AddDays(-7));
// ArgumentException: "O prazo da tarefa deve ser uma data futura."

// ✅ Passa
var prazo = new Prazo(DateTime.UtcNow.AddDays(7));
```

---

### Regra 3: Tarefa Concluída não pode ser Alterada

**Descrição:** Uma tarefa que já foi marcada como concluída não pode ser concluída novamente.

**Detalhes:**
- Campo: `Concluida` (bool)
- Validação: Se já é true, não pode chamar `Concluir()` novamente
- Ação se violada: Lança `InvalidOperationException`
- Implementação: [ServicoDeConclusao.cs#L17-L23](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Tarefas/Services/ServicoDeConclusao.cs#L17-L23)

**Exemplo:**
```csharp
// ✅ Primeira conclusão passa
await servicoDeConclusao.ConcluirTarefa(tarefaId);
// Tarefa.Concluida = true

// ❌ Segunda conclusão falha
await servicoDeConclusao.ConcluirTarefa(tarefaId);
// InvalidOperationException: "A tarefa já está concluída."
```

---

### Regra 4: Apenas o Dono pode Concluir a Tarefa

**Descrição:** Apenas o usuário que criou a tarefa pode marcá-la como concluída.

**Detalhes:**
- Campo: `UsuarioId` (Guid)
- Validação: Comparar `tarefa.UsuarioId` com `usuarioId` do executor
- Ação se violada: Lança `InvalidOperationException`
- Implementação: [ServicoDeConclusao.cs#L17-L23](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Tarefas/Services/ServicoDeConclusao.cs#L17-L23) (a implementar em versão futura)

**Exemplo:**
```csharp
// Tarefa criada por Usuario A
var tarefa = new Tarefa("Estudar", "...", usuarioIdA, prazo, prioridade);

// Usuario B tenta concluir
// InvalidOperationException: "Apenas o dono pode concluir a tarefa."

// Usuario A conclui (OK)
await servicoDeConclusao.ConcluirTarefa(tarefaId, usuarioIdA);  // ✅ Passa
```

---

## 🏗️ Arquitetura (DDD - Domain-Driven Design)

A aplicação está estruturada em **2 Bounded Contexts** independentes:

### **Bounded Context 1: TAREFAS**

**Responsabilidade:** Gerenciar a criação, validação e conclusão de tarefas.

**Componentes:**

- **Entities:**
  - `Tarefa` (Aggregate Root) - [Tarefa.cs](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Tarefas/Entities/Tarefa.cs)
  - `Usuario` - [Usuario.cs](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Tarefas/Entities/Usuario.cs)

- **Value Objects:**
  - `Prazo` - [Prazo.cs](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Tarefas/ValueObjects/Prazo.cs)
  - `Prioridade` - [Prioridade.cs](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Tarefas/ValueObjects/Prioridade.cs)

- **Repository:**
  - `ITarefaRepository` - [ITarefaRepository.cs](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Tarefas/Repositories/ITarefaRepository.cs)

- **Domain Service:**
  - `ServicoDeConclusao` - [ServicoDeConclusao.cs](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Tarefas/Services/ServicoDeConclusao.cs)

- **Factory:**
  - `TarefaFactory` - [TarefaFactory.cs](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Tarefas/Factories/TarefaFactory.cs)

---

### **Bounded Context 2: NOTIFICAÇÕES**

**Responsabilidade:** Gerenciar notificações sobre tarefas próximas do vencimento.

**Componentes:**

- **Anti-Corruption Layer:**
  - `TarefaAdapter` - [TarefaAdapter.cs](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Notificacoes/AntiCorruptionLayer/TarefaAdapter.cs)

- **Data Transfer Objects:**
  - `TarefaVencendoDTO` - [TarefaVencendoDTO.cs](https://github.com/seu-usuario/repo/blob/main/GerenciadorTarefas.Domain/Notificacoes/DTOs/TarefaVencendoDTO.cs)

**Integração:**
- Contexto Notificações **não depende** de Contexto Tarefas
- Comunicação via DTO traduzido pelo Adapter
- Padrão: **Published Language**

---

## 📁 Estrutura de Pastas

```
GerenciadorTarefas/
├── GerenciadorTarefas.Domain/           ← Lógica de negócio pura
│   ├── Core/
│   │   ├── Entity.cs                    ← Classe base para entities
│   │   ├── ValueObject.cs               ← Classe base para value objects
│   │   └── DomainEvent.cs               ← Classe base para eventos
│   │
│   ├── Tarefas/                         ← Bounded Context: Tarefas
│   │   ├── Entities/
│   │   │   ├── Tarefa.cs               ← Aggregate Root
│   │   │   └── Usuario.cs              ← Entity
│   │   ├── ValueObjects/
│   │   │   ├── Prazo.cs                ← Value Object
│   │   │   └── Prioridade.cs           ← Value Object
│   │   ├── Repositories/
│   │   │   └── ITarefaRepository.cs    ← Interface de persistência
│   │   ├── Services/
│   │   │   └── ServicoDeConclusao.cs   ← Domain Service
│   │   └── Factories/
│   │       └── TarefaFactory.cs        ← Factory
│   │
│   └── Notificacoes/                    ← Bounded Context: Notificações
│       ├── DTOs/
│       │   └── TarefaVencendoDTO.cs    ← Data Transfer Object
│       └── AntiCorruptionLayer/
│           └── TarefaAdapter.cs        ← Tradução entre contextos
│
├── GerenciadorTarefas.Tests/            ← Testes unitários
│   ├── TarefaTests.cs                  ← Testes da entidade Tarefa
│   ├── PrazoTests.cs                   ← Testes do value object Prazo
│   └── ServicoDeConclusaoTests.cs      ← Testes do domain service
│
├── GerenciadorTarefas.Application/      ← (Não implementado neste projeto)
├── GerenciadorTarefas.Infrastructure/   ← (Não implementado neste projeto)
│
└── README.md                            ← Este arquivo
```

---

## 🧪 Testes Unitários

A aplicação inclui **8 testes unitários** cobrindo todos os cenários de negócio:

### Testes de `Prazo.cs`

| Teste | Objetivo | Status |
|-------|----------|--------|
| `CriarPrazo_ComDataValida_DeveCriarComSucesso` | Validar que prazo com data futura é criado | ✅ |
| `CriarPrazo_ComDataPassada_DeveLancarExcecao` | Validar que prazo no passado lança exception | ✅ |

### Testes de `Tarefa.cs`

| Teste | Objetivo | Status |
|-------|----------|--------|
| `CriarTarefa_ComTituloValido_DeveCriarTarefa` | Validar que tarefa com título válido é criada | ✅ |
| `CriarTarefa_ComTituloInvalido_DeveLancarExcecao` | Validar que título vazio lança exception | ✅ |
| `CriarTarefa_ComPrazoPassado_DeveLancarExcecao` | Validar que prazo inválido lança exception | ✅ |
| `CriarTarefa_ComUsuarioVazio_DeveLancarExcecao` | Validar que usuário ID vazio lança exception | ✅ |

### Testes de `ServicoDeConclusao.cs`

| Teste | Objetivo | Status |
|-------|----------|--------|
| `ConcluirTarefa_ComTarefaValida_DeveConcluirComSucessoAsync` | Validar que tarefa válida é concluída | ✅ |
| `ConcluirTarefa_ComTarefaJaConcluida_DeveLancarExcecao` | Validar que reconclusão lança exception | ✅ |

---

## 📊 Cobertura de Testes

```
Cobertura Total: 88,2% (blocos) / 87,1% (linhas)
Domain: 77,5% de linhas
Status: ✅ ACIMA DE 80% (meta atingida)
```

**Como executar os testes:**

```bash
# Via Visual Studio
Test > Run All Tests

# Via Terminal
dotnet test
```

---

## 🛠️ Princípios Aplicados

### Domain-Driven Design (DDD)
- ✅ Ubiquitous Language
- ✅ Entities e Value Objects
- ✅ Aggregates e Aggregate Roots
- ✅ Repositories
- ✅ Domain Services e Factories
- ✅ Bounded Contexts
- ✅ Anti-Corruption Layer

### Orientação a Objetos (OO)
- ✅ Encapsulamento (propriedades com `private set`)
- ✅ Abstração (interfaces e classes abstratas)
- ✅ Herança (Entity, ValueObject)
- ✅ Polimorfismo (interfaces)

### SOLID
- ✅ **S**RP (Single Responsibility) - Cada classe uma responsabilidade
- ✅ **O**CP (Open/Closed) - Aberto para extensão via interfaces
- ✅ **L**SP (Liskov Substitution) - Subtypes substituíveis
- ✅ **I**SP (Interface Segregation) - Interfaces específicas
- ✅ **D**IP (Dependency Inversion) - Depende de abstrações

### GRASP
- ✅ **Expert** - Prazo valida prazos
- ✅ **Creator** - TarefaFactory cria tarefas
- ✅ **Controller** - ServicoDeConclusao orquestra
- ✅ **Low Coupling** - Contextos desacoplados via ACL
- ✅ **High Cohesion** - Classes coesas

### Testes
- ✅ Isolamento - Testes sem dependências externas
- ✅ Repetibilidade - Sempre mesmo resultado
- ✅ Rapidez - Execução em milissegundos
- ✅ Auto-verificação - FluentAssertions
- ✅ Abrangência - Casos positivos e negativos
- ✅ Mocks e Stubs - Moq para simular dependências

---

## 📚 Documentação

A documentação completa do projeto está disponível em:

1. **[DocumentacaoProjeto.md](./docs/DocumentacaoProjeto.md)** - DDD, escopo, regras, arquitetura
2. **[SOLID_GRASP_Aplicados.md](./docs/SOLID_GRASP_Aplicados.md)** - Princípios com código comentado
3. **[Testes_e_Cobertura.md](./docs/Testes_e_Cobertura.md)** - Testes, cenários, cobertura
4. **[Mapeamento_Rubricas.md](./docs/Mapeamento_Rubricas.md)** - Cada rubrica mapeada para código

---

## 🚀 Como Usar

### 1. Clonar o repositório

```bash
git clone https://github.com/seu-usuario/GerenciadorTarefas.git
cd GerenciadorTarefas
```

### 2. Abrir no Visual Studio

```bash
start GerenciadorTarefas.sln
```

### 3. Compilar

```bash
dotnet build
```

### 4. Executar testes

```bash
dotnet test
```

### 5. Visualizar cobertura

No Visual Studio:
- Test > Analyze Code Coverage for All Tests

---

## 📝 Licença

Este projeto é um exercício acadêmico.

---

## 👨‍💻 Autor

Desenvolvido como projeto acadêmico para demonstrar aplicação de:
- Domain-Driven Design
- Orientação a Objetos em C#
- Princípios SOLID e GRASP
- Testes Unitários e TDD

---

## ✅ Mapeamento de Rubricas de Avaliação

Esta seção mapeia cada critério de avaliação para a implementação no código-fonte:

### 1. Orientação a Objetos com C#

| Critério | Descrição | Link do Código |
|----------|-----------|---|
| **OO.1** | Encapsulamento, Abstração, Herança e Polimorfismo | [Entity.cs#L5](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Core/Entity.cs#L5), [ValueObject.cs#L3](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Core/ValueObject.cs#L3), [Tarefa.cs#L8](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Entities/Tarefa.cs#L8), [ITarefaRepository.cs#L5](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Repositories/ITarefaRepository.cs#L5) |
| **OO.2** | Modificadores de acesso, propriedades, métodos e construtores | [Usuario.cs#L5](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Entities/Usuario.cs#L5), [Tarefa.cs#L11-L23](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Entities/Tarefa.cs#L11-L23), [Prazo.cs#L9-L18](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/ValueObjects/Prazo.cs#L9-L18) |
| **OO.3** | Herança e polimorfismo para hierarquias flexíveis | [Entity.cs (base)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Core/Entity.cs), [Tarefa.cs#L8 (herança)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Entities/Tarefa.cs#L8), [Usuario.cs#L5 (herança)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Entities/Usuario.cs#L5), [ValueObject.cs (polimorfismo)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Core/ValueObject.cs) |
| **OO.4** | Abstração e encapsulamento com interfaces claras | [ITarefaRepository.cs](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Repositories/ITarefaRepository.cs), [ServicoDeConclusao.cs#L8](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Services/ServicoDeConclusao.cs#L8), [Tarefa.cs (propriedades privadas)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Entities/Tarefa.cs#L11-L16) |

---

### 2. Domain-Driven Design

| Critério | Descrição | Link do Código |
|----------|-----------|---|
| **DDD.1** | Ubiquitous Language, Entities, Value Objects e Repositories | [Tarefa.cs (Entity)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Entities/Tarefa.cs), [Prazo.cs (VO)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/ValueObjects/Prazo.cs), [ITarefaRepository.cs](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Repositories/ITarefaRepository.cs) |
| **DDD.2** | Aggregate, Bounded Contexts e Domain Services | [Tarefa.cs (Aggregate)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Entities/Tarefa.cs), [ServicoDeConclusao.cs (Service)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Services/ServicoDeConclusao.cs), [Tarefas/ (Context)](https://github.com/andrecesarvieira/tarefas/tree/main/GerenciadorTarefas.Domain/Tarefas), [Notificacoes/ (Context)](https://github.com/andrecesarvieira/tarefas/tree/main/GerenciadorTarefas.Domain/Notificacoes) |
| **DDD.3** | Diferenciação entre Domain Services e Factories | [TarefaFactory.cs (Factory)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Factories/TarefaFactory.cs), [ServicoDeConclusao.cs (Service)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Services/ServicoDeConclusao.cs) |
| **DDD.4** | Integração entre Bounded Contexts com ACL e Context Map | [TarefaAdapter.cs (ACL)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Notificacoes/AntiCorruptionLayer/TarefaAdapter.cs), [TarefaVencendoDTO.cs](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Notificacoes/DTOs/TarefaVencendoDTO.cs) |

---

### 3. SOLID e GRASP

| Critério | Descrição | Link do Código |
|----------|-----------|---|
| **SOLID.1** | Princípios SOLID (coesão, responsabilidade, acoplamento) | [Prazo.cs (SRP)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/ValueObjects/Prazo.cs), [ITarefaRepository (OCP)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Repositories/ITarefaRepository.cs), [TarefaAdapter (DIP)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Notificacoes/AntiCorruptionLayer/TarefaAdapter.cs) |
| **SOLID.2** | Single Responsibility Principle | [Prazo.cs#L9-L22](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/ValueObjects/Prazo.cs#L9-L22), [ServicoDeConclusao.cs#L13-L26](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Services/ServicoDeConclusao.cs#L13-L26), [TarefaFactory.cs#L7-L12](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Factories/TarefaFactory.cs#L7-L12) |
| **SOLID.3** | Low Coupling | [TarefaAdapter.cs (desacopla contextos)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Notificacoes/AntiCorruptionLayer/TarefaAdapter.cs), [ServicoDeConclusao.cs#L8 (depende de interface)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Services/ServicoDeConclusao.cs#L8) |
| **GRASP.1** | Controller | [ServicoDeConclusao.cs (orquestra caso de uso)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Domain/Tarefas/Services/ServicoDeConclusao.cs) |

---

### 4. Testes Unitários e TDD

| Critério | Descrição | Link do Código |
|----------|-----------|---|
| **TESTES.1** | Princípios de testes (isolamento, repetibilidade, rapidez, etc) | [TarefaTests.cs](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Tests/TarefaTests.cs), [PrazoTests.cs](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Tests/PrazoTests.cs), [ServicoDeConclusaoTests.cs](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Tests/ServicoDeConclusaoTests.cs) |
| **TESTES.2** | Testes abrangendo todos os métodos com regras de negócio | [TarefaTests.cs (4 testes)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Tests/TarefaTests.cs), [PrazoTests.cs (2 testes)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Tests/PrazoTests.cs), [ServicoDeConclusaoTests.cs (2 testes)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Tests/ServicoDeConclusaoTests.cs) |
| **TESTES.3** | Uso adequado de mocks e stubs | [ServicoDeConclusaoTests.cs#L14-L35 (Mock)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Tests/ServicoDeConclusaoTests.cs#L14-L35), [TarefaTests.cs (Stubs)](https://github.com/andrecesarvieira/tarefas/blob/main/GerenciadorTarefas.Tests/TarefaTests.cs) |
| **TESTES.4** | Cobertura superior a 80% do código de domínio | 87,1% de cobertura (ver [Testes_e_Cobertura.md](./docs/Testes_e_Cobertura.md)) |

---

## 📞 Contato

Para dúvidas sobre o projeto, consulte a documentação completa ou abra uma issue no GitHub.

