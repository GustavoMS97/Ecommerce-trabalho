using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrabalhoEcommerce.Models
{
    [Table("Boleto")]
    public class Boleto
    {
        [Key]
        public string Codigo { get; set; }
        public double Valor { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}
