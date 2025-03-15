using System.Collections.Generic;
using System.Data.SQLite;
using Avalonia.Animation.Easings;
using Avalonia.Automation.Peers;

namespace ProjCrud {

    public static class livroDAO{

        public static void Criar(Livro livro){

            //Conectar ao banco
            using (var conexao = Conexao.Conectar()){
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "INSERT INTO livros (titulo, autor, editora, ano) VALUES (@titulo, @autor, @editora, @ano)";
                cmd.Parameters.AddWithValue("@titulo", livro.Titulo);
                cmd.Parameters.AddWithValue("@autor", livro.Autor);
                cmd.Parameters.AddWithValue("@editora", livro.Editora);
                cmd.Parameters.AddWithValue("@ano", livro.Ano);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Livro> Ler(){
            var livros = new List<Livro>();

            using( var conexao = Conexao.Conectar()){

                var cmd = new SQLiteCommand("SELECT * FROM livros", conexao);
                using (var reader = cmd.ExecuteReader()){

                    while (reader.Read()){

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

        public static void Atualizar(Livro livro){

            using(var conexao = Conexao.Conectar()){
                var cmd = new SQLiteCommand("UPDATE livros SET titulo = @titulo, autor = @autor, editora = @editora, ano = @ano WHERE id = @id", conexao);
                cmd.Parameters.AddWithValue("@titulo", livro.Titulo);
                cmd.Parameters.AddWithValue("@autor", livro.Autor);
                cmd.Parameters.AddWithValue("@editora", livro.Editora);
                cmd.Parameters.AddWithValue("@ano", livro.Ano);
                cmd.Parameters.AddWithValue("@id", livro.Id);        
                cmd.ExecuteNonQuery();

            }
        }

        public static void Deletar(int id){

            using (var conexao = Conexao.Conectar()){
                var cmd = new SQLiteCommand("DELETE FROM Livro where id = @id", conexao);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

            }        
            
        }
    }

}

