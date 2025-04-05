using System.Data.SqlClient;

namespace ProjCrud
{
    public static class Conexao
    {
        private static string conexaoString = "Server=192.168.0.3;Database=Livraria;User Id=sa;Password=Teixeira;";

        public static SqlConnection Conectar()
        {
            var conexao = new SqlConnection(conexaoString);
            conexao.Open();
            return conexao;
        }
    }
}
