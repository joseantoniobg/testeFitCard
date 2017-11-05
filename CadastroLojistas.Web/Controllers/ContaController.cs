using ControleEstoque.Web.DAO;
using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ControleEstoque.Web.Controllers
{
    public class ContaController : Controller
    {

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login (LoginViewModel Login, string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Status = "Falso", URL = "" });
            }

            string achou = UsuarioDAO.ValidarUsuario(Login.Usuario,Login.Senha);

            if (!achou.Equals(""))
            {
                FormsAuthentication.SetAuthCookie(Login.Usuario, Login.LembrarMe);
                Session["Nome"] = achou;
                if (Url.IsLocalUrl(ReturnUrl) && !ReturnUrl.Equals("/"))
                {
                    return Json(new { Status = "OK", Nome = achou, URL = ReturnUrl });
                }
                else
                {
                    return Json(new { Status = "OK", Nome = achou, URL = Url.Action("Index", "Home")});
                }
            }
            else
            {
                return Json(new { Status = "Invalido", Nome = "", URL = "" });
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}