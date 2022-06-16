using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    public class RepositorioMedicamentoEmBancoDadosTest
    {

        private Medicamento medicamento;
        private RepositorioMedicamentoEmBancoDados repositorio;

        public RepositorioMedicamentoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");

            medicamento = new Medicamento("Dorflex", "Para dor de cabeça", "Lote 1", System.DateTime.Now);
            repositorio = new RepositorioMedicamentoEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_medicamento()
        {

        }

        public void Deve_editar_informacoes_medicamento()
        {
            //arrange                      
            repositorio.Inserir(medicamento);

            //action
            medicamento.Nome = "João de Moraes";
            medicamento.Descricao = "987654321";
            medicamento.Lote = "987654321";
            medicamento.Validade = System.DateTime.Now;
            repositorio.Editar(medicamento);

            //assert
            var medicamentoEncontrado = repositorio.SelecionarPorId(medicamento.Id);

            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento, medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_excluir_medicamento()
        {
            //arrange           
            repositorio.Inserir(medicamento);

            //action           
            repositorio.Excluir(medicamento);

            //assert
            var medicamentoEncontrado = repositorio.SelecionarPorId(medicamento.Id);
            Assert.IsNull(medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_medicamento()
        {
            //arrange          
            repositorio.Inserir(medicamento);

            //action
            var medicamentoEncontrado = repositorio.SelecionarPorId(medicamento.Id);

            //assert
            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento, medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_medicamentos()
        {
            //arrange
            var p01 = new Medicamento("Cimegripe", "Para gripe", "Lote 2", System.DateTime.Now);
            var p02 = new Medicamento("Paracetamol", "Para dor de cabeça", "Lote 1", System.DateTime.Now);
            var p03 = new Medicamento("Dorflex", "Para dor de cabeça", "Lote 1", System.DateTime.Now);

            var repositorio = new RepositorioMedicamentoEmBancoDados();
            repositorio.Inserir(p01);
            repositorio.Inserir(p02);
            repositorio.Inserir(p03);

            //action
            var medicamentos = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(3, medicamentos.Count);

            Assert.AreEqual(p01.Nome, medicamentos[0].Nome);
            Assert.AreEqual(p02.Nome, medicamentos[1].Nome);
            Assert.AreEqual(p03.Nome, medicamentos[2].Nome);
        }


    }
}
