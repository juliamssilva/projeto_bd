using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjCrud
{
    public static class clientesDAO
    {
        
        public static void Criar(Cliente cliente)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = conexao.CreateCommand();
                cmd.CommandText = "INSERT INTO Cliente (CpfCliente, NomeCliente, Email, IsFlamengo, IsOnePieceFan, IsTeixeira) VALUES (@CpfCliente, @NomeCliente, @Email, @IsFlamengo, @IsOnePieceFan, @IsTeixeira)";
                cmd.Parameters.AddWithValue("@CpfCliente", cliente.CpfCliente);
                cmd.Parameters.AddWithValue("@NomeCliente", cliente.NomeCliente);
                cmd.Parameters.AddWithValue("@Email", cliente.Email);
                cmd.Parameters.AddWithValue("@IsFlamengo", cliente.IsFlamengo);
                cmd.Parameters.AddWithValue("@IsOnePieceFan", cliente.IsOnePieceFan);
                cmd.Parameters.AddWithValue("@IsTeixeira", cliente.IsTeixeira);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Cliente> Ler()
        {
            var clientes = new List<Cliente>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM Livro", conexao);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clientes.Add(new Cliente
                        {
                            CpfCliente = reader.GetInt32(0),
                            NomeCliente = reader.GetString(1),
                            Email = reader.GetString(2),
                            IsFlamengo = reader.GetBoolean(3),
                            IsOnePieceFan = reader.GetBoolean(4),
                            IsTeixeira = reader.GetBoolean(5)
                        });
                    }
                }
            }

            return clientes;
        }

        public static void Atualizar(Cliente cliente)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("UPDATE Cliente SET CpfCliente = @CpfCliente, NomeCliente = @NomeCliente, Email = @Email, IsFlamengo = @IsFlamengo, IsOnePieceFan = @IsOnePieceFan, IsTeixeira = @IsTeixeira WHERE id = @Id", conexao);
                cmd.Parameters.AddWithValue("@CpfCliente", cliente.CpfCliente);
                cmd.Parameters.AddWithValue("@NomeCliente", cliente.NomeCliente);
                cmd.Parameters.AddWithValue("@Email", cliente.Email);
                cmd.Parameters.AddWithValue("@IsFlamengo", cliente.IsFlamengo);
                cmd.Parameters.AddWithValue("@IsOnePieceFan", cliente.IsOnePieceFan);
                cmd.Parameters.AddWithValue("@IsTeixeira", cliente.IsTeixeira); 
                cmd.ExecuteNonQuery();
            }
        }

        public static void Deletar(int id)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("DELETE FROM Cliente WHERE id = @id", conexao);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

//Método para pesquisar clientes pelo CPF
        public static Cliente Pesquisar(int cpf)
        {
            Cliente cliente = new Cliente();

                using (var conexao = Conexao.Conectar())
                {
                    var cmd = new SqlCommand("SELECT * FROM Cliente WHERE CpfCliente = @cpf", conexao);
                    cmd.Parameters.AddWithValue("@cpf", cpf);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new Cliente
                            {
                                CpfCliente = reader.GetInt32(0),
                                NomeCliente = reader.GetString(1),
                                Email = reader.GetString(2),
                                IsFlamengo = reader.GetBoolean(3),
                                IsOnePieceFan = reader.GetBoolean(4),
                                IsTeixeira = reader.GetBoolean(5)
                            };
                        }
                    }
                }

                return cliente;
                }
    }
}
