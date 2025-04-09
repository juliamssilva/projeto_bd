using System.Collections.Generic;
namespace ProjCrud
{
    public class Cliente
    {
    
        public int CpfCliente { get; set; } 

        public string NomeCliente { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        public bool IsFlamengo  { get; set;} 
        public bool IsOnePieceFan  { get; set;} 
        public bool IsTeixeira  { get; set;} 

        //Um cliente pode fazer varias compras
        public List<Compra> Compras { get; set; } = new List<Compra>();
    }
}
    
        
   