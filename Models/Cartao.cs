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
        [DisplayName("Conta Corrente")]
        public string Cc { get; set; }
        [DisplayName("Agência")]
        public string Ag { get; set; }
        [DisplayName("Data de Vencimento")]
        [DisplayFormat(DataFormatString = "{yyyy/MM}", ApplyFormatInEditMode = true)]
        public DateTime DataVencimento { get; set; }
        [NotMapped]
        public string DataVencimentoStr { get; set; }
    }
}
