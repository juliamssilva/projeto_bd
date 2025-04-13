using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

#nullable disable
namespace ProjCrud
{
    public class ComprasDAO
    {

        public static List<Compra> Ler()
        {
            var compras = new List<Compra>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM Compra", conexao);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        compras.Add(new Compra
                        {
                            Id = (int)reader["Id"],
                            CpfCliente = reader["CpfCliente"].ToString(),
                            DataCompra = (DateTime)reader["DataCompra"],
                            IdVendedor = (int)reader["IdVendedor"],
                            Total = reader["Total"] == DBNull.Value ? 0m : (decimal)reader["Total"], // Aqui está o fix
                            FormaPagamento = reader["FormaPagamento"].ToString(),
                            StatusPagamento = reader["StatusPagamento"].ToString()
                        });
                    }
                }
            }
            return compras;
        }

        
        public static void Criar(Compra compra)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO Compra (CpfCliente, IdVendedor, DataCompra, FormaPagamento, StatusPagamento) 
                    VALUES (@cpfCliente, @idVendedor, @dataCompra, @formaPagamento, @statusPagamento)",
                    conexao);
                    
                cmd.Parameters.AddWithValue("@cpfCliente", compra.CpfCliente);
                cmd.Parameters.AddWithValue("@idVendedor", compra.IdVendedor);
                cmd.Parameters.AddWithValue("@dataCompra", compra.DataCompra);
                cmd.Parameters.AddWithValue("@formaPagamento", compra.FormaPagamento);
                cmd.Parameters.AddWithValue("@statusPagamento", compra.StatusPagamento);
                
                cmd.ExecuteNonQuery();
            }
        }

        public static void Atualizar(Compra compra)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand(@"
                    UPDATE Compra 
                    SET CpfCliente = @cpfCliente, IdVendedor = @idVendedor, DataCompra = @dataCompra, FormaPagamento = @formaPagamento, StatusPagamento = @statusPagamento
                    WHERE Id = @id",
                    conexao);
                    
                cmd.Parameters.AddWithValue("@id", compra.Id);
                cmd.Parameters.AddWithValue("@cpfCliente", compra.CpfCliente);
                cmd.Parameters.AddWithValue("@idVendedor", compra.IdVendedor);
                cmd.Parameters.AddWithValue("@dataCompra", compra.DataCompra);
                cmd.Parameters.AddWithValue("@formaPagamento", compra.FormaPagamento);
                cmd.Parameters.AddWithValue("@statusPagamento", compra.StatusPagamento);
                
                cmd.ExecuteNonQuery();
            }
        }

        public static List<ItemPedido> ItensPedido(int IdCompra)
        {
            var itens = new List<ItemPedido>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM ItemPedido WHERE IdCompra = @IdCompra", conexao);
                cmd.Parameters.AddWithValue("@IdCompra", IdCompra );
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itens.Add(new ItemPedido
                        {
                            Id = reader.GetInt32(0),
                            IdCompra = reader.GetInt32(1),
                            IdLivro = reader.GetInt32(2),
                            Quantidade = reader.GetInt32(3),
                            SubTotal = reader.GetDecimal(4),
                        }); 
                    }    
                }
            }
            return itens;
        }

        public static void CalculoTotal(int idCompra){
            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT SUM(SubTotal) FROM ItemPedido WHERE IdCompra = @IdCompra", conexao);
                cmd.Parameters.AddWithValue("@IdCompra", idCompra);
                object resultado = cmd.ExecuteScalar();
                var total = resultado == DBNull.Value ? 0m : (decimal)resultado;

                if (total == 0.0m)
                {
                    throw new Exception("Erro ao calcular o total da compra.");
                }

                var cmd2 = new SqlCommand("SELECT IsFlamengo FROM Cliente Cli, Compra C WHERE Cli.CpfCliente = C.CpfCliente and C.Id = @idCompra ", conexao);
                cmd2.Parameters.AddWithValue("@IdCompra", idCompra);
                object flamengoResult = cmd2.ExecuteScalar();
                bool flamengo = flamengoResult != DBNull.Value && (bool)flamengoResult;
                
                var cmd3 = new SqlCommand("SELECT IsOnePieceFan FROM Cliente Cli, Compra C WHERE Cli.CpfCliente = C.CpfCliente and C.Id = @idCompra ", conexao);
                cmd3.Parameters.AddWithValue("@IdCompra", idCompra); 
                object onepieceResult = cmd3.ExecuteScalar(); 
                bool onepiece = onepieceResult != DBNull.Value && (bool)onepieceResult; 
                
                
                var cmd4 = new SqlCommand("SELECT IsTeixeira FROM Cliente Cli, Compra C WHERE Cli.CpfCliente = C.CpfCliente and C.Id = @idCompra ", conexao);
                cmd4.Parameters.AddWithValue("@IdCompra", idCompra);
                object teixeiraResult = cmd4.ExecuteScalar(); 
                bool Teixeira = teixeiraResult != DBNull.Value && (bool)teixeiraResult; 

                //Desconto de 15% para flamengo, onepiece e Teixeira
                if (flamengo == true || onepiece == true || Teixeira == true) 
                {
                    total = total - (total * 0.15m);
                }

                // Atualiza o total na tabela Compra
                cmd.CommandText = "UPDATE Compra SET Total = @Total WHERE Id = @IdCompra";
                cmd.Parameters.AddWithValue("@Total", total);
                cmd.ExecuteNonQuery();
            }

        }

        public static List<ItemPedido> ItemPedidos(string CpfCliente)
        {
            var compras = new List<ItemPedido>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand(@"
                SELECT IP.* 
                FROM ItemPedido IP 
                JOIN Compra C ON IP.IdCompra = C.Id 
                WHERE C.CpfCliente = @CpfCliente", conexao);
                
            cmd.Parameters.AddWithValue("@CpfCliente", CpfCliente);
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        compras.Add(new ItemPedido
                        {
                            Id = reader.GetInt32(0),
                            IdCompra = reader.GetInt32(1),
                            IdLivro = reader.GetInt32(2),
                            Quantidade = reader.GetInt32(3),
                            SubTotal = reader.GetDecimal(4),
                        }); 
                    }    
                }
            }
            return compras;
        }

         public static List<ItemPedido> Vendas_Vendedor(int IdVendedor)
        {
            var vendas = new List<ItemPedido>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT IP.* FROM ItemPedido IP JOIN Compra C ON IP.IdCompra = C.Id WHERE C.IdVendedor = @IdVendedor", conexao);
                cmd.Parameters.AddWithValue("@IdVendedor", IdVendedor );
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vendas.Add(new ItemPedido
                        {
                            Id = reader.GetInt32(0),
                            IdCompra = reader.GetInt32(1),
                            IdLivro = reader.GetInt32(2),
                            Quantidade = reader.GetInt32(3),
                            SubTotal = reader.GetDecimal(4),
                        }); 
                    }    
                }
            }
            return vendas;
        }


    }
}




