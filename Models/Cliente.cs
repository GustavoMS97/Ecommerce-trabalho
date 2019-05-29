using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrabalhoEcommerce.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        public string CPF { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }

        public ICollection<Cartao> Cartoes { get; set; }
        public ICollection<Venda> Compras { get; set; }
    }
}
