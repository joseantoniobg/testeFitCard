using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.Models
{
    public class LoginViewModel
    {
        [Display(Name="Usuário")]
        [Required(ErrorMessage ="Informe o Usuário")]
        public string Usuario { get; set; }
        [Display(Name = "Senha")]
        [Required(ErrorMessage ="Informe a Senha")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        [Display(Name = "Lembrar-me")]
        public bool LembrarMe { get; set; }
    }
}