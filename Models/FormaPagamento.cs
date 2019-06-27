using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrabalhoEcommerce.Models
{
    [Table("FormaPagamento")]
    public class FormaPagamento
    {
        [Key]
        public int ID { get; set; }
        public Boleto Boleto { get; set; }
        public Cartao Cartao { get; set; }
    }
}
