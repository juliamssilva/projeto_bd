using System.Data.SqlClient;

namespace ProjCrud
{
    public static class Conexao
    {
        private static string conexaoString = "Server=localhost;Database=MeuBanco;User Id=sa;Password=MinhaSenha;";

        public static SqlConnection Conectar()
        {
            var conexao = new SqlConnection(conexaoString);
            conexao.Open();
            return conexao;
        }
    }
}
