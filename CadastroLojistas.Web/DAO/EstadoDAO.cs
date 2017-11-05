using ControleEstoque.Web.DAOGen;
using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.DAO
{

    public class CategoriaDAO
    {
        private static BD banco;

        public static List<CategoriaModel> GetCategorias(int pagina = 0, int tamanhoPagina = 0, int id = 0)
        {
            banco = new BD();
            banco.Conexao();
            banco.sqlParameterCollec.AddWithValue("Id", id);
            banco.sqlParameterCollec.AddWithValue("Pagina", pagina);
            banco.sqlParameterCollec.AddWithValue("Tamanho", tamanhoPagina);

            DataTable dt = banco.Pesquisa("SELECT * FROM Categorias (NOLOCK) " + (id > 0 ? " WHERE Id_Categoria = @Id" : "") + " ORDER BY Categoria " + (tamanhoPagina != 0 ? " OFFSET @Pagina ROWS FETCH NEXT @Tamanho ROWS ONLY" : ""));

            List<CategoriaModel> categorias = new List<CategoriaModel>();

            foreach (DataRow linha in dt.Rows)
            {
                CategoriaModel cat = new CategoriaModel() { IdCategoria = (byte)linha["id_Categoria"], Categoria = linha["Categoria"].ToString() };
                categorias.Add(cat);
            }

            return categorias;
        }
    }
}