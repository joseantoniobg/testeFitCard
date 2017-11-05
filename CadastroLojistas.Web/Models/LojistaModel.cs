using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.Models
{
    public class LojistaModel
    {
        public LojistaModel()
        {
            this.Cidade = new CidadeModel() { IdCidade = 0, Cidade = "", Estado = "" };
            this.Categoria = new CategoriaModel() { IdCategoria = 0, Categoria = "" };
            this.Status = true;
        }

        public int IdLojista { get; set; }
        public string IdCodificado { get; set; }
        [Required(ErrorMessage = "Informe a Razão Social")]
        [MaxLength(50, ErrorMessage = "Razão Social muito grande")]
        public string Razao { get; set; }
        [MaxLength(50, ErrorMessage = "Nome Fantasia muito grande")]
        public string Fantasia { get; set; }
        [Required(ErrorMessage = "Informe o CNPJ")]
        public string CNPJ { get; set; }
        [MaxLength(50, ErrorMessage = "E-mail muito grande")]
        public string Email { get; set; }
        [MaxLength(60, ErrorMessage = "Endereço muito grande")]
        public string Endereco { get; set; }
        [MaxLength(10, ErrorMessage = "Numero muito grande")]
        public string Numero { get; set; }
        [MaxLength(30, ErrorMessage = "Numero muito grande")]
        public string Complemento { get; set; }
        [MaxLength(40, ErrorMessage = "Bairro muito grande")]
        public string Bairro { get; set; }
        public CidadeModel Cidade { get; set; }
        public CategoriaModel Categoria { get; set; }
        public string Celular { get; set; }
        public string Telefone { get; set; }
        public DateTime? DataCadastro { get; set; }
        public bool Status { get; set; }
    }
}