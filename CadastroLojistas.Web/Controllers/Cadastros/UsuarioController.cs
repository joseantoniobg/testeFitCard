using ControleEstoque.Web.DAO;
using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers.Cadastros
{
    public class UsuarioController : Controller
    {

        public int quantItensPagina { get; set; }

        // GET: Cadastro
        [Authorize]
        public ActionResult Index()
        {

            ViewBag.ListaPagina = new SelectList(new int[] { 5, 10, 15, 20 });

            quantItensPagina = 5;

            ViewBag.QuantMaxLinhasPorPagina = quantItensPagina;

            var lista = UsuarioDAO.GetUsuarios(0, quantItensPagina, 0);

            ViewBag.PaginaAtual = 1;
            int Quant = UsuarioDAO.getQuantProdutos();
            ViewBag.QuantItens = Quant;
            ViewBag.QuantPaginas = (Quant / quantItensPagina);

            return View(lista);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult UsuarioPagina(int pagina, int quantItens)
        {

            quantItensPagina = quantItens;

            ViewBag.QuantMaxLinhasPorPagina = quantItensPagina;

            var lista = UsuarioDAO.GetUsuarios((pagina - 1) * quantItensPagina, quantItensPagina, 0);

            ViewBag.PaginaAtual = pagina;

            return Json(lista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult MudaQuantItensPagina(int quant)
        {
            quantItensPagina = quant;
            ViewBag.QuantMaxLinhasPorPagina = quantItensPagina;
            return UsuarioPagina(1, quantItensPagina);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperaUsuario(int id)
        {
            UsuarioModel gp = UsuarioDAO.GetUsuarios(id)[0];
            return Json(gp);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirUsuario(int id)
        {
            return Json(UsuarioDAO.ExcluiUsuario(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarUsuario(UsuarioModel user)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    UsuarioModel userM = UsuarioDAO.SalvarUsuario(user);
                    idSalvo = userM.Id.ToString();
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }

            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AlteraSenha(string user, string senha)
        {

            string resultado = "PENDENTE";

            try
            {
                UsuarioDAO.alteraSenhaUsuario(user, senha);
            }
            catch (Exception ex)
            {
                resultado = "ERRO";
            }

            return Json(new { Resultado = resultado });

        }
    }
}