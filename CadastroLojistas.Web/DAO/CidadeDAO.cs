using ControleEstoque.Web.DAOGen;
using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.DAO
{
    public class CidadeDAO
    {
        private static BD banco;

        public static List<CidadeModel> GetCidades(int pagina = 0, int tamanhoPagina = 0, int id = 0)
        {
            banco = new BD();
            banco.Conexao();
            banco.sqlParameterCollec.AddWithValue("Id", id);
            banco.sqlParameterCollec.AddWithValue("Pagina", pagina);
            banco.sqlParameterCollec.AddWithValue("Tamanho", tamanhoPagina);

            DataTable dt = banco.Pesquisa("SELECT * FROM Cidades (NOLOCK) " + (id > 0 ? " WHERE Id_Cidade = @Id" : "") + " ORDER BY Estado, Cidade " + (tamanhoPagina != 0 ? " OFFSET @Pagina ROWS FETCH NEXT @Tamanho ROWS ONLY" : ""));

            List<CidadeModel> cidades = new List<CidadeModel>();

            foreach (DataRow linha in dt.Rows)
            {
                CidadeModel cid = new CidadeModel() { IdCidade = (Int16)linha["id_Cidade"], Cidade = linha["Cidade"].ToString(), Estado = linha["Estado"].ToString() };
                cidades.Add(cid);
            }

            return cidades;
        }
    }
}