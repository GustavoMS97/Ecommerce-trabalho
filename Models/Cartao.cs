using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrabalhoEcommerce.Models
{
    [Table("Cartao")]
    public class Cartao
    {
        [Key]
        public int ID { get; set; }
        public Cliente Cliente { get; set; }
        [DisplayName("Número do Cartão")]
        public string Numero { get; set; }
        [DisplayName("Código de Segurança")]
        public string Ccv { get; set; }
        [DisplayName("Data de Vencimento")]
        public DateTime DataVencimento { get; set; }
        [NotMapped]
        [DisplayFormat(DataFormatString = "{yyyy/MM}", ApplyFormatInEditMode = true)]
        public string DataVencimentoStr { get; set; }
    }
}
