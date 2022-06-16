using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloPaciente
{
    [TestClass]
    public class RepositorioPacienteEmBancoDadosTest
    {
        private Paciente paciente;
        private RepositorioPacienteEmBancoDados repositorio;

        public RepositorioPacienteEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBPACIENTE; DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)");

            paciente = new Paciente("José da Silva", "321654987");
            repositorio = new RepositorioPacienteEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_paciente()
        {
            //action
            repositorio.Inserir(paciente);

            //assert
            var pacienteEncontrado = repositorio.SelecionarPorId(paciente.Id);

            Assert.IsNotNull(pacienteEncontrado);
            Assert.AreEqual(paciente, pacienteEncontrado);
        }

        [TestMethod]
        public void Deve_editar_informacoes_paciente()
        {
            //arrange                      
            repositorio.Inserir(paciente);

            //action
            paciente.Nome = "João de Moraes";
            paciente.CartaoSUS = "987654321";
            repositorio.Editar(paciente);

            //assert
            var pacienteEncontrado = repositorio.SelecionarPorId(paciente.Id);

            Assert.IsNotNull(pacienteEncontrado);
            Assert.AreEqual(paciente, pacienteEncontrado);
        }

        [TestMethod]
        public void Deve_excluir_paciente()
        {         
            repositorio.Inserir(paciente);
         
            repositorio.Excluir(paciente);

            var pacienteEncontrado = repositorio.SelecionarPorId(paciente.Id);
            Assert.IsNull(pacienteEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_paciente()
        {         
            repositorio.Inserir(paciente);

            var pacienteEncontrado = repositorio.SelecionarPorId(paciente.Id);

            Assert.IsNotNull(pacienteEncontrado);
            Assert.AreEqual(paciente, pacienteEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_pacientes()
        {
            var p01 = new Paciente("Doida 1", "9923123123");
            var p02 = new Paciente("Doida 2", "9992131231");
            var p03 = new Paciente("Doido 1", "9324523523");

            var repositorio = new RepositorioPacienteEmBancoDados();
            repositorio.Inserir(p01);
            repositorio.Inserir(p02);
            repositorio.Inserir(p03);

            var pacientes = repositorio.SelecionarTodos();

            Assert.AreEqual(3, pacientes.Count);

            Assert.AreEqual(p01.Nome, pacientes[0].Nome);
            Assert.AreEqual(p02.Nome, pacientes[1].Nome);
            Assert.AreEqual(p03.Nome, pacientes[2].Nome);
        }
    }
}
