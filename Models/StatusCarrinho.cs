using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrabalhoEcommerce.Models
{
    [Table("StatusCarrinho")]
    public class StatusCarrinho
    {
        [Key]
        public int ID { get; set; }
        public string nomeStatus { get; set; }
    }
}
