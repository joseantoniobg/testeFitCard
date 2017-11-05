using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Informe o Usuário")]
        public string Nome { get; set; }
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Informe o Login")]
        public string Login { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}