using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrabalhoEcommerce.Models
{
    [Table("Produto")]
    public class Produto
    {
        [Key]
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }
    }
}
