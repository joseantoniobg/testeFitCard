using ControleEstoque.Web.DAO;
using ControleEstoque.Web.Funcoes;
using ControleEstoque.Web.Models;
using ControleEstoque.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class LojistaController : Controller
    {

        public int quantItensPagina { get; set; }

        [Authorize]
        public ActionResult Index()
        {

            ViewBag.ListaPagina = new SelectList(new int[] { 10, 25, 50, 100 });

            quantItensPagina = 10;

            ViewBag.QuantMaxLinhasPorPagina = quantItensPagina;

            var lista = LojistaDAO.GetLojistas(0, 0, "0");
            ViewBag.PaginaAtual = 1;
            int Quant = LojistaDAO.GetQuantLojistas();
            ViewBag.QuantItens = Quant;
            ViewBag.QuantPaginas = (Quant / quantItensPagina);

            return View(lista);
        }

        [HttpGet]
        public ActionResult Cadastro(string id = "0")
        {
            LojistaViewModel lojista = new LojistaViewModel();

            lojista.Lojista = (id.Equals("0") ? new LojistaModel() : LojistaDAO.GetLojistas(0,0, id)[0]);
            lojista.Cidade = CidadeDAO.GetCidades();
            lojista.Categorias = CategoriaDAO.GetCategorias();

            return View(lojista);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult LojistasPagina(int pagina, int quantItens)
        {

            quantItensPagina = quantItens;

            ViewBag.QuantMaxLinhasPorPagina = quantItensPagina;

            var lista = LojistaDAO.GetLojistas((pagina - 1) * quantItensPagina, quantItensPagina, "0");

            ViewBag.PaginaAtual = pagina;

            int quant = LojistaDAO.GetQuantLojistas();
            ViewBag.QuantItens = quant;

            return Json(new { Lista = lista, Quant = quant });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult MudaQuantItensPagina(int quant)
        {
            quantItensPagina = quant;
            ViewBag.QuantMaxLinhasPorPagina = quantItensPagina;
            return LojistasPagina(1, quantItensPagina);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarLojista(string id)
        {
            LojistaModel gp = LojistaDAO.GetLojistas(0, 0, id)[0];
            return Json(gp);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirLojista(string id)
        {
            return Json(LojistaDAO.ExcluiLojista(id));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarLojista(LojistaModel gp)
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
                if (!fo.IsCnpj(gp.CNPJ))
                {
                    resultado = "AVISO";
                    mensagens.Add("CNPJ Inválido");
                }
                else if (gp.Categoria.IdCategoria == 1 && gp.Telefone == null && gp.Celular == null)
                {
                    resultado = "AVISO";
                    mensagens.Add("Informe ao menos um telefone ou celular");
                }
                else
                {
                    try
                    {
                        LojistaModel gpm = LojistaDAO.SalvarLojista(gp);
                        idSalvo = gpm.IdLojista.ToString();
                    }
                    catch (Exception ex)
                    {
                        resultado = "ERRO";
                    }
                }
            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}