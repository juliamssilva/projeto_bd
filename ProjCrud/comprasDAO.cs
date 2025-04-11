using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace ProjCrud
{
    public class ComprasDAO
    {
        public static void Comprar(Compra compra)
        {
            using (var conexao = Conexao.Conectar())
            {
                var cmd = conexao.CreateCommand();
                // Comando SQL para inserir um novo item de pedido na tabela ItemPedido
                cmd.CommandText = "INSERT INTO Compra (Id,CpfCliente, DataCompra, IdVendedor,Total, FormaPagamento, StatusPagamento) VALUES (@Id, @IdCliente, @IdVendedor, @FormaPagamento, @StatusPagamento)";
                cmd.Parameters.AddWithValue("@Id", compra.Id);
                cmd.Parameters.AddWithValue("@IdCliente", compra.CpfCliente);
                cmd.Parameters.AddWithValue("@IdVendedor", compra.IdVendedor);
                cmd.Parameters.AddWithValue("@DataCompra", compra.DataCompra);
                cmd.Parameters.AddWithValue("@Total", compra.Total);
                cmd.Parameters.AddWithValue("@FormaPagamento", compra.FormaPagamento);
                cmd.Parameters.AddWithValue("@StatusPagamento", compra.StatusPagamento);
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
                var total = (decimal)cmd.ExecuteScalar();

                if (total == 0.0m)
                {
                    throw new Exception("Erro ao calcular o total da compra.");
                }

                var cmd2 = new SqlCommand("SELECT IsFlamengo FROM Cliente Cli, Compra C WHERE Cli.CpfClient e = C.CpfCliente and IdCompra = @idCompra ", conexao);
                cmd2.Parameters.AddWithValue("@IdCompra", idCompra);
                var flamengo = (bool)cmd2.ExecuteScalar();
                
                var cmd3 = new SqlCommand("SELECT IsOnePieceFan FROM Cliente Cli, Compra C WHERE Cli.CpfClient e = C.CpfCliente and IdCompra = @idCompra ", conexao);
                cmd3.Parameters.AddWithValue("@IdCompra", idCompra);
                var onepiece = (bool)cmd3.ExecuteScalar();
                
                
                var cmd4 = new SqlCommand("SELECT IsTeixeira FROM Cliente Cli, Compra C WHERE Cli.CpfClient e = C.CpfCliente and IdCompra = @idCompra ", conexao);
                cmd4.Parameters.AddWithValue("@IdCompra", idCompra);
                var Teixeira = (bool)cmd4.ExecuteScalar();

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

        public static List<ItemPedido> CompraCliente(int CpfCliente)
        {
            var compras = new List<ItemPedido>();

            using (var conexao = Conexao.Conectar())
            {
                var cmd = new SqlCommand("SELECT * FROM ItemPedido WHERE CpfCliente = @CpfCliente", conexao);
                cmd.Parameters.AddWithValue("@CpfCliente", CpfCliente );
                
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
                var cmd = new SqlCommand("SELECT * FROM ItemPedido WHERE IdVendendor = @IdVendedor", conexao);
                cmd.Parameters.AddWithValue("@IdVendedpr", IdVendedor );
                
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




