using System.Collections.Generic;
namespace ProjCrud
{
    public class Vendedor
    {
    
        public int IdVendedor { get; set; } 
        public string NomeVendedor { get; set; } = string.Empty;
        public decimal Salario  { get; set; }

        // Um vendedor pode estar responsável por várias compras
        public List<Compra> Compras { get; set; } = new List<Compra>();
        
    }
}