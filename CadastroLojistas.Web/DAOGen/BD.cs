using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ControleEstoque.Web.DAOGen
{
	public class BD
	{

        public SqlParameterCollection sqlParameterCollec { get; set; }

        public SqlConnection SqlConexao;

        SqlCommand sqlComm;

        SqlTransaction transaction;

        public void BeginTrans()
        {
            SqlConexao = CriarConexao();

            SqlConexao.Open();

            sqlComm = SqlConexao.CreateCommand();

            transaction = SqlConexao.BeginTransaction("transacaosimples");

            sqlComm.Connection = SqlConexao;
            sqlComm.Transaction = transaction;

        }

        public void Conexao()
        {
            SqlConexao = CriarConexao();

            SqlConexao.Open();

            sqlComm = SqlConexao.CreateCommand();

            sqlComm.Connection = SqlConexao;

        }

        public void CommitTrans()
        {
            transaction.Commit();

            SqlConexao.Close();
        }

        public void FechaConexao()
        {
            SqlConexao.Close();
        }

        public void RollBack()
        {
            transaction.Rollback();

            SqlConexao.Close();
        }

		//Criar a conexão com o banco
		public SqlConnection CriarConexao()
		{
            string strConexao = ConfigurationManager.ConnectionStrings["testes"].ConnectionString;

            return new SqlConnection(strConexao);
		}

		public BD()
		{
			sqlParameterCollec = new SqlCommand().Parameters;
		}

		public void LimparParametros()
		{
			//sqlParameterCollec.Clear();
			sqlParameterCollec = new SqlCommand().Parameters;
		}

		public void AdicionaParametros(string nomeParametro, object valorParametro)
		{
			sqlParameterCollec.Add(new SqlParameter(nomeParametro, valorParametro));
		}

		public object ExecutarFunction(string Function)
		{

				try
				{

					//Colocando infos. dentro do comando
					sqlComm.CommandText = Function;
					sqlComm.CommandTimeout = 3600; //Em segundos, ou 1 hora
                    sqlComm.Parameters.Clear();

                    foreach (SqlParameter sqlParameter in sqlParameterCollec)
                    {
                        sqlComm.Parameters.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.Value));
                    }

					object saida = sqlComm.ExecuteScalar();

					return saida;
				}
				
                catch (Exception)
				{
					try
					{
						transaction.Rollback();
					}
					catch (Exception)
					{
						throw;
					}
				}

				return 0;
		}

        public object ExecutarProcedure(string Function)
        {

            try
            {

                //Colocando infos. dentro do comando
                sqlComm.CommandText = Function;
                sqlComm.CommandTimeout = 3600; //Em segundos, ou 1 hora
                sqlComm.Parameters.Clear();
                sqlComm.CommandType = CommandType.StoredProcedure;

                foreach (SqlParameter sqlParameter in sqlParameterCollec)
                {
                    sqlComm.Parameters.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.Value));
                }

                object saida = sqlComm.ExecuteReader();

                return saida;
            }

            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return 0;
        }

		//Retorna o Scope Identity
		public object Executa(string nomeStoredProcedureOuTextoSql) 
		{

            //Colocando infos. dentro do comando
            sqlComm.CommandType = CommandType.Text;
            sqlComm.CommandText = nomeStoredProcedureOuTextoSql;
            sqlComm.CommandTimeout = 3600; //Em segundos, ou 1 hora
            sqlComm.Parameters.Clear();

            foreach (SqlParameter sqlParameter in sqlParameterCollec)
            {
                sqlComm.Parameters.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.Value));
            }

            //Executa o comando
            return sqlComm.ExecuteScalar();
            
		}

		//Consultar registros do banco
		public DataTable Pesquisa(string nomeStoredProcedureOuTextoSql)
		{
			//Cria a conexão
			SqlConnection sqlConnection = CriarConexao();

			//Abre a conexão
			sqlConnection.Open();

			//Comando que vai levar a info ao banco
			SqlCommand sqlComm = sqlConnection.CreateCommand();

			//Colocando infos. dentro do comando
			sqlComm.CommandType = CommandType.Text;
			sqlComm.CommandText = nomeStoredProcedureOuTextoSql;
			sqlComm.CommandTimeout = 3600; //Em segundos, ou 1 hora
            sqlComm.Parameters.Clear();

			//Adiciona os parâmetros no comando
			foreach (SqlParameter sqlParameter in sqlParameterCollec)
			{
				sqlComm.Parameters.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.Value));
			}

			//Cria um adaptador
			SqlDataAdapter sqlDataAdapt = new SqlDataAdapter(sqlComm);

			//DataTable - tabela vazia que vai ser preenchida com dados da tabela do banco
			DataTable dataTabl = new DataTable();

			//Preenche o datatable
			sqlDataAdapt.Fill(dataTabl);

            sqlConnection.Close();

            sqlConnection.Dispose();

			return dataTabl;
		}

        public DataTable PesquisaRelatorio(string nomeStoredProcedureOuTextoSql, SqlParameterCollection lista)
        {
            //Cria a conexão
            SqlConnection sqlConnection = CriarConexao();

            //Abre a conexão
            sqlConnection.Open();

            //Comando que vai levar a info ao banco
            SqlCommand sqlComm = sqlConnection.CreateCommand();

            //Colocando infos. dentro do comando
            sqlComm.CommandType = CommandType.Text;
            sqlComm.CommandText = nomeStoredProcedureOuTextoSql;
            sqlComm.CommandTimeout = 3600; //Em segundos, ou 1 hora
            sqlComm.Parameters.Clear();

            //Adiciona os parâmetros no comando
            foreach (SqlParameter sqlParameter in lista)
            {
                sqlComm.Parameters.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.Value));
            }

            //Cria um adaptador
            SqlDataAdapter sqlDataAdapt = new SqlDataAdapter(sqlComm);

            //DataTable - tabela vazia que vai ser preenchida com dados da tabela do banco
            DataSet dataTabl = new DataSet();

            //Preenche o datatable
            sqlDataAdapt.Fill(dataTabl, "Teste");

            sqlConnection.Close();

            sqlConnection.Dispose();

            return dataTabl.Tables[0];
        }

        public DataSet testds = new DataSet();

	}
}
