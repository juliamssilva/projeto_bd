using System.Collections.Generic;
using System.Data.SqlClient;
#nullable disable

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
                var cmd = new SqlCommand("SELECT * FROM Cliente", conexao);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clientes.Add(new Cliente
                        {
                            CpfCliente = reader["CpfCliente"].ToString(),
                            NomeCliente = reader["NomeCliente"].ToString(),
                            Email = reader["Email"].ToString(),
                            IsFlamengo = (bool)reader["IsFlamengo"],
                            IsOnePieceFan = (bool)reader["IsOnePieceFan"],
                            IsTeixeira = (bool)reader["IsTeixeira"]
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
                var cmd = new SqlCommand("UPDATE Cliente SET CpfCliente = @CpfCliente, NomeCliente = @NomeCliente, Email = @Email, IsFlamengo = @IsFlamengo, IsOnePieceFan = @IsOnePieceFan, IsTeixeira = @IsTeixeira where CpfCliente = @CpfCliente", conexao);
                cmd.Parameters.AddWithValue("@CpfCliente", cliente.CpfCliente);
                cmd.Parameters.AddWithValue("@NomeCliente", cliente.NomeCliente);
                cmd.Parameters.AddWithValue("@Email", cliente.Email);
                cmd.Parameters.AddWithValue("@IsFlamengo", cliente.IsFlamengo);
                cmd.Parameters.AddWithValue("@IsOnePieceFan", cliente.IsOnePieceFan);
                cmd.Parameters.AddWithValue("@IsTeixeira", cliente.IsTeixeira); 
                cmd.ExecuteNonQuery();
            }
        }

        public static void Deletar(string CpfCliente)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("DELETE FROM Cliente WHERE CpfCliente = @CpfCliente", conexao);
                cmd.Parameters.AddWithValue("@CpfCliente", CpfCliente);
                cmd.ExecuteNonQuery();
            }
        }

//Método para pesquisar clientes pelo CPF
        public static Cliente Pesquisar(string cpf)
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
                                CpfCliente = reader["CpfCliente"].ToString(),
                                NomeCliente = reader["NomeCliente"].ToString(),
                                Email = reader["Email"].ToString(),
                                IsFlamengo = (bool)reader["IsFlamengo"],
                                IsOnePieceFan = (bool)reader["IsOnePieceFan"],
                                IsTeixeira = (bool)reader["IsTeixeira"]
                            };
                        }
                    }
                }

                return cliente;
                }
    }
}
