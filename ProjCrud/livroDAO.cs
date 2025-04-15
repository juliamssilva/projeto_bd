using System.Collections.Generic;
using System;
using System.Data.SqlClient;

#nullable disable

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
                cmd.CommandText = "INSERT INTO Livro (Titulo, Autor, Editora, Ano, Categoria, Preco, Estoque) VALUES (@Titulo, @Autor, @Editora, @Ano, @Categoria, @Preco, @Estoque)";
                cmd.Parameters.AddWithValue("@Titulo", livro.Titulo);
                cmd.Parameters.AddWithValue("@Autor", livro.Autor);
                cmd.Parameters.AddWithValue("@Editora", livro.Editora);
                cmd.Parameters.AddWithValue("@Ano", livro.Ano);
                cmd.Parameters.AddWithValue("@Categoria", livro.Categoria);
                cmd.Parameters.AddWithValue("@Preco", livro.Preco);
                cmd.Parameters.AddWithValue("@Estoque", livro.Estoque);
                cmd.ExecuteNonQuery();
            }
        }

        // Método para ler todos os livros do banco de dados
       public static List<Livro> Ler()
     {
            var livros = new List<Livro>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM Livro", conexao);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        livros.Add(new Livro
                        {
                            Id = (int)reader["Id"],
                            Titulo = reader["Titulo"].ToString(),
                            Autor = reader["Autor"].ToString(),
                            Editora = reader["Editora"].ToString(),
                            Ano = (int)reader["Ano"],
                            Categoria = reader["Categoria"].ToString(),
                            Preco = (decimal)reader["Preco"],
                            Estoque = (int)reader["Estoque"]
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
                var cmd = new SqlCommand("UPDATE Livro SET titulo = @Titulo, autor = @Autor, editora = @Editora, ano = @Ano, categoria = @Categoria, preco = @Preco, estoque = @Estoque  WHERE id = @Id", conexao);
                cmd.Parameters.AddWithValue("@Titulo", livro.Titulo);
                cmd.Parameters.AddWithValue("@Autor", livro.Autor);
                cmd.Parameters.AddWithValue("@Editora", livro.Editora);
                cmd.Parameters.AddWithValue("@Ano", livro.Ano);
                cmd.Parameters.AddWithValue("@Categoria", livro.Categoria);
                cmd.Parameters.AddWithValue("@Preco", livro.Preco);
                cmd.Parameters.AddWithValue("@Estoque", livro.Estoque);
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
                var cmd = new SqlCommand("DELETE FROM Livro WHERE id = @id", conexao);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }


        // Método para pesquisar livros pelo título
        public static List<Livro> PesquisarTitulo(string titulo)
        {
            var livros = new List<Livro>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM Livro WHERE Titulo LIKE @titulo", conexao);
                cmd.Parameters.AddWithValue("@Titulo", "%" + titulo + "%");
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        livros.Add(new Livro
                        {
                            Id = (int)reader["Id"],
                            Titulo = reader["Titulo"].ToString(),
                            Autor = reader["Autor"].ToString(),
                            Editora = reader["Editora"].ToString(),
                            Ano = (int)reader["Ano"],
                            Categoria = reader["Categoria"].ToString(),
                            Preco = (decimal)reader["Preco"],
                            Estoque = (int)reader["Estoque"]
                        });
                    }
                }
            }
            return livros;
        }

        // Método para atualizar o estoque de um livro
        public static int AtualizarEstoque(int id, int quantidade)
        {
            using (var conexao = Conexao.Conectar())
            {
                // Obter o estoque atual do livro
                var cmdSelect = new SqlCommand("SELECT Estoque FROM Livro WHERE Id = @Id", conexao);
                cmdSelect.Parameters.AddWithValue("@Id", id);
                int estoqueAtual = (int)cmdSelect.ExecuteScalar();

                //Tratamento de casos
                if (estoqueAtual < quantidade)
                {
                    return -1;
                }
                if (estoqueAtual == 0)
                {
                    return -2;
                }
        
                // Calcular novo estoque
                int novoEstoque = estoqueAtual - quantidade;
                
                // Atualizar o estoque no banco de dados
                var cmdUpdate = new SqlCommand("UPDATE Livro SET Estoque = @NovoEstoque WHERE Id = @Id", conexao);
                cmdUpdate.Parameters.AddWithValue("@NovoEstoque", novoEstoque);
                cmdUpdate.Parameters.AddWithValue("@Id", id);
                cmdUpdate.ExecuteNonQuery();
                return 1;
            }
        }

        public static List<Livro> PesquisarEstoque(int num)
        {
            var livros = new List<Livro>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM Livro WHERE Estoque <= @num", conexao);
                cmd.Parameters.AddWithValue("@num", num);
                
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        livros.Add(new Livro
                        {
                            Id = (int)reader["Id"],
                            Titulo = reader["Titulo"].ToString(),
                            Autor = reader["Autor"].ToString(),
                            Editora = reader["Editora"].ToString(),
                            Ano = (int)reader["Ano"],
                            Categoria = reader["Categoria"].ToString(),
                            Preco = (decimal)reader["Preco"],
                            Estoque = (int)reader["Estoque"]
                        });
                    }
                }
            }
            return livros;
        }
        public static List<Livro> PesquisarCategoria(string categoria)
        {
            var livros = new List<Livro>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM Livro WHERE Categoria = @categoria", conexao);
                cmd.Parameters.AddWithValue("@categoria", categoria);
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        livros.Add(new Livro
                        {
                            Id = (int)reader["Id"],
                            Titulo = reader["Titulo"].ToString(),
                            Autor = reader["Autor"].ToString(),
                            Editora = reader["Editora"].ToString(),
                            Ano = (int)reader["Ano"],
                            Categoria = reader["Categoria"].ToString(),
                            Preco = (decimal)reader["Preco"],
                            Estoque = (int)reader["Estoque"]
                        });
                    }
                }
            }
            return livros;
        }

        public static List<Livro> PesquisarEditora(string editora)
        {
            var livros = new List<Livro>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM Livro WHERE Editora LIKE @editora", conexao);
                cmd.Parameters.AddWithValue("@editora", "%" + editora + "%");
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        livros.Add(new Livro
                        {
                            Id = (int)reader["Id"],
                            Titulo = reader["Titulo"].ToString(),
                            Autor = reader["Autor"].ToString(),
                            Editora = reader["Editora"].ToString(),
                            Ano = (int)reader["Ano"],
                            Categoria = reader["Categoria"].ToString(),
                            Preco = (decimal)reader["Preco"],
                            Estoque = (int)reader["Estoque"]
                        });
                    }
                }
            }
            return livros;
        }

        public static List<Livro> Faixa_de_preco(decimal a, decimal b)
        {
            var livros = new List<Livro>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM Livro WHERE Preco BETWEEN @a AND @b ", conexao);
                cmd.Parameters.AddWithValue("@a", a);
                cmd.Parameters.AddWithValue("@b", b);
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        livros.Add(new Livro
                        {
                            Id = (int)reader["Id"],
                            Titulo = reader["Titulo"].ToString(),
                            Autor = reader["Autor"].ToString(),
                            Editora = reader["Editora"].ToString(),
                            Ano = (int)reader["Ano"],
                            Categoria = reader["Categoria"].ToString(),
                            Preco = (decimal)reader["Preco"],
                            Estoque = (int)reader["Estoque"]
                        });
                    }
                }
            }
            return livros;
        }

    }

}

