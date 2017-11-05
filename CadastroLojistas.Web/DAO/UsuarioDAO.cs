using ControleEstoque.Web.DAOGen;
using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace ControleEstoque.Web.DAO
{
    public class UsuarioDAO
    {

        public static BD banco;

        public static string ValidarUsuario(string usuario, string senha)
        {

            banco = new BD();

            banco.Conexao();
            banco.sqlParameterCollec.AddWithValue("@user", usuario);
            banco.sqlParameterCollec.AddWithValue("@Senha", senha);

            string valido = Convert.ToString(banco.ExecutarFunction("SELECT dbo.ValidaUsuario(@User, @Senha) AS Valido"));

            return valido;
   
        }

        public static UsuarioModel SalvarUsuario(UsuarioModel user)
        {
            banco = new BD();

            try
            {
                banco.Conexao();
                banco.BeginTrans();
                banco.sqlParameterCollec.AddWithValue("Nome", user.Nome);
                banco.sqlParameterCollec.AddWithValue("Login", user.Login);

                if (user.Id == 0)
                {
                    int id = Convert.ToInt32(banco.Executa("INSERT INTO Usuarios (Nome, Usuario, Senha) VALUES (@Nome, @Login, '');SELECT SCOPE_IDENTITY();"));
                    user.Id = id;
                }
                else
                {
                    banco.sqlParameterCollec.AddWithValue("Id", user.Id);
                    banco.Executa("UPDATE Usuarios SET Nome = @Nome, Usuario = @Login WHERE Id_Usuario = @Id");
                }

                banco.CommitTrans();

            }
            catch (Exception ex)
            {
                banco.RollBack();
                throw;
            }

            return user;
        }

        public static string alteraSenhaUsuario(string login, string senha)
        {
            banco = new BD();

            try
            {
                banco.Conexao();
                banco.BeginTrans();
                banco.sqlParameterCollec.AddWithValue("Login", login);
                banco.sqlParameterCollec.AddWithValue("Senha", senha);
                banco.ExecutarProcedure("EXEC dbo.AtualizaSenha @Login, @Senha");
                banco.CommitTrans();
            }
            catch (Exception ex)
            {
                banco.RollBack();
                throw;
            }

            return "OK";

        }

        public static int getQuantProdutos()
        {
            banco = new BD();
            banco.Conexao();
            DataTable gruposProduto = banco.Pesquisa("SELECT COUNT(*) AS Quant FROM Usuarios (NOLOCK)");
            return Convert.ToInt32(gruposProduto.Rows[0]["Quant"]);
        }

        public static List<UsuarioModel> GetUsuarios(int pagina = 0, int tamanhoPagina = 0, int id = 0)
        {

            banco = new BD();
            banco.Conexao();
            banco.sqlParameterCollec.AddWithValue("Id", id);
            banco.sqlParameterCollec.AddWithValue("Pagina", pagina);
            banco.sqlParameterCollec.AddWithValue("Tamanho", tamanhoPagina);
            DataTable gruposProduto = banco.Pesquisa("SELECT * FROM Usuarios" + (id > 0 ? " WHERE Id_Usuario = @Id" : "") + " ORDER BY Nome" + (tamanhoPagina != 0 ? " OFFSET @Pagina ROWS FETCH NEXT @Tamanho ROWS ONLY" : ""));
            List<UsuarioModel> listaGruposProduto = new List<UsuarioModel>();

            foreach (DataRow linha in gruposProduto.Rows)
            {
                UsuarioModel user = new UsuarioModel();
                user.Id = (int)linha["Id_Usuario"];
                user.Nome = linha["Nome"].ToString();
                user.Login = linha["Usuario"].ToString();
                user.DataCadastro = Convert.ToDateTime(linha["Data_Cadastro"]);

                listaGruposProduto.Add(user);
            }

            return listaGruposProduto;
        }

        public static bool ExcluiUsuario(int id)
        {

            banco = new BD();
            banco.Conexao();
            banco.BeginTrans();
            banco.sqlParameterCollec.AddWithValue("Id", id);
            banco.Executa("DELETE FROM Usuarios WHERE Id_Usuario = @Id");
            banco.CommitTrans();

            return true;
        }
    }
}