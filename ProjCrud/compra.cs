using System.Collections.Generic;
using System;
namespace ProjCrud
{
    public class Compra
    {
    
        public int Id { get; set; } 

        public string CpfCliente { get; set; } = string.Empty;
        public Cliente Cliente { get; set;} = new Cliente();


        public int IdVendedor { get; set; }
        public Vendedor Vendedor { get; set;} = new Vendedor();


        public DateTime DataCompra { get; set; }  
        public decimal Total  { get; set;} 
        public string FormaPagamento  { get; set;} = string.Empty;
        public string StatusPagamento  { get; set;} = string.Empty;

        //Uma compra pode ter vários itens de pedido
        public List<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();

        public override string ToString()
        {
            return $"{Id} - CPF Cliente: {CpfCliente} Código do Vendedor:{IdVendedor} -  {DataCompra:dd/MM/yyyy HH:mm:ss} - R$ {Total:F2} - Forma de pagamento: {FormaPagamento}";
        }
    }
}
  