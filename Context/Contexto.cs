using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using TrabalhoEcommerce.Models;

namespace TrabalhoEcommerce
{
    public class Contexto : DbContext
    {
        public Contexto() : base("name = ecommerce")
        {
        }
        public DbSet<Boleto> Boleto { get; set; }
        public DbSet<Carrinho> Carrinho { get; set; }
        public DbSet<CarrinhoProduto> CarrinhoProduto { get; set; }
        public DbSet<Cartao> Cartao { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<FormaPagamento> FormaPagamento { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<StatusCarrinho> StatusCarrinho { get; set; }
        public DbSet<StatusPagamento> StatusPagamento { get; set; }
        public DbSet<Venda> Venda { get; set; }
        public DbSet<VendaProduto> VendaProduto { get; set; }
    }
}
