using ControleMedicamentos.Dominio.ModuloPaciente;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.ModuloPaciente
{
    public class RepositorioPacienteEmBancoDados
    {

        private string enderecoBanco =
            @"Data Source=(LOCALDB)\MSSQLLOCALDB;
              Initial Catalog=ControleMedicamentosDb;
              Integrated Security=True";

        private const string sqlInserir =
           @"INSERT INTO 
                TBPACIENTE
                    (
                        NOME,
                        CARTAOSUS
                    )
                        VALUES
                    (
                        @NOME,
                        @CARTAOSUS
                    ); SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE [TBPACIENTE]	
		        SET
			        [NOME] = @NOME,
                    [CARTAOSUS] = @CARTAOSUS
		        WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
            @"DELETE FROM [TBPACIENTE]
		        WHERE
			        [ID] = @ID";

        private const string sqlSelecionarTodos =
           @"SELECT 
		            [ID], 
		            [NOME], 
		            [CARTAOSUS]
	            FROM 
		            [TBPACIENTE]";

        private const string sqlSelecionarPorNumero =
               @"SELECT 
		            [ID], 
		            [NOME], 
		            [CARTAOSUS]
	            FROM 
		            [TBPACIENTE]
		        WHERE
                    [ID] = @ID";

        public ValidationResult Inserir(Paciente novoPaciente)
        {
            var validador = new ValidadorPaciente();

            var resultadoValidacao = validador.Validate(novoPaciente);

            if (!resultadoValidacao.IsValid)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosPaciente(novoPaciente, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novoPaciente.Id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }


        public ValidationResult Editar(Paciente paciente)
        {
            var validador = new ValidadorPaciente();

            var resultadoValidacao = validador.Validate(paciente);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosPaciente(paciente, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Paciente paciente)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", paciente.Id);

            conexaoComBanco.Open();
            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o registro"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public List<Paciente> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorPaciente = comandoSelecao.ExecuteReader();

            List<Paciente> pacientes = new List<Paciente>();

            while (leitorPaciente.Read())
            {
                Paciente funcionario = ConverterParaPaciente(leitorPaciente);

                pacientes.Add(funcionario);
            }

            conexaoComBanco.Close();

            return pacientes;
        }

        public Paciente SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorPaciente = comandoSelecao.ExecuteReader();

            Paciente paciente = null;
            if (leitorPaciente.Read())
                paciente = ConverterParaPaciente(leitorPaciente);

            conexaoComBanco.Close();

            return paciente;
        }

        private static Paciente ConverterParaPaciente(SqlDataReader leitorPaciente)
        {
            int id = Convert.ToInt32(leitorPaciente["ID"]);
            string nome = Convert.ToString(leitorPaciente["NOME"]);
            string cartaosus = Convert.ToString(leitorPaciente["CARTAOSUS"]);



            var paciente = new Paciente
            {
                Id = id,
                Nome = nome,
                CartaoSUS = cartaosus,
            };

            return paciente;
        }

        private static void ConfigurarParametrosPaciente(Paciente novoPaciente, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("NUMERO", novoPaciente.Id);
            comando.Parameters.AddWithValue("NOME", novoPaciente.Nome);
            comando.Parameters.AddWithValue("CARTAOSUS", novoPaciente.CartaoSUS);
        }
    }
}
