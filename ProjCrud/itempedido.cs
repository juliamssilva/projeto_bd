using System.Collections.Generic;
namespace ProjCrud
{
    public class ItemPedido
    {
    
        public int Id { get; set; } 

        public int IdCompra { get; set; }
        public Compra Compra { get; set;} = new Compra();

        public int IdLivro { get; set; }
        public Livro Livro { get; set;} = new Livro();

        
        public int Quantidade  { get; set;} 
        public decimal SubTotal  { get; set;} 

        public override string ToString()
        {
            return $"Id: {Id}, IdCompra: {IdCompra}, IdLivro: {IdLivro}, Quantidade: {Quantidade}, SubTotal: {SubTotal}";
        }
    }
}
  