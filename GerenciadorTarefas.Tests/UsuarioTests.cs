using GerenciadorTarefas.Domain.Tarefas.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GerenciadorTarefas.Tests
{
    public class UsuarioTests
    {
        [Fact]
        public void CriarUsuario_ComNomeValido_DeveCriarUsuario()
        {
            // Dados de teste
            var nome = "João Silva";

            // Criar o objeto Usuario
            var usuario = new Usuario(nome);

            // Verificar se o objeto foi criado corretamente
            Assert.NotNull(usuario);
        }

        [Fact]
        public void CriarUsuario_SemNomeValido_NaoDeveCriarUsuario()
        {
            // Dados de teste
            var nome = "";

            // Deve lançar uma exceção ArgumentException
            Assert.Throws<ArgumentException>(() => new Usuario(nome));
        }
    }
}
