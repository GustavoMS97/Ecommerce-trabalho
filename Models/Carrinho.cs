using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrabalhoEcommerce.Models
{
    [Table("Carrinho")]
    public class Carrinho
    {
        [Key]
        public int ID { get; set; }
        public Cliente Cliente { get; set; }
        public StatusCarrinho StatusCarrinho { get; set; }
    }
}
