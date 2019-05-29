using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrabalhoEcommerce.Models
{
    [Table("CarrinhoProduto")]
    public class CarrinhoProduto
    {
        [Key]
        public int ID { get; set; }
        public Carrinho Carrinho { get; set; }
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
    }
}
