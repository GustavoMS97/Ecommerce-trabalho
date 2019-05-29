using TrabalhoEcommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrabalhoEcommerce.Models
{
    [Table("VendaProduto")]
    public class VendaProduto
    {
        [Key]
        public int ID { get; set; }
        public Venda Venda { get; set; }
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
    }
}
