using System.Collections.Generic;
using System.Data.SQLite;

namespace ProjCrud
{
    public static class livroDAO
    {
        public static void Criar(Livro livro)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "INSERT INTO Livro (Titulo, Autor, Editora, Ano) VALUES (@Titulo, @Autor, @Editora, @Ano)";
                cmd.Parameters.AddWithValue("@Titulo", livro.Titulo);
                cmd.Parameters.AddWithValue("@Autor", livro.Autor);
                cmd.Parameters.AddWithValue("@Editora", livro.Editora);
                cmd.Parameters.AddWithValue("@Ano", livro.Ano);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Livro> Ler()
        {
            var livros = new List<Livro>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SQLiteCommand("SELECT * FROM Livro", conexao);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        livros.Add(new Livro
                        {
                            Id = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            Autor = reader.GetString(2),
                            Editora = reader.GetString(3),
                            Ano = reader.GetInt32(4)
                        });
                    }
                }
            }

            return livros;
        }

        public static void Atualizar(Livro livro)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SQLiteCommand("UPDATE Livro SET titulo = @Titulo, autor = @Autor, editora = @Editora, ano = @Ano WHERE id = @Id", conexao);
                cmd.Parameters.AddWithValue("@Titulo", livro.Titulo);
                cmd.Parameters.AddWithValue("@Autor", livro.Autor);
                cmd.Parameters.AddWithValue("@Editora", livro.Editora);
                cmd.Parameters.AddWithValue("@Ano", livro.Ano);
                cmd.Parameters.AddWithValue("@Id", livro.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Deletar(int id)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SQLiteCommand("DELETE FROM Livro WHERE id = @id", conexao);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}