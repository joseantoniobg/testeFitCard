using ControleEstoque.Web.DAOGen;
using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.DAO
{
    public class LojistaDAO
    {
        private static BD banco;

        public static LojistaModel SalvarLojista(LojistaModel lojista)
        {
            banco = new BD();

            try
            {
                banco.Conexao();
                banco.BeginTrans();
                banco.sqlParameterCollec.AddWithValue("CNPJ", lojista.CNPJ ?? "");
                banco.sqlParameterCollec.AddWithValue("Razao", lojista.Razao ?? "");
                banco.sqlParameterCollec.AddWithValue("Fantasia", lojista.Fantasia ?? "");
                banco.sqlParameterCollec.AddWithValue("Email", lojista.Email ?? "");
                banco.sqlParameterCollec.AddWithValue("Endereco", lojista.Endereco ?? "");
                banco.sqlParameterCollec.AddWithValue("Numero", lojista.Numero ?? "");
                banco.sqlParameterCollec.AddWithValue("Complemento", lojista.Complemento ?? "");
                banco.sqlParameterCollec.AddWithValue("Bairro", lojista.Bairro ?? "");
                banco.sqlParameterCollec.AddWithValue("Telefone", lojista.Telefone ?? "");
                banco.sqlParameterCollec.AddWithValue("Celular", lojista.Celular ?? "");
                banco.sqlParameterCollec.AddWithValue("idCidade", lojista.Cidade.IdCidade);
                banco.sqlParameterCollec.AddWithValue("idCategoria", lojista.Categoria.IdCategoria);
                banco.sqlParameterCollec.AddWithValue("DataCadastro", lojista.DataCadastro ?? DateTime.Now);
                banco.sqlParameterCollec.AddWithValue("Status", lojista.Status);

                if (lojista.IdLojista == 0)
                {
                    int id = Convert.ToInt32(banco.Executa("INSERT INTO Lojistas (CNPJ, Razao, Fantasia, Email, " +
                                                           "Endereco, Numero, Complemento, Bairro, Telefone, Celular, " +
                                                           "id_Cidade, id_Categoria, Data_Cadastro, Status) " + 
                                                           "VALUES (@CNPJ, @Razao, @Fantasia, @Email, @Endereco, " +
                                                           "@Numero, @Complemento, @Bairro, @Telefone, @Celular, " +
                                                           (lojista.Cidade.IdCidade == 0 ? "NULL" : "@idCidade") + ", " + (lojista.Categoria.IdCategoria == 0 ? "NULL" : "@idCategoria") + ", " + (lojista.DataCadastro != null ? "@DataCadastro" : "NULL") + ", @Status);SELECT SCOPE_IDENTITY();"));
                    lojista.IdLojista = id;
                }
                else
                {
                    banco.sqlParameterCollec.AddWithValue("Id", lojista.IdLojista);
                    banco.Executa("UPDATE Lojistas SET CNPJ = @CNPJ, Razao = @Razao, Fantasia = @Fantasia, Email = @Email, " +
                                                           "Endereco = @Endereco, Numero = @Numero, Complemento = @Complemento, " +
                                                           "Bairro = @Bairro, Telefone = @Telefone, Celular = @Celular, " +
                                                           "id_Cidade = " + (lojista.Cidade.IdCidade == 0 ? "NULL" : "@idCidade") + ", id_Categoria = " + (lojista.Categoria.IdCategoria == 0 ? "NULL" : "@idCategoria") + ", " +
                                                           "Data_Cadastro = " + (lojista.DataCadastro != null ? "@DataCadastro," : "NULL,") + " Status = @Status " +
                                                           "WHERE Id_Lojista = @Id");
                }

                banco.CommitTrans();

            }
            catch (Exception ex)
            {
                banco.RollBack();
                throw;
            }
           
            return lojista;
        }

        public static int GetQuantLojistas()
        {
            banco = new BD();
            banco.Conexao();
            DataTable gruposProduto = banco.Pesquisa("SELECT COUNT(id_Lojista) AS Quant FROM Lojistas (NOLOCK)");
            return Convert.ToInt32(gruposProduto.Rows[0]["Quant"]);
        }

        public static List<LojistaModel> GetLojistas(int pagina = 0, int tamanhoPagina = 0, string id = "0")
        {

            banco = new BD();
            banco.Conexao();
            banco.sqlParameterCollec.AddWithValue("Id", id);
            banco.sqlParameterCollec.AddWithValue("Pagina", pagina);
            banco.sqlParameterCollec.AddWithValue("Tamanho", tamanhoPagina);
            DataTable gruposProduto = banco.Pesquisa("SELECT * FROM Lojistas (NOLOCK) " + 
                                                     "LEFT JOIN Cidades (NOLOCK) ON Cidades.id_Cidade = Lojistas.id_Cidade " +
                                                     "LEFT JOIN Categorias (NOLOCK) ON Categorias.id_Categoria = Lojistas.id_Categoria " + (!id.Equals("0") ? " WHERE Id_Codificado = @Id" : "") + " ORDER BY Razao" + (tamanhoPagina != 0 ? " OFFSET @Pagina ROWS FETCH NEXT @Tamanho ROWS ONLY" : ""));
            List<LojistaModel> listaLojistas = new List<LojistaModel>();

            foreach(DataRow linha in gruposProduto.Rows)
            {
                LojistaModel lojista = new LojistaModel();
                lojista.IdLojista = (int)linha["Id_Lojista"];
                lojista.IdCodificado = linha["Id_Codificado"].ToString();
                lojista.CNPJ = linha["CNPJ"].ToString();
                lojista.Razao = linha["Razao"].ToString();
                lojista.Fantasia = linha["Fantasia"].ToString();
                lojista.Email = linha["Email"].ToString();
                lojista.Endereco = linha["Endereco"].ToString();
                lojista.Numero = linha["Numero"].ToString();
                lojista.Complemento = linha["Complemento"].ToString();
                lojista.Bairro = linha["Bairro"].ToString();
                lojista.Telefone = linha["Telefone"].ToString();
                lojista.Celular = linha["Celular"].ToString();
                lojista.Cidade = new CidadeModel { IdCidade = (linha["Id_Cidade"] != System.DBNull.Value ? Convert.ToInt16(linha["Id_Cidade"]) : (Int16)0), Cidade = (linha["Cidade"] != System.DBNull.Value ? Convert.ToString(linha["Cidade"]) : ""), Estado = (linha["Estado"] != System.DBNull.Value ? Convert.ToString(linha["Estado"]) : "") };
                lojista.Categoria = new CategoriaModel { IdCategoria = (linha["Id_Categoria"] != System.DBNull.Value ? Convert.ToByte(linha["Id_Categoria"]) : (byte)0), Categoria = (linha["Categoria"] != System.DBNull.Value ? Convert.ToString(linha["Categoria"]) : "") };
                lojista.DataCadastro = (linha["Data_Cadastro"] != System.DBNull.Value ? Convert.ToDateTime(linha["Data_Cadastro"]) : (DateTime?)null);
                lojista.Status = (bool)linha["Status"];

                listaLojistas.Add(lojista);
            }

            return listaLojistas;
        }

        public static bool ExcluiLojista(string id)
        {

            banco = new BD();
            banco.Conexao();
            banco.BeginTrans();
            banco.sqlParameterCollec.AddWithValue("Id", id);
            banco.Executa("DELETE FROM Lojistas WHERE Id_Codificado = @Id");
            banco.CommitTrans();

            return true;
        }
    }
}