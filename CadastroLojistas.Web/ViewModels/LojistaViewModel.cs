using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.ViewModels
{
    public class LojistaViewModel
    {
        public LojistaModel Lojista { get; set; }
        public List<CidadeModel> Cidade { get; set; }
        public List<CategoriaModel> Categorias { get; set; }
    }
}