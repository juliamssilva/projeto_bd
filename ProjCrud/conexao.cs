using System.Data.SQLite;

namespace ProjCrud
{
    public static class Conexao
    {
        private static string conexaoString = "Data Source=banco.db";

        public static SQLiteConnection Conectar()
        {
            var conexao = new SQLiteConnection(conexaoString);
            conexao.Open();
            return conexao;
        }
    }
}
