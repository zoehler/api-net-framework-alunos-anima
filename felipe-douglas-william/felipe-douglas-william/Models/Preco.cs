using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace felipe_douglas_william.Models
{
    public class Preco
    {
        [Key] public int Id { get; set; }
        [Required] public int ProdutoId { get; set; }
        [Required] public decimal Valor { get; set; }
        [Required] public DateTime Data { get; set; }

    }
}