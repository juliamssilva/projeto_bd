using System.Collections.Generic;
using System.Data.SQLite;

namespace ProjCrud
{
    public static class livroDAO
    {
        // Método para criar um livro no banco de dados
        // Recebe um objeto do tipo Livro como parâmetro
        public static void Criar(Livro livro)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = conexao.CreateCommand();
                // Comando SQL para inserir um novo livro na tabela Livro
                cmd.CommandText = "INSERT INTO Livro (Titulo, Autor, Editora, Ano) VALUES (@Titulo, @Autor, @Editora, @Ano)";
                cmd.Parameters.AddWithValue("@Titulo", livro.Titulo);
                cmd.Parameters.AddWithValue("@Autor", livro.Autor);
                cmd.Parameters.AddWithValue("@Editora", livro.Editora);
                cmd.Parameters.AddWithValue("@Ano", livro.Ano);
                cmd.ExecuteNonQuery();
            }
        }

        // Método para ler todos os livros do banco de dados
        public static List<Livro> Ler()
        {
            //Cria uma lista de livros
            var livros = new List<Livro>();

            using (var conexao = Conexao.Conectar())
            {
                //Criação do comando sql para selecionar todos os livros da tabela Livro
                var cmd = new SQLiteCommand("SELECT * FROM Livro", conexao);
                using (var reader = cmd.ExecuteReader())
                {
                    //itera sobre os resultados da consulta
                    while (reader.Read())
                    {
                        //cria um novo objeto livro para cada linha e adiciona a lista
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

        // Método para atualizar um livro no banco de dados
        // Recebe um objeto livro como parâmetro
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

        // Método para deletar um livro do banco de dados
        // Recebe o id do livro como parâmetro
        public static void Deletar(int id)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SQLiteCommand("DELETE FROM Livro WHERE id = @id", conexao);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // Método para pesquisar livros pelo título
        public static List<Livro> Pesquisar(string titulo)
        {
            var livros = new List<Livro>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SQLiteCommand("SELECT * FROM Livro WHERE Titulo LIKE @Titulo", conexao);
                cmd.Parameters.AddWithValue("@Titulo", "%" + titulo + "%");
                
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
    }
}
