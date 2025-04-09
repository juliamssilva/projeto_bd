using System.Data.SqlClient;

namespace ProjCrud
{
    public static class Conexao
    {
        private static string conexaoString = "Server=livraria.database.windows.net;Database=MeuBanco;User Id=adminsql@livraria;Password=Teixeira-1;";

        public static SqlConnection Conectar()
        {
            var conexao = new SqlConnection(conexaoString);
            conexao.Open();
            return conexao;
        }
    }
}
