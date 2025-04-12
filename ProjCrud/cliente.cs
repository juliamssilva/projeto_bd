using System.Collections.Generic;
namespace ProjCrud
{
    public class Cliente
    {
    
        public string CpfCliente { get; set; }  = string.Empty;

        public string NomeCliente { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        public bool IsFlamengo  { get; set;} 
        public bool IsOnePieceFan  { get; set;} 
        public bool IsTeixeira  { get; set;} 

        //Um cliente pode fazer varias compras
        public List<Compra> Compras { get; set; } = new List<Compra>();

       public override string ToString()
        {
            return $"{CpfCliente} - {NomeCliente} ({Email}) - " +
                $"Flamenguista: {(IsFlamengo ? "Sim" : "Não")}, " + 
                $"Fã de One Piece: {(IsOnePieceFan ? "Sim" : "Não")}, " +
                $"Teixeirense: {(IsTeixeira ? "Sim" : "Não")}";
        }
    }
}
    
        
   