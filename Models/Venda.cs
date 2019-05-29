using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrabalhoEcommerce.Models
{
    [Table("Venda")]
    public class Venda
    {
        [Key]
        public int ID { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime DataCompra { get; set; }
        public double Desconto { get; set; }
        public bool Entregue { get; set; }
        public StatusPagamento StatusPagamento { get; set; }
        public string Remessa { get; set; }
    }
}
