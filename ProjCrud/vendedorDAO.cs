using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjCrud
{
    public static class vendedorDAO
    {
        public static void Criar(Vendedor vendedor)
            {
                using (var conexao = Conexao.Conectar())
                {
                    var cmd = conexao.CreateCommand();
                    cmd.CommandText = "INSERT INTO Vendedor (NomeVendedor, Salario) VALUES (@NomeVendedor, @Salario)";
                    cmd.Parameters.AddWithValue("@NomeVendedor", vendedor.NomeVendedor);
                    cmd.Parameters.AddWithValue("@Salario", vendedor.Salario);
                    
                    cmd.ExecuteNonQuery();
                }
            }

        public static List<Vendedor> Ler()
        {
            var vendedores = new List<Vendedor>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM Vendedor", conexao);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vendedores.Add(new Vendedor
                        {
                            IdVendedor = (int)reader["IdVendedor"],
                            NomeVendedor = reader["NomeVendedor"].ToString(),
                            Salario = (decimal)reader["Salario"]
                        });
                    }
                }
            }

            return vendedores;
        }

        public static void Atualizar(Vendedor vendedor)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("UPDATE Vendedor SET NomeVendedor = @NomeVendedor, Salario = @Salario WHERE IdVendedor = @IdVendedor", conexao);
                cmd.Parameters.AddWithValue("@NomeVendedor", vendedor.NomeVendedor);
                cmd.Parameters.AddWithValue("@Salario", vendedor.Salario);
                cmd.ExecuteNonQuery();
            }
        }
        
        public static void Deletar(int IdVendedor)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("DELETE FROM Vendedor WHERE IdVendedor = @IdVendedor", conexao);
                cmd.Parameters.AddWithValue("@IdVendedor", IdVendedor);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Vendedor> Pesquisar(string NomeVendedor)
        {
            var vendedores = new List<Vendedor>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM Vendedor WHERE NomeVendedor LIKE @NomeVendedor", conexao);
                cmd.Parameters.AddWithValue("@NomeVendedor", "%" + NomeVendedor + "%");
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vendedores.Add(new Vendedor
                        {
                            IdVendedor = (int)reader["IdVendedor"],
                            NomeVendedor = reader["NomeVendedor"].ToString(),
                            Salario = (decimal)reader["Salario"]
                        });
                        
                    }
                }
            }
            return vendedores;
        }
    }
}

            